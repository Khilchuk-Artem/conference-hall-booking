using System.Security.Cryptography;
using ConferenceBooking.Application.Abstractions.Idempotency;

namespace ConferenceBooking.Api.Idempotency;

public sealed class IdempotencyMiddleware
{
    private const int MaxKeyLength = 200;
    private static readonly TimeSpan Retention = TimeSpan.FromHours(24);
    private readonly RequestDelegate _next;

    public IdempotencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IIdempotencyStore idempotencyStore)
    {
        if (!HttpMethods.IsPost(context.Request.Method) ||
            context.GetEndpoint()?.Metadata.GetMetadata<IdempotencyAttribute>() is null)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var headerValues) ||
            headerValues.Count == 0)
        {
            await _next(context);
            return;
        }

        var key = headerValues[0]?.Trim();
        if (string.IsNullOrWhiteSpace(key))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Idempotency-Key must not be empty.", context.RequestAborted);
            return;
        }

        if (key.Length > MaxKeyLength)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Idempotency-Key must be at most {MaxKeyLength} characters.", context.RequestAborted);
            return;
        }

        context.Request.EnableBuffering();
        var requestHash = await context.Request.ComputeRequestHashAsync(context.RequestAborted);
        context.Request.Body.Position = 0;

        var now = DateTimeOffset.UtcNow;
        var operation = $"{context.Request.Method}:{context.Request.Path}";

        await using var transaction = await idempotencyStore.BeginTransactionAsync(context.RequestAborted);
        var claim = await idempotencyStore.TryAcquireAsync(
            operation,
            key,
            requestHash,
            now,
            now.Add(Retention),
            context.RequestAborted);

        if (!claim.IsNew)
        {
            await transaction.CommitAsync(context.RequestAborted);

            if (!CryptographicOperations.FixedTimeEquals(requestHash, claim.RequestHash))
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsync("The idempotency key was already used with a different request payload.", context.RequestAborted);
                return;
            }

            await context.Response.ReplayAsync(claim, context.RequestAborted);
            return;
        }

        var originalResponseBody = context.Response.Body;
        await using var responseBuffer = new MemoryStream();
        context.Response.Body = responseBuffer;

        try
        {
            await _next(context);

            if (context.Response.StatusCode >= StatusCodes.Status500InternalServerError)
            {
                await transaction.RollbackAsync(CancellationToken.None);
                await responseBuffer.CopyResponseAsync(originalResponseBody, context.RequestAborted);
                return;
            }

            await idempotencyStore.CompleteAsync(
                claim.Id,
                context.Response.StatusCode,
                context.Response.ContentType,
                responseBuffer.ToArray(),
                context.Response.Headers.Location,
                context.RequestAborted);
            await transaction.CommitAsync(context.RequestAborted);
            await responseBuffer.CopyResponseAsync(originalResponseBody, context.RequestAborted);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }
    }

}

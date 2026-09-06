using System.Buffers;
using System.Security.Cryptography;
using ConferenceBooking.Application.Abstractions.Idempotency;

namespace ConferenceBooking.Api.Idempotency;

internal static class IdempotencyHttpExtensions
{
    public static async Task<byte[]> ComputeRequestHashAsync(this HttpRequest request, CancellationToken cancellationToken)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        var buffer = ArrayPool<byte>.Shared.Rent(81920);

        try
        {
            int read;
            while ((read = await request.Body.ReadAsync(buffer.AsMemory(), cancellationToken)) > 0)
                hash.AppendData(buffer, 0, read);

            return hash.GetHashAndReset();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static async Task ReplayAsync(this HttpResponse response, IdempotencyClaim claim, CancellationToken cancellationToken)
    {
        response.StatusCode = claim.ResponseStatusCode ?? StatusCodes.Status200OK;
        response.ContentType = claim.ResponseContentType;
        if (!string.IsNullOrWhiteSpace(claim.Location))
            response.Headers.Location = claim.Location;

        response.ContentLength = claim.ResponseBody.Length;
        await response.Body.WriteAsync(claim.ResponseBody, cancellationToken);
    }

    public static async Task CopyResponseAsync(this MemoryStream responseBuffer, Stream originalResponseBody, CancellationToken cancellationToken)
    {
        responseBuffer.Position = 0;
        await responseBuffer.CopyToAsync(originalResponseBody, cancellationToken);
    }
}

namespace ConferenceBooking.Application.Abstractions.Idempotency;

public interface IIdempotencyStore
{
    Task<IIdempotencyTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<IdempotencyClaim> TryAcquireAsync(string operation, string key, byte[] requestHash, DateTimeOffset now, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);

    Task CompleteAsync(Guid id, int responseStatusCode, string? responseContentType, byte[] responseBody, string? location, CancellationToken cancellationToken = default);
}

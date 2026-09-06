namespace ConferenceBooking.Application.Abstractions.Idempotency;

public interface IIdempotencyTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

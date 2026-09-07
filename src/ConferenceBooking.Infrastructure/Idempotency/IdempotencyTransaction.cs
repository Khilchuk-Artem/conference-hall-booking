using ConferenceBooking.Application.Abstractions.Idempotency;
using Microsoft.EntityFrameworkCore.Storage;

namespace ConferenceBooking.Infrastructure.Idempotency;

public sealed class IdempotencyTransaction : IIdempotencyTransaction
{
    private readonly IDbContextTransaction _transaction;

    public IdempotencyTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.CommitAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.RollbackAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return _transaction.DisposeAsync();
    }
}

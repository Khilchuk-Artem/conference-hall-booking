using ConferenceBooking.Application.Abstractions.Idempotency;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Idempotency;

public sealed class IdempotencyStore : IIdempotencyStore
{
    private readonly ConferenceBookingDbContext _context;

    public IdempotencyStore(ConferenceBookingDbContext context)
    {
        _context = context;
    }

    public async Task<IIdempotencyTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new IdempotencyTransaction(transaction);
    }

    public async Task<IdempotencyClaim> TryAcquireAsync(string operation, string key, byte[] requestHash, DateTimeOffset now, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        var expiredRecords = await _context.IdempotencyKeys
            .Where(record => record.ExpiresAt <= now)
            .ToListAsync(cancellationToken);

        _context.IdempotencyKeys.RemoveRange(expiredRecords);

        var candidate = new IdempotencyKey
        {
            Id = Guid.NewGuid(),
            Operation = operation,
            Key = key,
            RequestHash = requestHash,
            ExpiresAt = expiresAt
        };
        _context.IdempotencyKeys.Add(candidate);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return ToClaim(candidate, isNew: true);
        }
        // another request may have claimed this key first
        catch (DbUpdateException)
        {
            _context.Entry(candidate).State = EntityState.Detached;

            var existing = await _context.IdempotencyKeys
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    record => record.Operation == operation && record.Key == key,
                    cancellationToken);

            if (existing is null)
            {
                throw;
            }

            return ToClaim(existing, isNew: false);
        }
    }

    public async Task CompleteAsync(Guid id, int responseStatusCode, string? responseContentType, byte[] responseBody, string? location, CancellationToken cancellationToken = default)
    {
        var record = await _context.IdempotencyKeys
            .SingleOrDefaultAsync(key => key.Id == id, cancellationToken);

        if (record is null)
        {
            throw new InvalidOperationException($"Idempotency key '{id}' could not be completed.");
        }

        record.ResponseStatusCode = responseStatusCode;
        record.ResponseContentType = responseContentType;
        record.ResponseBody = responseBody;
        record.Location = location;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static IdempotencyClaim ToClaim(IdempotencyKey record, bool isNew)
    {
        return new IdempotencyClaim
        {
            Id = record.Id,
            IsNew = isNew,
            RequestHash = record.RequestHash,
            ResponseStatusCode = record.ResponseStatusCode,
            ResponseContentType = record.ResponseContentType,
            ResponseBody = record.ResponseBody,
            Location = record.Location
        };
    }
}

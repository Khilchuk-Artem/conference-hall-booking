using ConferenceBooking.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace ConferenceBooking.UnitTests.Infrastructure;

public class IdempotencyTransactionTests
{
    [Fact]
    public async Task Transaction_ForwardsCommitRollbackAndDispose()
    {
        var transaction = new Mock<IDbContextTransaction>();
        var sut = new IdempotencyTransaction(transaction.Object);

        await sut.CommitAsync(CancellationToken.None);
        await sut.RollbackAsync(CancellationToken.None);
        await sut.DisposeAsync();

        transaction.Verify(item => item.CommitAsync(CancellationToken.None), Times.Once);
        transaction.Verify(item => item.RollbackAsync(CancellationToken.None), Times.Once);
        transaction.Verify(item => item.DisposeAsync(), Times.Once);
    }
}

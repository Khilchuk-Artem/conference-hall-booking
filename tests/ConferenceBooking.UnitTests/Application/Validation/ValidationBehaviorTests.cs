using ConferenceBooking.Application.Behaviors;
using FluentValidation;
using MediatR;
using Xunit;

namespace ConferenceBooking.UnitTests.Application.Validation;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithoutValidators_CallsNext()
    {
        var nextCalled = false;
        var behavior = new ValidationBehavior<TestRequest, string>([]);

        var result = await behavior.Handle(new TestRequest { Value = 1 }, _ =>
        {
            nextCalled = true;
            return Task.FromResult("handled");
        }, CancellationToken.None);

        Assert.True(nextCalled);
        Assert.Equal("handled", result);
    }

    [Fact]
    public async Task Handle_WithValidRequest_CallsNext()
    {
        var validator = new InlineValidator<TestRequest>();
        validator.RuleFor(request => request.Value).GreaterThan(0);
        var nextCalled = false;
        var behavior = new ValidationBehavior<TestRequest, string>([validator]);

        var result = await behavior.Handle(new TestRequest { Value = 1 }, _ =>
        {
            nextCalled = true;
            return Task.FromResult("handled");
        }, CancellationToken.None);

        Assert.True(nextCalled);
        Assert.Equal("handled", result);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ThrowsAndDoesNotCallNext()
    {
        var validator = new InlineValidator<TestRequest>();
        validator.RuleFor(request => request.Value).GreaterThan(0);
        var nextCalled = false;
        var behavior = new ValidationBehavior<TestRequest, string>([validator]);

        await Assert.ThrowsAsync<ValidationException>(() => behavior.Handle(new TestRequest { Value = 0 }, _ =>
        {
            nextCalled = true;
            return Task.FromResult("handled");
        }, CancellationToken.None));

        Assert.False(nextCalled);
    }

    private class TestRequest
    {
        public int Value { get; init; }
    }
}

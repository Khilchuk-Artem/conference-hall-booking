using FluentValidation;
using MediatR;

namespace ConferenceBooking.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validators = _validators.ToArray();

        if (validators.Length == 0)
        {
            return await next();
        }

        var validationResults = await Task.WhenAll(
            validators.Select(validator =>
                validator.ValidateAsync(
                    new ValidationContext<TRequest>(request),
                    cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
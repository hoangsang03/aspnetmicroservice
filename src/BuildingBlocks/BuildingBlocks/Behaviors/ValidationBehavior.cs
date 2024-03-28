using BuildingBlocks.CQRS;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        List<ValidationFailure> validationFailures =
            validationResults
            .SelectMany(v => v.Errors)
            .Where(f => f != null)
            .ToList();

        if (validationFailures.Count != 0)
            throw new ValidationException(validationFailures);

        return await next();
    }
}

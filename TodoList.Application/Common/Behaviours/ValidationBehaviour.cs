using ValidationException = TodoList.Application.Common.Exceptions.ValidationException;

namespace TodoList.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var validationContext = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));

            var failures = validationResults
                .Where(f => f.Errors.Count > 0)
                .SelectMany(e => e.Errors)
                .ToList();

            if (failures.Count > 0)
                throw new ValidationException(failures);
        }
        return await next();
    }
}
using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviour;

//This will collect all fluent validators and run before handler
public class ValidationBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    //IValidator, will return all the classes which implement this under _validators
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            //This runs all the validation rules one by one returns the validation result
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //Now, need to check for any failures
            var failures = validationResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

        }
        //On success, continue the mediator pipeline for the next step
        return await next();
    }
}


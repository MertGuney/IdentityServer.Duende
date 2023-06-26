namespace IdentityServer.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResponseModel<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    //TODO: Wrong error code
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

        var failures = results.SelectMany(x => x.Errors).Where(x => x is not null).ToList();

        if (!failures.Any())
        {
            return await next.Invoke();
        }

        var errors = failures.Select(f => new ErrorModel(1, f.PropertyName, f.ErrorMessage)).ToList();

        var response = await ResponseModel<TResponse>.FailureAsync(errors, StatusCodes.Status422UnprocessableEntity);

        return await Task.FromResult(response as TResponse);
    }
}
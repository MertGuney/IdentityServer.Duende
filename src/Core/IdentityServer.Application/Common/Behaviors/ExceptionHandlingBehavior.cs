namespace IdentityServer.Application.Common.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseModel<TResponse>
    where TException : Exception
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse, TException>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse, TException>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var response = CreateExceptionError(exception);

        _logger.LogError(JsonSerializer.Serialize(response));

        state.SetHandled(response as TResponse);

        return Task.FromResult(response);
    }
    //Wrong error code
    private static ErrorModel CreateExceptionError(TException exception)
    {
        var methodName = exception.TargetSite?.DeclaringType?.FullName;
        var message = exception.Message;
        var innerException = exception.InnerException?.Message;
        var stackTrace = exception.StackTrace;

        return new ErrorModel(FailureTypes.APPLICATION_EXCEPTION,
            $"Message: {message}, Method Name: {methodName}, Inner Exception: {innerException}, Stack Trace: {stackTrace}",
            Guid.NewGuid().ToString());
    }
}

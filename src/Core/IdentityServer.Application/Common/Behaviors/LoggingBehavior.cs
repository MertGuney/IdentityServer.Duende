namespace IdentityServer.Application.Common.Behaviors;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseModel<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestNameWithGuid = $"{request.GetType().Name} [{Guid.NewGuid()}] [User: {_currentUserService.UserId ?? string.Empty}]";

        _logger.LogInformation($"[Start] {requestNameWithGuid}");

        var stopwatch = Stopwatch.StartNew();

        try
        {
            LogRequestWithProps(request, requestNameWithGuid);

            return await next();
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation($"[End] {requestNameWithGuid} Execution Time = {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    private void LogRequestWithProps(TRequest request, string requestNameWithGuid)
    {
        try
        {
            _logger.LogInformation($"[PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(request)}");
        }
        catch (NotSupportedException)
        {
            _logger.LogInformation($"[Serialization ERROR] {requestNameWithGuid} Could not serialize the request.");
        }
    }
}
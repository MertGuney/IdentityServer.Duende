namespace IdentityServer.Application.Common.Extensions;
public class ExceptionHandlingMiddleware : IMiddleware
{
    private const int ErrorCode = 2000;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var errors = GetErrors(exception);

        var response = await ResponseModel<NoContentModel>.FailureAsync(errors, statusCode);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception)
        => exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception)
        => exception switch
        {
            CustomApplicationException applicationException => applicationException.Title,
            _ => "Server Error"
        };

    private static List<ErrorModel> GetErrors(Exception exception)
    {
        List<ErrorModel> errors = new()
            {
                new ErrorModel(ErrorCode, GetTitle(exception), exception.Message)
            };
        return errors;
    }
}
namespace IdentityServer.Application.Common.ActionFilters;

public class ValidationFilterAttribute : IAsyncActionFilter
{
    private const int ValidationErrorCode = 4000;
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .Where(x => x.Errors.Count > 0)
                .SelectMany(x => x.Errors)
                .Select(x => new ErrorModel(ValidationErrorCode, $"Invalid Parameter", x.ErrorMessage)).ToList();
            context.Result = new BadRequestObjectResult(await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status422UnprocessableEntity));
        }
        else
        {
            await next();
        }
    }
}

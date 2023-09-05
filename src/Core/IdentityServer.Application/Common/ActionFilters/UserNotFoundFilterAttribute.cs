namespace IdentityServer.Application.Common.ActionFilters;

public class UserNotFoundFilterAttribute : IAsyncActionFilter
{
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public UserNotFoundFilterAttribute(UserManager<User> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user is not null)
            {
                await next.Invoke();
                return;
            }
            else
            {
                context.Result = new NotFoundObjectResult(ResponseModel<NoContentModel>.UserNotFound());
            }
        }
        else
        {
            context.Result = new BadRequestObjectResult(ResponseModel<NoContentModel>.UserNotFound());
        }
    }
}
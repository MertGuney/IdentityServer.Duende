namespace IdentityServer.Api.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [NonAction]
    public static IActionResult ActionResultInstance<T>(ResponseModel<T> response)
        => new ObjectResult(response)
        {
            StatusCode = response.StatusCode,
        };
}

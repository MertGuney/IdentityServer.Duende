namespace IdentityServer.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(LocalApi.PolicyName)]
[Route("api/v{version:apiVersion}/users/[action]")]
public class UsersController : BaseController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Current()
        => ActionResultInstance(await _mediator.Send(new GetCurrentUserQueryRequest()));

    [HttpPut]
    public async Task<IActionResult> ChangeEmail(ChangeEmailCommandRequest request)
    => ActionResultInstance(await _mediator.Send(request));

    [HttpPut]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));
}

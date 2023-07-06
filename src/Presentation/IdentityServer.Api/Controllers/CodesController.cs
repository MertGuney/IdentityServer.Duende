namespace IdentityServer.Api.Controllers;

[Route("api/codes/[action]")]
[ApiController]
public class CodesController : BaseController
{
    public CodesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Send(SendRegisterCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> Send(SendChangeEmailCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> Send(SendChangePasswordCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> Send(SendForgotPasswordCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));
}

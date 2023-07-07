namespace IdentityServer.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/codes/[action]")]
public class CodesController : BaseController
{
    public CodesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> SendRegisterCode(SendRegisterCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> SendChangeEmailCode(SendChangeEmailCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> SendForgotPasswordCode(SendForgotPasswordCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));
}

namespace IdentityServer.Api.Controllers;

[Route("api/codes/[action]")]
[ApiController]
public class CodesController : BaseController
{
    public CodesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Send(SendCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> Verify(VerifyCodeCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));
}

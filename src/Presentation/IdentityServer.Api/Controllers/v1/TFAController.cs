namespace IdentityServer.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = LocalApi.PolicyName)]
[Route("api/v{version:apiVersion}/tfa/[action]")]
public class TFAController : BaseController
{
    public TFAController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Enable()
        => ActionResultInstance(await _mediator.Send(new EnableTFACommandRequest()));

    [HttpPost]
    public async Task<IActionResult> Activate(ActivateTFACommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));

    [HttpPost]
    public async Task<IActionResult> Deactivate(DeactivateTFACommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));
}

namespace IdentityServer.Api.Controllers;

[Route("api/tfa/[action]")]
[ApiController]
[Authorize(LocalApi.PolicyName)]
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

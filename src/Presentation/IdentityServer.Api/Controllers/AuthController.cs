using IdentityServer.Application.Features.Commands.Auth.Register;

namespace IdentityServer.Api.Controllers;

[Route("api/auth/[action]")]
[ApiController]
public class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCommandRequest request)
        => ActionResultInstance(await _mediator.Send(request));
}


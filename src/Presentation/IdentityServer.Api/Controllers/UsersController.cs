using IdentityServer.Application.Features.Queries.Users.GetCurrentUser;

namespace IdentityServer.Api.Controllers
{
    [Route("api/users/[action]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Current()
            => ActionResultInstance(await _mediator.Send(new GetCurrentUserQueryRequest()));

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommandRequest request)
            => ActionResultInstance(await _mediator.Send(request));
    }
}

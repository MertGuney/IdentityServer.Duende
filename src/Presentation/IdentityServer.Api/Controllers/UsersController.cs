namespace IdentityServer.Api.Controllers
{
    [Route("api/users/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
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

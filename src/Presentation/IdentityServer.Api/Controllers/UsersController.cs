namespace IdentityServer.Api.Controllers
{
    [Route("api/users/[action]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommandRequest request)
            => ActionResultInstance(await _mediator.Send(request));
    }
}

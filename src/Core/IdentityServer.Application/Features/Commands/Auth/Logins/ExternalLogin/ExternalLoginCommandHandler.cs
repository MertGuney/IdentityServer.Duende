namespace IdentityServer.Application.Features.Commands.Auth.Logins.ExternalLogin;

public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommandRequest, ExternalLoginCommandResponse>
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public ExternalLoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
    }

    public async Task<ExternalLoginCommandResponse> Handle(ExternalLoginCommandRequest request, CancellationToken cancellationToken)
    {
        ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync() ?? throw new ArgumentNullException();

        User user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null)
        {
            IdentityResult loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
            if (loginResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
        }
        else
        {
            user = new(request.Email, request.Email);
            IdentityResult createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                IdentityResult loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                if (loginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }
            }
        }
        return new ExternalLoginCommandResponse();
    }
}

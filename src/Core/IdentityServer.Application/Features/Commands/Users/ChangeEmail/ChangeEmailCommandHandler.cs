namespace IdentityServer.Application.Features.Commands.Users.ChangeEmail;

public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ICodeService _codeService;

    public ChangeEmailCommandHandler(IUserService userService, IAuthService authService, ICodeService codeService)
    {
        _userService = userService;
        _authService = authService;
        _codeService = codeService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ChangeEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetAsync();

        var isCodeVerified = await _codeService.VerifyAsync(user.Id, request.Code, CodeTypeEnum.ChangeEmail, cancellationToken);
        if (!isCodeVerified) return await ResponseModel<NoContentModel>.FailureAsync(1,
            "Unverified code",
            "Code unverified",
            StatusCodes.Status400BadRequest);

        var changeEmailResponse = await _authService.ChangeEmailAsync(user, request.NewEmail);
        if (!changeEmailResponse.IsSuccessful) return changeEmailResponse;

        return await _authService.ConfirmEmailAsync(user);
    }
}
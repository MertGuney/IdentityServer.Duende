namespace IdentityServer.Application.Features.Commands.Auth.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IAuthService _authService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;

    public RegisterCommandHandler(IAuthService authService, ICodeService codeService, IMailService mailService)
    {
        _authService = authService;
        _codeService = codeService;
        _mailService = mailService;
    }
    //TODO: Wrong error code
    public async Task<ResponseModel<NoContentModel>> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        User user = new(request.Email, request.UserName);

        var registerResponse = await _authService.RegisterAsync(user, request.Password);
        if (!registerResponse.IsSuccessful) return registerResponse;

        var addUserToRoleResponse = await _authService.AddToRoleAsync(user, "Customer");
        if (!addUserToRoleResponse.IsSuccessful) return addUserToRoleResponse;

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.Register, cancellationToken);

        return await _mailService.SendEmailConfirmationMailAsync(user.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync(StatusCodes.Status201Created)
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

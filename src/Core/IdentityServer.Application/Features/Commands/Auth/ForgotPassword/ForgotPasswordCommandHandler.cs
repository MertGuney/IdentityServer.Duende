namespace IdentityServer.Application.Features.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;

    public ForgotPasswordCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetByEmailAsync(request.Email);

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.ForgotPassword, cancellationToken);

        return await _mailService.SendForgotPasswordMailAsync(user.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

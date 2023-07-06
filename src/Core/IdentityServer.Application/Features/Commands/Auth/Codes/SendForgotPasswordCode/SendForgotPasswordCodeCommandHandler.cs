namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendForgotPasswordCode;

public class SendForgotPasswordCodeCommandHandler : IRequestHandler<SendForgotPasswordCodeCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;
    public SendForgotPasswordCodeCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(SendForgotPasswordCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.ForgotPassword, cancellationToken);

        return await _mailService.SendForgotPasswordMailAsync(request.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

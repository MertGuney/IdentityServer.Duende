namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendChangePasswordCode;

public class SendChangePasswordCodeCommandHandler : IRequestHandler<SendChangePasswordCodeCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;
    public SendChangePasswordCodeCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(SendChangePasswordCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.ChangePassword, cancellationToken);

        return await _mailService.SendChangePasswordMailAsync(request.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

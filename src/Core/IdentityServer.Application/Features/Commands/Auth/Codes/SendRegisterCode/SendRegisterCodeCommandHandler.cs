namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendRegisterCode;

public class SendRegisterCodeCommandHandler : IRequestHandler<SendRegisterCodeCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;

    public SendRegisterCodeCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(SendRegisterCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.Register, cancellationToken);

        return await _mailService.SendEmailConfirmationMailAsync(request.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

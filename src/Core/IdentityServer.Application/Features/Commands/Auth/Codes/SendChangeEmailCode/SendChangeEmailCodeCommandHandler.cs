namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendChangeEmailCode;

public class SendChangeEmailCodeCommandHandler : IRequestHandler<SendChangeEmailCodeCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;
    public SendChangeEmailCodeCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(SendChangeEmailCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var isExistEmail = await _userService.GetByEmailAsync(request.NewEmail);
        if (isExistEmail is not null)
        {
            return await ResponseModel<NoContentModel>.FailureAsync(1,
                "Registered email address.",
                "This mail address is registered.",
                StatusCodes.Status400BadRequest);
        }

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.ChangeEmail, cancellationToken);

        return await _mailService.SendChangeEmailConfirmationMailAsync(request.NewEmail, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

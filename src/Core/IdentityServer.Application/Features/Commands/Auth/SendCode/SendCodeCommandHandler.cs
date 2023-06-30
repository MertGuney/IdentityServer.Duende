namespace IdentityServer.Application.Features.Commands.Auth.SendCode;

public class SendCodeCommandHandler : IRequestHandler<SendCodeCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;

    public SendCodeCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(SendCodeCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var code = await _codeService.GenerateAsync(user.Id, request.CodeType, cancellationToken);

        return await _mailService.SendAsync(user.Email, user.Id, code, request.CodeType)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}
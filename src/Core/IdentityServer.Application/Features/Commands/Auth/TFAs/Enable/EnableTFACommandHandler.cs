namespace IdentityServer.Application.Features.Commands.Auth.TFAs.Enable;

public class EnableTFACommandHandler : IRequestHandler<EnableTFACommandRequest, ResponseModel<EnableTFACommandResponse>>
{
    private readonly ITFAService _tfaService;
    private readonly IUserService _userService;
    private readonly IQRCodeService _qrCodeService;

    public EnableTFACommandHandler(ITFAService tfaService, IUserService userService, IQRCodeService qrCodeService)
    {
        _tfaService = tfaService;
        _userService = userService;
        _qrCodeService = qrCodeService;
    }

    public async Task<ResponseModel<EnableTFACommandResponse>> Handle(EnableTFACommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetAsync();

        var isTFAEnabled = await _tfaService.IsEnabledAsync(user);
        if (isTFAEnabled)
            return await ResponseModel<EnableTFACommandResponse>.FailureAsync(1,
                "TFAEnabled",
                "Two factor authentication is already enabled on this account",
                StatusCodes.Status400BadRequest);

        var authenticatorKey = await _tfaService.GetAuthenticatorKeyAsync(user);
        if (authenticatorKey is null)
        {
            var resetAuthenticatorKeyResult = await _tfaService.ResetAuthenticatorKeyAsync(user);
            if (!resetAuthenticatorKeyResult.IsSuccessful)
                return await ResponseModel<EnableTFACommandResponse>.FailureAsync(
                    resetAuthenticatorKeyResult.Errors,
                    resetAuthenticatorKeyResult.StatusCode);

            authenticatorKey = await _tfaService.GetAuthenticatorKeyAsync(user);
        }

        var formattedKey = _qrCodeService.Generate(user.Email, authenticatorKey);

        return await ResponseModel<EnableTFACommandResponse>.SuccessAsync(new EnableTFACommandResponse(formattedKey, authenticatorKey));
    }
}

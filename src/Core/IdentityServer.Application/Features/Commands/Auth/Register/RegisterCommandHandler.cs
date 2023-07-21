using IdentityServer.Application.Common.Constants;

namespace IdentityServer.Application.Features.Commands.Auth.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;
    private readonly IMailService _mailService;

    public RegisterCommandHandler(IUserService userService, ICodeService codeService, IMailService mailService)
    {
        _userService = userService;
        _codeService = codeService;
        _mailService = mailService;
    }

    //TODO: Wrong error code
    public async Task<ResponseModel<NoContentModel>> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        User user = new(request.Email, request.UserName);

        var createResponse = await _userService.CreateAsync(user, request.Password);
        if (!createResponse.IsSuccessful) return createResponse;

        var addUserToRoleResponse = await _userService.AddToRoleAsync(user, RoleConstants.Customer);
        if (!addUserToRoleResponse.IsSuccessful) return addUserToRoleResponse;

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.Register, cancellationToken);

        return await _mailService.SendEmailConfirmationMailAsync(user.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync(StatusCodes.Status201Created)
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }
}

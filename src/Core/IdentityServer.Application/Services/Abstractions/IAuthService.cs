namespace IdentityServer.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ResponseModel<NoContentModel>> ConfirmEmailAsync(User user);

        Task<ResponseModel<NoContentModel>> UpdateSecurityStampAsync(User user);

        Task<ResponseModel<NoContentModel>> ChangeEmailAsync(User user, string newEmail);

        Task<ResponseModel<NoContentModel>> ResetPasswordAsync(User user, string newPassword);

        Task<ResponseModel<NoContentModel>> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}

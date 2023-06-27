namespace IdentityServer.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<User> FindByEmailAsync(string email);

        Task<ResponseModel<NoContentModel>> UpdateSecurityStampAsync(User user);

        Task<ResponseModel<NoContentModel>> AddToRoleAsync(User user, string role);

        Task<ResponseModel<NoContentModel>> RegisterAsync(User user, string password);

        Task<ResponseModel<NoContentModel>> ResetPasswordAsync(User user, string newPassword);

        Task<ResponseModel<NoContentModel>> ForgotPasswordAsync(string email, CancellationToken cancellationToken);

        Task<ResponseModel<NoContentModel>> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}

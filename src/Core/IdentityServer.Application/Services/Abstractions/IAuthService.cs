namespace IdentityServer.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<bool> SendEmailConfirmationTokenAsync(User user);
        Task<ResponseModel<NoContentModel>> AddToRoleAsync(User user, string role);
        Task<ResponseModel<NoContentModel>> RegisterAsync(User user, string password);
        //Task<bool> ResetPasswordAsync(string email, string code, string newPassword, CancellationToken cancellationToken);
    }
}

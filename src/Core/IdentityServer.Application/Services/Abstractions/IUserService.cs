namespace IdentityServer.Application.Services.Abstractions;

public interface IUserService
{
    Task<User> GetAsync();

    Task<User> GetByEmailAsync(string email);

    Task<User> GetByUserNameAsync(string username);

    Task<ResponseModel<NoContentModel>> UpdateAsync(User User);

    Task<ResponseModel<NoContentModel>> VerifyEmailAsync(User user);

    Task<ResponseModel<NoContentModel>> AddToRoleAsync(User user, string role);

    Task<ResponseModel<NoContentModel>> CreateAsync(User user, string password);

    Task<ResponseModel<NoContentModel>> CreateLoginAsync(User user, UserLoginInfo loginInfo);
}
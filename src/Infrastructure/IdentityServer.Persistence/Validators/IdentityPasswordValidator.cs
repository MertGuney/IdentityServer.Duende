namespace IdentityServer.Persistence.Validators;
public class IdentityPasswordValidator : IPasswordValidator<User>
{
    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
    {
        List<IdentityError> errors = new();
        if (password!.ToLower().Contains(user.UserName!.ToLower()))
        {
            errors.Add(new() { Code = "PasswordContainUserName", Description = "Password field can not contain UserName" });
        }

        if (errors.Any())
        {
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}

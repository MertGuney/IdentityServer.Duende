namespace IdentityServer.Persistence.Validators;
public class IdentityUserValidator : IUserValidator<User>
{
    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        List<IdentityError> errors = new();
        var isDigit = int.TryParse(user.UserName![0].ToString(), out _);

        if (isDigit)
        {
            errors.Add(new() { Code = "UserNameContainFirstLetterDigit", Description = "The first character of the username can not contain a numeric character." });
        }

        return errors.Any()
            ? Task.FromResult(IdentityResult.Failed(errors.ToArray()))
            : Task.FromResult(IdentityResult.Success);
    }
}

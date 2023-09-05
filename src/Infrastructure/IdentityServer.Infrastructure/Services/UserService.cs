using IdentityServer.Shared.Enums;

namespace IdentityServer.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public UserService(ILogger<UserService> logger, UserManager<User> userManager, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<User> GetAsync()
    {
        return await _userManager.FindByIdAsync(_currentUserService.UserId);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User> GetByUserNameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<ResponseModel<NoContentModel>> VerifyEmailAsync(User user)
    {
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(FailureTypes.VERIFY_EMAIL_FAILED, error.Description, Guid.NewGuid().ToString()));
                _logger.LogWarning($"An error occurred while updating to the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> UpdateAsync(User user)
    {
        IdentityResult result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(FailureTypes.UPDATE_USER_FAILED, error.Description, Guid.NewGuid().ToString()));
                _logger.LogWarning($"An error occurred while updating to the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> CreateAsync(User user, string password)
    {
        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(FailureTypes.CREATE_USER_FAILED, error.Description, Guid.NewGuid().ToString()));
                _logger.LogWarning($"An error occurred while creating the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync(StatusCodes.Status201Created);
    }

    public async Task<ResponseModel<NoContentModel>> CreateLoginAsync(User user, UserLoginInfo loginInfo)
    {
        IdentityResult result = await _userManager.AddLoginAsync(user, loginInfo);

        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(FailureTypes.CREATE_EXTERNAL_LOGIN_FAILED, error.Description, Guid.NewGuid().ToString()));
                _logger.LogWarning($"An error occurred while creating the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync(StatusCodes.Status201Created);
    }

    public async Task<ResponseModel<NoContentModel>> AddToRoleAsync(User user, string role)
    {
        IdentityResult result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(FailureTypes.ADD_ROLE_TO_USER_FAILED, error.Description, Guid.NewGuid().ToString()));
                _logger.LogWarning($"An error occurred while adding the role to the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }
}
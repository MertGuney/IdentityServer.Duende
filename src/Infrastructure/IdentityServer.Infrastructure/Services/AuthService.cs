﻿using IdentityServer.Domain.Enums;

namespace IdentityServer.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly IMailService _mailService;
    private readonly ICodeService _codeService;
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;

    public AuthService(IMailService mailService, ICodeService codeService, ILogger<AuthService> logger, UserManager<User> userManager)
    {
        _mailService = mailService;
        _codeService = codeService;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<User> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ResponseModel<NoContentModel>> RegisterAsync(User user, string password)
    {
        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
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
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while adding the role to the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByEmailAsync(email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var code = await _codeService.GenerateAsync(user.Id, CodeTypeEnum.ForgotPassword, cancellationToken);

        return await _mailService.SendForgotPasswordMailAsync(user.Email, user.Id, code)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }

    public async Task<ResponseModel<NoContentModel>> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var isExistPassword = await _userManager.CheckPasswordAsync(user, currentPassword);
        if (!isExistPassword) return await ResponseModel<NoContentModel>.FailureAsync(1, "CurrentPasswordNotMatch", "Current password does not match", StatusCodes.Status404NotFound);

        IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!changePasswordResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in changePasswordResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while changing the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }


    public async Task<ResponseModel<NoContentModel>> ResetPasswordAsync(User user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        IdentityResult resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!resetPasswordResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in resetPasswordResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while resetting the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> UpdateSecurityStampAsync(User user)
    {
        IdentityResult securityStampResult = await _userManager.UpdateSecurityStampAsync(user);
        if (!securityStampResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in securityStampResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while the security updateding. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }
}

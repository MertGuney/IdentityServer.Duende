namespace IdentityServer.Application.Services.Abstractions;
public interface ICurrentUserService
{
    public string UserId { get; }
}
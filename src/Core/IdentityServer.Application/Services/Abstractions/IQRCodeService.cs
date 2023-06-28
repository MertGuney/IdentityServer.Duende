namespace IdentityServer.Application.Services.Abstractions;

public interface IQRCodeService
{
    byte[] Generate(string text);
    string Generate(string email, string unformattedKey);
}

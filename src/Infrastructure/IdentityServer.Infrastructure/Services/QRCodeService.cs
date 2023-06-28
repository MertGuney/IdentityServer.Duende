namespace IdentityServer.Infrastructure.Services;

public class QRCodeService : IQRCodeService
{
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public byte[] Generate(string text)
    {
        QRCodeGenerator generator = new();
        QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new(data);
        return qrCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
    }

    public string Generate(string email, string unformattedKey)
    {
        return string.Format(AuthenticatorUriFormat,
            CryptographyExtensions.UrlEncode("Two-Factor Auth"),
            CryptographyExtensions.UrlEncode(email),
            unformattedKey);
    }
}

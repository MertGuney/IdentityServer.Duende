namespace IdentityServer.Application.Common.Utilities.Regexs;

public static class MailRegex
{
    public static readonly string EmailValidation_Regex = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

    public static readonly Regex EmailValidation_Regex_Compiled = new(EmailValidation_Regex, RegexOptions.IgnoreCase);

    public static readonly string EmailValidation_Regex_JS = $"/{EmailValidation_Regex}/";

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email,
                @"(@)(.+)$",
                DomainMapper,
                RegexOptions.None,
                TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public static bool IsValidEmailAddress(string email, bool useRegEx = false, bool requireDotInDomainName = false)
    {
        var isValid = useRegEx
            ? email is not null && EmailValidation_Regex_Compiled.IsMatch(email)
            : new EmailAddressAttribute().IsValid(email);

        if (isValid && requireDotInDomainName)
        {
            var arr = email.Split('@', StringSplitOptions.RemoveEmptyEntries);
            isValid = arr.Length == 2 && arr[1].Contains('.');
        }
        return isValid;
    }

    public static bool ValidateMailAddress(string emailAddress)
    {
        try
        {
            var email = new MailAddress(emailAddress);
            return email.Address == emailAddress.Trim();
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ValidateMailAddressUsingAttribute(string emailAddress)
    {
        var emailValidation = new EmailAddressAttribute();
        return emailValidation.IsValid(emailAddress);
    }

    public static bool ValidateMailAddressUsingRegex(string emailAddress)
    {
        var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
        var regex = new Regex(pattern);
        return regex.IsMatch(emailAddress);
    }
}

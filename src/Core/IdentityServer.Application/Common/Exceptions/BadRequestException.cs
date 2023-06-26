namespace IdentityServer.Application.Common.Exceptions;
public class BadRequestException : Exception
{
    public List<string> Errors { get; set; }

    public BadRequestException() : base()
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, Exception exception) : base(message, exception)
    {
    }

    public BadRequestException(List<string> errors) : base("Multiple errors occurred. See error details.")
    {
        Errors = errors;
    }
}

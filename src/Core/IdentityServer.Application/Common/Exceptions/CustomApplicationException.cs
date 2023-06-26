using System.Globalization;

namespace IdentityServer.Application.Common.Exceptions;

public class CustomApplicationException : Exception
{
    public string Title { get; set; }
    public CustomApplicationException() : base()
    {
    }

    public CustomApplicationException(string message) : base(message)
    {
    }

    public CustomApplicationException(string message, Exception exception) : base(message, exception)
    {
    }

    public CustomApplicationException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
    public CustomApplicationException(string title, string message) : base(message)
    {
        Title = title;
    }
}

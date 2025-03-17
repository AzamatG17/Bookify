namespace Bookify.Domain_.Exceptions;

public class InvalidUserAgentException : ApplicationException
{
    public InvalidUserAgentException(string message) : base(message) { }
}

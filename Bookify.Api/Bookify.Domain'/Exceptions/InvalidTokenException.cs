namespace Bookify.Domain_.Exceptions;

public class InvalidTokenException : ApplicationException
{
    public InvalidTokenException(string message) : base(message) { }
}

namespace Bookify.Domain_.Exceptions;

public class UnauthorizedAccessException : ApplicationException
{
    public UnauthorizedAccessException(string message) :base(message) { }
}

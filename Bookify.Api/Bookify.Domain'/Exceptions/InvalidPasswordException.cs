namespace Bookify.Domain_.Exceptions;

public sealed class InvalidPasswordException : ApplicationException
{
    public InvalidPasswordException(string message) : base(message) { }  
}

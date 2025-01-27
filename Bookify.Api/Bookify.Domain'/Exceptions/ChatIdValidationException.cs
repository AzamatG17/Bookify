namespace Bookify.Domain_.Exceptions;

public class ChatIdValidationException : ApplicationException
{
    public ChatIdValidationException(string message) : base(message) { }
}

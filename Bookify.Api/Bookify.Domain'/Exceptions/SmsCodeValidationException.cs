namespace Bookify.Domain_.Exceptions;

public class SmsCodeValidationException : ApplicationException
{
    public SmsCodeValidationException(string message) : base(message) { }
}

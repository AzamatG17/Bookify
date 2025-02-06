namespace Bookify.Domain_.Exceptions;

public class DuplicateBookingException : ApplicationException
{
    public DuplicateBookingException(string message) :base(message) { }
}

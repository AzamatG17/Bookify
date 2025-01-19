namespace Bookify.Domain_.Exceptions;

public class UserNameAlreadyTakenException : ApplicationException
{
    public UserNameAlreadyTakenException(string message) :base(message) { }
}

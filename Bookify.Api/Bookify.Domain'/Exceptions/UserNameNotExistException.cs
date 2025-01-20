namespace Bookify.Domain_.Exceptions;

public class UserNameNotExistException :ApplicationException
{
    public UserNameNotExistException(string message) : base(message) { }
}

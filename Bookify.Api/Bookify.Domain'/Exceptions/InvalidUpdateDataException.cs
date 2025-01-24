namespace Bookify.Domain_.Exceptions;

public class InvalidUpdateDataException : ApplicationException
{
    public InvalidUpdateDataException(string messager) : base(messager) { }
    public InvalidUpdateDataException(string messager, Exception ex) : base(messager, ex) { }
}

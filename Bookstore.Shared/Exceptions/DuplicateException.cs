namespace Bookstore.Shared.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string message) : base(message) { }
}

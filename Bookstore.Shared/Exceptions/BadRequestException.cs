namespace Bookstore.Shared.Exceptions;

public class BadRequestException : Exception
{
    public Dictionary<string, string[]> Errors { get; set; }
    public BadRequestException(string message) : base(message) { }

    public BadRequestException(Dictionary<string, string[]> errors) : base("")
    {
        Errors = errors;
    }
}

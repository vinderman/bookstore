namespace Bookstore.WebApi.Common;

public class ErrorResponse : ApiResponse
{
    /// <summary>
    /// Системное сообщение об ошибке
    /// </summary>
    public string Description { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }


    public ErrorResponse()
    {
        Success = false;
    }
}

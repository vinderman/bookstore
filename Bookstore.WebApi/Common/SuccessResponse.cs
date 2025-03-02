using System.Text.Json.Serialization;

namespace Bookstore.WebApi.Common;

/// <summary>
/// Базовая обертка для ответа API
/// </summary>
/// <typeparam name="T"></typeparam>
public class SuccessResponse<TData> : ApiResponse
{

    /// <summary>
    /// Результат запроса / детализация ошибки
    /// </summary>
    [JsonPropertyName("data")]
    public TData Data { get; set; }

    public SuccessResponse(TData data, string message = "")
    {
        Data = data;
        Message = message;
        Success = true;
    }
}

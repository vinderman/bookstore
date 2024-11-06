using System.Text.Json.Serialization;

namespace Bookstore.WebApi.Common;

/// <summary>
/// Базовая обертка для ответа API
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse
{
    /// <summary>
    /// Статус успешности запроса
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; } = true;

    /// <summary>
    /// Cообщение об успехе операции/ошибке
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    public ApiResponse() { }
}

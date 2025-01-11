using Bookstore.Shared.Exceptions;
using Bookstore.WebApi.Common;

namespace Bookstore.WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DuplicateException ex)
        {
            var response = new ErrorResponse { Message = "123", Description = ex.Message };

            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(response);
        }

        catch (BadRequestException ex)
        {
            if (ex.Errors?.Count > 0)
            {
                var response = new ErrorResponse { Message = "Заполнены не все обязательные поля", Errors = ex.Errors, Description = ex.ToString() };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
            }
            else
            {

                var response = new ErrorResponse { Message = ex.Message, Description = ex.ToString() };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        catch (NotFoundException ex)
        {
            var response = new ErrorResponse { Message = ex.Message, Description = ex.ToString() };
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(response);
        }

        catch (Exception ex)
        {
            // Handle other unhandled exceptions
            // Log the exception for debugging purposes.
            Console.WriteLine($"Unhandled Exception: {ex}");

            // Customize the error response as needed.
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = "Произошла серверная ошибка. Повторите позднее или свяжитесь с администратором",
                Description = ex.ToString()
            });
        }
    }
}

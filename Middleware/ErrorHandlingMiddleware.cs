using LittleLemon_API.Exceptions;
using Microsoft.Data.SqlClient;

namespace LittleLemon_API.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    
    public ErrorHandlingMiddleware()
    {
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(badRequestException.Message);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(notFoundException.Message);
        }
        catch (ArgumentOutOfRangeException argumentOutOfRangeException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(argumentOutOfRangeException.Message);
        }
        catch (IndexOutOfRangeException indexOutOfRangeException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(indexOutOfRangeException.Message);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(invalidOperationException.Message);
        }
        catch (SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 547: // Ograniczenie klucza obcego
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync("Operacja naruszyła ograniczenia integralności bazy danych.");
                    break;
                case 2601: // Naruszenie unikalności
                case 2627:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync("Jedna z wartości narusza unikalność danych.");
                    break;
                case -2: // Przekroczenie czasu oczekiwania
                    context.Response.StatusCode = 408;
                    await context.Response.WriteAsJsonAsync("Przekroczenie czasu oczekiwania na odpowiedź z bazy danych.");
                    break;
                default: 
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync("Wystąpił błąd wewnętrzny bazy danych.");
                    break;
            }
        }
        catch (SystemException)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync("Wystąpił nieoczekiwany błąd systemowy.");
        }
    }
}

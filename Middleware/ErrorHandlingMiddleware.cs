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
                case 547: // Foreign key constraint
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync("Operation violated database integrity constraints.");
                    break;
                case 2601: // Unique index violation
                case 2627:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync("One of the values violates data uniqueness.");
                    break;
                case -2: // Timeout
                    context.Response.StatusCode = 408;
                    await context.Response.WriteAsJsonAsync("Database response timeout exceeded.");
                    break;
                default:
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync("An internal database error occurred.");
                    break;
            }
        }
        catch (SystemException)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync("An unexpected system error occurred.");
        }
    }
}

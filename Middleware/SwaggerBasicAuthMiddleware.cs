using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace LittleLemon_API.Middleware
{
    public class SwaggerBasicAuthMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;

        public SwaggerBasicAuthMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    var header = AuthenticationHeaderValue.Parse(authHeader);
                    var inBytes = Convert.FromBase64String(header.Parameter);
                    var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];

                    var validUsername = _configuration["SwaggerAuth:Username"];
                    var validPassword = _configuration["SwaggerAuth:Password"];
                    if (username.Equals(validUsername) && password.Equals(validPassword))
                    {
                        await next(context);
                        return;
                    }
                }

                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await next(context);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StudentApi.Web.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        const string API_KEY = "ABC123";
        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            bool success = httpContext.Request.Headers.TryGetValue
           (API_KEY, out var apiKeyFromHttpHeader);
            if (!success)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("The Api Key for accessing this endpoint is not available");
                return;
            }
            string api_key_From_Configuration = _configuration[API_KEY];
            if (!api_key_From_Configuration.Equals(apiKeyFromHttpHeader))
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("The authenticationkey is incorrect : Unauthorized access");
                return;
            }
            await _next(httpContext);
        }
    }
}

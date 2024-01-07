using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Web.Authorize
{
    public class AuthorizeApiAttribute: Attribute, IAsyncActionFilter
{
    private const string API_KEY = "ABC123";
    public async Task OnActionExecutionAsync
           (ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool success = context.HttpContext.Request.Headers.TryGetValue
            (API_KEY, out var apiKeyFromHttpHeader);
        if (!success)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "The Api Key for accessing this endpoint is not available"
            };
            return;
        }
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("AppSettings.json");
        IConfiguration Configuration = configurationBuilder.Build();
        string api_key_From_Configuration = Configuration[API_KEY];
        if (!api_key_From_Configuration.Equals(apiKeyFromHttpHeader))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "The Api key is incorrect : Unauthorized access"
            };
            return;
        }
        await next();
    }
}

}

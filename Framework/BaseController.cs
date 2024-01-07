using Microsoft.AspNetCore.Mvc;
using StudentApi.Shared.Response;

namespace StudentApi.Web.Framework
{
        public class BaseController : Controller
        {
            protected new IActionResult Response(CommonResponse response)
            {
                var returnResult = response.IsValid ? response.Data : new { Error = response.Exception.Message };
                return new ObjectResult(returnResult) { StatusCode = response.HttpCode };
            }
        }
}

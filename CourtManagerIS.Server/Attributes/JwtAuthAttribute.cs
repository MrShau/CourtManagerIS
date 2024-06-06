using CourtManagerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CourtManagerApi.Attributes
{
    public class JwtAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HttpContext.Response.Headers["WWW-Authenticate"] = "Bearer";
                context.Result = new JsonResult("Unauthorized");
            }
            else
            {
                var authService = context.HttpContext.RequestServices.GetRequiredService<JwtService>();
                var user = authService.Verify(token);
                if (user == null)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.HttpContext.Response.Headers["WWW-Authenticate"] = "Bearer";
                    context.Result = new JsonResult("Unauthorized");
                }
            }
            base.OnActionExecuting(context);
        }
    }
}

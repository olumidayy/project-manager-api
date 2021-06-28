using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManager.ApplicationCore.Entities;
using ProjectManager.Common;
using System;
using System.Linq;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Items["Token"];
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            var message = token == null ? "No" : "Invalid";
            context.Result = new JsonResult(
                new CustomResponse(status: "error", message: String.Format("{0} token provided.", message)))
            { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}

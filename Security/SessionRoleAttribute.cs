
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SessionRoleAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _requiredRole;

    public SessionRoleAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var session = context.HttpContext.Session;
        var userId = session.GetString("UserId");
        var userRole = session.GetString("UserRole");

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedObjectResult(
                "Please log in first.");
            return;
        }

        if (!string.Equals(
            userRole, _requiredRole,
            StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}
s
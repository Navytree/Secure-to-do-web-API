using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ToDoUserWebAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static int GetUserIdFromToken(this HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            { return 0; }
            return userId;
        }


    }
}

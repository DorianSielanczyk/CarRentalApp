using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.WebUI.Server.Auth
{
    public static class AuthEndpointExtensions
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/auth/logout", async (HttpContext httpContext) =>
            {
                await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                httpContext.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
                httpContext.Response.Headers.Pragma = "no-cache";
                httpContext.Response.Headers.Expires = "0";

                return Results.Redirect("/");
            });

            return endpoints;
        }
    }
}
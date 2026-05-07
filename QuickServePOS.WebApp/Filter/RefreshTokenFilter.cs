using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuickServePOS.WebApp.Services;

namespace QuickServePOS.WebApp.Filter
{
    public class RefreshTokenFilter : IAsyncActionFilter
    {
        private readonly ITokenWebService _tokenWebService;

        public RefreshTokenFilter(
            ITokenWebService tokenWebService)
        {
            _tokenWebService = tokenWebService;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            // ============================================
            // CHECK USER AUTHENTICATED
            // ============================================

            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                var accessToken =
                    httpContext.Request.Cookies["AccessToken"];

                var refreshToken =
                    httpContext.Request.Cookies["RefreshToken"];

                // ========================================
                // BOTH TOKENS MISSING
                // ========================================

                if (string.IsNullOrEmpty(accessToken)
                    && string.IsNullOrEmpty(refreshToken))
                {
                    // REMOVE COOKIE AUTH
                    await httpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    // CLEAR COOKIES
                    httpContext.Response.Cookies.Delete(
                        "AccessToken");

                    httpContext.Response.Cookies.Delete(
                        "RefreshToken");

                    context.Result =
                        new RedirectToActionResult(
                            "Login",
                            "Authentication",
                            null);

                    return;
                }

                // ========================================
                // VALIDATE TOKEN / REFRESH TOKEN FLOW
                // ========================================

                var token =
                    await _tokenWebService
                    .GetValidAccessTokenAsync();

                // ========================================
                // REFRESH FAILED
                // ========================================

                if (string.IsNullOrEmpty(token))
                {
                    await httpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    httpContext.Response.Cookies.Delete(
                        "AccessToken");

                    httpContext.Response.Cookies.Delete(
                        "RefreshToken");

                    context.Result =
                        new RedirectToActionResult(
                            "Login",
                            "Authentication",
                            null);

                    return;
                }
            }

            await next();
        }
    }
}
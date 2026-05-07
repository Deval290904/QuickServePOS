using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using QuickServePOS.Models.DTO.Auth;
using QuickServePOS.Models.ViewModel;
using System.Security.Claims;

namespace QuickServePOS.WebApp.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("AuthenticationAPI/Login", viewmodel);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Invalid Email or Password";
                return View(viewmodel);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result == null)
            {
                TempData["Error"] ="Login failed.";

                return View(viewmodel);
            }

            // STORE ACCESS TOKEN COOKIE

            Response.Cookies.Append("AccessToken",result.AccessToken!,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.Strict,
                   Expires = viewmodel.RememberMe
                       ? DateTime.UtcNow.AddDays(7)
                       : DateTime.UtcNow.AddHours(1)
               });

            //STORE REFRESH TOKEN COOKIE

            Response.Cookies.Append(
               "RefreshToken",
               result.RefreshToken!,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.Strict,
                   Expires = viewmodel.RememberMe
                       ? DateTime.UtcNow.AddDays(7)
                       : DateTime.UtcNow.AddHours(1)
               });


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, result.Email),
                new Claim(ClaimTypes.Name, result.Email),
                new Claim(ClaimTypes.Role, result.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // COOKIE LOGIN
            await HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme,principal,
                new AuthenticationProperties
                {
                    IsPersistent = viewmodel.RememberMe,
                    ExpiresUtc = viewmodel.RememberMe
                        ? DateTime.UtcNow.AddDays(7)
                        : DateTime.UtcNow.AddHours(1)
                });
            TempData["Success"] = "Login Successful!";
            return RedirectToAction("Index", "DashBoard");

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form.";
                return View(viewmodel);
            }
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("AuthenticationAPI/Register", viewmodel);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Registration Failed. Please try again.";
                return View(viewmodel);
            }
            TempData["Success"] = "Registration Successful! Please log in.";
            return RedirectToAction("Login", "Authentication");

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Remove Authentication Cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Remove JWT Cookies
            Response.Cookies.Delete("AccessToken");

            Response.Cookies.Delete("RefreshToken");

            TempData["Success"] =
                "Logout Successful.";

            return RedirectToAction(
                "Login",
                "Authentication");
        }
    }
}

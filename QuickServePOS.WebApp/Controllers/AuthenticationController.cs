using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using QuickServePOS.Models.DTO.Auth;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.ViewModel;
using QuickServePOS.WebApp.HttpHelper;
using System.Security.Claims;

namespace QuickServePOS.WebApp.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IApiHelper _apiHelper;

        public AuthenticationController(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "DashBoard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            var result = await _apiHelper.PostDataAsync<LoginViewModel, LoginResponse>("AuthenticationAPI/Login", viewmodel);

            if (result == null)
            {
                TempData["Error"] = "Login failed.";

                return View(viewmodel);
            }

            if (string.IsNullOrEmpty(result.AccessToken))
            { 
             
                TempData["Error"] = result?.Message?? "Login failed.";
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

            var result =await _apiHelper.PostDataAsync<RegisterViewModel,ApiResponse>("AuthenticationAPI/Register",viewmodel);

            if (result == null)
            {
                TempData["Error"] = "Registration failed.";

                return View(viewmodel);
            }

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return View(viewmodel);
            }

            TempData["Success"] = result?.Message ?? "Registration successful. Please confirm your email.";
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

            TempData["Success"] ="Logout Successful.";

            return RedirectToAction("Login","Authentication");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            var result =await _apiHelper.PostDataAsync<ForgotPasswordViewModel,ApiResponse>("AuthenticationAPI/forgot-password",viewmodel);

            if(result==null)
            {
                TempData["Error"] = "ForgotPassword failed.";
                return View(viewmodel);
            }
            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return View(viewmodel);
            }

            TempData["Success"] = result?.Message ?? "Reset link sent.";

            return RedirectToAction("Login","Authentication");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            var result =await _apiHelper.PostDataAsync<ResetPasswordViewModel,ApiResponse>("AuthenticationAPI/reset-password",viewmodel);

            if (result == null)
            {
                TempData["Error"] = "Password reset failed.";
                return View(viewmodel);
            }
            if (!result.Success)
            {
                TempData["Error"] = result?.Message;

                return View(viewmodel);
            }

            TempData["Success"] =result?.Message
                ?? "Password reset successful.";

            return RedirectToAction(
                "Login",
                "Authentication");
        }
    }
}

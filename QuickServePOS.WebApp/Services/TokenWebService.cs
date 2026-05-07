
using Newtonsoft.Json;
using QuickServePOS.Models.DTO.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace QuickServePOS.WebApp.Services
{
    public class TokenWebService : ITokenWebService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly HttpClient _httpClient;

        public TokenWebService(IHttpContextAccessor httpContextAccessor,IHttpClientFactory httpclientFactory)
        {
            _httpContextAccessor = httpContextAccessor;

            _httpClient = httpclientFactory.CreateClient("ApiClient");
        }

        public async Task<string?> GetValidAccessTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            var accessToken =
                context?.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(accessToken)
                    && !IsTokenExpired(accessToken))
            {
                return accessToken;
            }

            var refreshToken =
                context?.Request.Cookies["RefreshToken"];


            // ============================================
            // REFRESH TOKEN API CALL
            // ============================================

            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            var request = new
            {
                RefreshToken = refreshToken
            };  

            var content = new StringContent(JsonConvert.SerializeObject(request),Encoding.UTF8,"application/json");

            var response =await _httpClient.PostAsync("AuthenticationAPI/Refresh-token",content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result =
                JsonConvert.DeserializeObject<LoginResponse>(
                    await response.Content.ReadAsStringAsync());

            // ============================================
            // SAVE NEW TOKENS
            // ============================================

            context!.Response.Cookies.Append(
                "AccessToken",
                result!.AccessToken!,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });

            context.Response.Cookies.Append(
                "RefreshToken",
                result.RefreshToken!,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

            return result.AccessToken;
        }

        private bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.ValidTo < DateTime.UtcNow;
        }
    }
}
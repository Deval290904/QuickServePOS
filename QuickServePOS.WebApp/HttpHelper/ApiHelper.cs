using NuGet.Common;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.WebApp.Services;
using System.Net.Http.Headers;

namespace QuickServePOS.WebApp.HttpHelper
{
    public class ApiHelper : IApiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenWebService _tokenWebService;

        public ApiHelper(IHttpClientFactory httpClientFactory,ITokenWebService tokenWebService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _tokenWebService = tokenWebService;
        }

        private async Task<bool> AddTokenAsync()
        {
            var token = await _tokenWebService.GetValidAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            _httpClient.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue( "Bearer",token);

            return true;
        }

        // 🔹 GET
        public async Task<T?> GetAsync<T>(string url)
        {
            await AddTokenAsync();

            var response =
                await _httpClient.GetAsync(url);

            // TOKEN EXPIRED DURING REQUEST

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                await AddTokenAsync();

                response =
                    await _httpClient.GetAsync(url);
            }

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content
                .ReadFromJsonAsync<T>();
        }

        // 🔹 POST
        public async Task<ApiResponse> PostAsync<T>(string url, T data)
        {
            await AddTokenAsync();
            var response = await _httpClient.PostAsJsonAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                return new ApiResponse { Success = false, Message = error };
            }

            return new ApiResponse { Success = true, Message = "Success" };
        }

        public async Task<TResponse?> PostDataAsync<TRequest, TResponse>(string url, TRequest data)
        {
            await AddTokenAsync();

            var response =
                await _httpClient.PostAsJsonAsync(
                    url,
                    data);

            return await response.Content
                .ReadFromJsonAsync<TResponse>();
        }

        public async Task<ApiResponse> DeleteAsync(string url)
        {
            await AddTokenAsync();
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse { Success = false, Message = error };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return new ApiResponse { Success = result?.Success ?? false, Message = result?.Message ?? "Deleted" };
        }

        public async Task<ApiResponse> PutAsync<T>(string url, T data)
        {
            await AddTokenAsync();
            var response = await _httpClient.PutAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse { Success = false, Message = error };
            }
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return new ApiResponse { Success = result?.Success ?? false, Message = result?.Message ?? "Updated" };
        }

        public async Task<ApiResponse> PutAsync(string url)
        {
            await AddTokenAsync();
            var response = await _httpClient.PutAsync(url, null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse { Success = false, Message = error };
            }
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return new ApiResponse { Success = result?.Success ?? false, Message = result?.Message ?? "Updated" };
        }

        public async Task<TResponse?> PutDataAsync<TRequest, TResponse>(string url,TRequest data)
        {
            await AddTokenAsync();
            var response = await _httpClient.PutAsJsonAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content
                .ReadFromJsonAsync<TResponse>();
        }
    }
}

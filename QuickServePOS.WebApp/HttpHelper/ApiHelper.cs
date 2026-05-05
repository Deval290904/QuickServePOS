using QuickServePOS.Models.DTO.Common;

namespace QuickServePOS.WebApp.HttpHelper
{
    public class ApiHelper : IApiHelper
    {
        private readonly HttpClient _httpClient;

        public ApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // 🔹 GET
        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        // 🔹 POST
        public async Task<ApiResponse> PostAsync<T>(string url, T data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                return new ApiResponse { Success = false, Message = error };
            }

            return new ApiResponse { Success = true, Message = "Success" };
        }

        public async Task<ApiResponse> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse { Success = false, Message = error };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return new ApiResponse { Success = result?.Success ?? false, Message = result?.Message ?? "Deleted" };
        }

        public async Task<ApiResponse> PutAsync(string url)
        {
            var response = await _httpClient.PutAsync(url, null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse { Success = false, Message = error };
            }
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return new ApiResponse { Success = result?.Success ?? false, Message = result?.Message ?? "Updated" };
        }
    }
}

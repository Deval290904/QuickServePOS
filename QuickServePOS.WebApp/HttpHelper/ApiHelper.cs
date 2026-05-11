using Newtonsoft.Json;
using NuGet.Common;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.WebApp.Services;
using System.Net.Http.Headers;
using System.Web.Helpers;

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

            var json =await response.Content.ReadAsStringAsync();

            var result =JsonConvert.DeserializeObject<ApiResponse>(json);

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

        public async Task<TResponse?> PostFormDataAsync<TRequest, TResponse>(string url,TRequest model)
        {
            try
            {
                await AddTokenAsync();

                using var content =
                    new MultipartFormDataContent();

                var properties =
                    typeof(TRequest).GetProperties();

                foreach (var property in properties)
                {
                    var value = property.GetValue(model);

                    if (value == null)
                        continue;

                    // Handle file upload
                    if (value is IFormFile file)
                    {
                        var streamContent =
                            new StreamContent(
                                file.OpenReadStream());

                        streamContent.Headers.ContentType =
                            new MediaTypeHeaderValue(
                                file.ContentType);

                        content.Add(
                            streamContent,
                            property.Name,
                            file.FileName);
                    }
                    else
                    {
                        content.Add(
                            new StringContent(value.ToString()),
                            property.Name);
                    }
                }

                var response =await _httpClient.PostAsync(url,content);

                var json =await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponse>(json);
            }
            catch
            {
                return default;
            }
        }

        public async Task<TResponse?> PutFormDataAsync<TRequest, TResponse>(string url,TRequest model)
        {
            try
            {
                await AddTokenAsync();

                using var content =
                    new MultipartFormDataContent();

                var properties =
                    typeof(TRequest).GetProperties();

                foreach (var property in properties)
                {
                    var value = property.GetValue(model);

                    if (value == null)
                        continue;

                    // FILE
                    if (value is IFormFile file)
                    {
                        var streamContent =
                            new StreamContent(
                                file.OpenReadStream());

                        streamContent.Headers.ContentType =
                            new MediaTypeHeaderValue(
                                file.ContentType);

                        content.Add(
                            streamContent,
                            property.Name,
                            file.FileName);
                    }
                    else
                    {
                        content.Add(
                            new StringContent(value.ToString()!),
                            property.Name);
                    }
                }

                var request =
                    new HttpRequestMessage(
                        HttpMethod.Put,
                        url)
                    {
                        Content = content
                    };

                var response =await _httpClient.SendAsync(request);

                var json =await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponse>(json);
            }
            catch
            {
                return default;
            }
        }
    }
}

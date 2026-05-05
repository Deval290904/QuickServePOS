using QuickServePOS.Models.DTO.Common;

namespace QuickServePOS.WebApp.HttpHelper
{
    public interface IApiHelper
    {
        Task<T?> GetAsync<T>(string url);
        Task<ApiResponse> PostAsync<T>(string url, T data);

        Task<ApiResponse> DeleteAsync(string url);

        Task<ApiResponse> PutAsync(string url);
    }
}

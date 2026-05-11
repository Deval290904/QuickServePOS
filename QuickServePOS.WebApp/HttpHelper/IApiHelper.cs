
using QuickServePOS.Models.DTO.Common;
using System.Threading.Tasks;

namespace QuickServePOS.WebApp.HttpHelper
{
    public interface IApiHelper
    {
        Task<T?> GetAsync<T>(string url);
        Task<ApiResponse> PostAsync<T>(string url, T data);

        Task<TResponse?> PostDataAsync<TRequest, TResponse>(string url,TRequest data);

        Task<TResponse?> PostFormDataAsync<TRequest, TResponse>(string url, TRequest model);

        Task<ApiResponse> DeleteAsync(string url);

        Task<ApiResponse> PutAsync(string url);
        Task<ApiResponse> PutAsync<T>(string url, T data);
        Task<TResponse?> PutDataAsync<TRequest, TResponse>(string url, TRequest data);
        Task<TResponse?> PutFormDataAsync<TRequest, TResponse>(string url, TRequest model);

    }
}

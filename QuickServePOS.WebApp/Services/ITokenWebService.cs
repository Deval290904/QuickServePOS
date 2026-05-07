namespace QuickServePOS.WebApp.Services
{
    public interface ITokenWebService
    {
        Task<string?> GetValidAccessTokenAsync();
    }
}

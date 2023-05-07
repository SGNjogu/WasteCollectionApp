namespace ttnm.Infrastructure.Services.APIService
{
    public interface IRestService
    {
        Task<T> GETRequest<T>(string uri);
        Task<T> SENDRequest<T>(string uri, HttpMethod httpMethod, object? payload = null);
        Task<T> POSTRequest<T>(string uri, object sender);
        Task<T> POSTRequest<T>(string uri);
    }
}
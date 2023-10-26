namespace Atea.Core.Services
{
    public interface IHTTPService
    {
        Task<HttpResponseMessage> GetData();
    }
}

using Atea.Core.Services;

namespace Atea.Services
{
    public class HTTPService : IHTTPService
    {
        private HttpClient _client;

        public HTTPService(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetData(string url)
        {
            var data = await _client.GetAsync(url);

            return data;
        }
    }
}

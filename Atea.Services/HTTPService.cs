using Atea.Core.Services;

namespace Atea.Services
{
    public class HTTPService : IHTTPService
    {
        private HttpClient _client;

        public HTTPService(HttpClient client)
        {
            _client = client;

            _client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("APIEndpoint"));
        }

        public async Task<HttpResponseMessage> GetData()
        {
            var data = await _client.GetAsync("/random?auth=null");

            return data;
        }
    }
}

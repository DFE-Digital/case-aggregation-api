using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public abstract class ApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ApiClient> _logger;
        private readonly string _httpClientName;

        protected ApiClient(
            IHttpClientFactory clientFactory, 
            ILogger<ApiClient> logger,
            string httpClientName)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _httpClientName = httpClientName;
        }

        public async Task<T> Get<T>(string endpoint, IDictionary<string, string>? headers = null) where T : class
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

                // Add custom headers if provided
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                var client = CreateHttpClient();

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                // Read response content
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize content
                var result = JsonConvert.DeserializeObject<T>(content);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        
        private HttpClient CreateHttpClient()
        {
            var client = _clientFactory.CreateClient(_httpClientName);
            
            //TODO: we still using this class?
            // client.DefaultRequestHeaders.Add(HttpHeaderConstants.UserContextName, _httpContextAccessor.HttpContext.User?.Identity?.Name);

            return client;
        }
    }
}

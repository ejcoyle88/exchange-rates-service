using ExchangeRateService.Common.Requests;
using ExchangeRateService.Common.Responses;
using RestSharp;

namespace ExchangeRateService.Client
{
    public class ExchangeRateServiceClient : IExchangeRateServiceClient
    {
        private readonly ExchangeRateServiceClientConfiguration _configuration;

        private RestClient _restClient;

        private RestClient RestClient
        {
            get { return _restClient ?? (_restClient = new RestClient(_configuration.ApiLocation)); }
            set { _restClient = value; }
        }

        internal ExchangeRateServiceClient(ExchangeRateServiceClientConfiguration config)
        {
            _configuration = config;
        }

        public ExchangeRatesResponse GetExchangeRatesFor(ExchangeRatesRequest request)
        {
            var req = CreateRestRequest("/", Method.POST);
            req.AddBody(request);
            return RestClient.Execute<ExchangeRatesResponse>(req).Data;
        }

        private static RestRequest CreateRestRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");
            request.RequestFormat = DataFormat.Json;

            return request;
        }
    }
}
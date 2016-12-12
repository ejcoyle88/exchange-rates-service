using ExchangeRateService.Common.Requests;
using ExchangeRateService.Common.Responses;

namespace ExchangeRateService.Client
{
    public interface IExchangeRateServiceClient
    {
        ExchangeRatesResponse GetExchangeRatesFor(ExchangeRatesRequest request);
    }
}
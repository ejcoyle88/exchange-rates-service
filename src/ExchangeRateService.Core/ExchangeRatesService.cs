using System.Diagnostics.Contracts;
using System.Linq;
using ExchangeRateService.Common.Requests;
using ExchangeRateService.Common.Responses;
using ExchangeRateService.Core.Caching;
using ExchangeRateService.Core.DataFetching;

namespace ExchangeRateService.Core
{
	public interface IExchangeRatesService
	{
		ExchangeRatesResponse GetExchangeRatesFor(ExchangeRatesRequest request);
	}

	public class ExchangeRatesService : IExchangeRatesService
	{
		private readonly ExchangeRatesCache _cache = new ExchangeRatesCache();
		private readonly IExchangeRatesFetchingService _fetch = new OpenExchangeRatesFetchingService();

		public ExchangeRatesResponse GetExchangeRatesFor(ExchangeRatesRequest request)
		{
			Contract.Ensures(Contract.Result<ExchangeRatesResponse>() != null);
			// Check if there is a valid cached response to this request.
			var cached = _cache.Get(request);
			if (cached != null)
			{
				// If yes, return that result
				return new ExchangeRatesResponse(cached);
			}
			// If no, fetch the most recent data from the data fetching api.
			var latest = _fetch.GetLatest();
			// If there was data, store this data in the historical storage & cache.
			foreach (var rate in latest)
			{
				_cache.Add(rate);
			}
			// If we have the rate we are looking for, return it.
			var requestedRate = latest.SingleOrDefault(x => x.BaseCurrencyCode == request.BaseCurrencyCode &&
													   x.CurrencyCode == request.CurrencyCode);
			return new ExchangeRatesResponse(requestedRate);
		}
	}
}

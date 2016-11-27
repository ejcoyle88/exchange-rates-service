using System.Diagnostics.Contracts;
using System.Linq;
using ExchangeRateService.Common.Requests;
using ExchangeRateService.Common.Responses;
using ExchangeRateService.Core.Caching;
using ExchangeRateService.Core.DataFetching;
using System;

namespace ExchangeRateService.Core
{
	public interface IExchangeRatesService
	{
		ExchangeRatesResponse GetExchangeRatesFor(ExchangeRatesRequest request);
	}

	public class ExchangeRatesService : IExchangeRatesService
	{
		private readonly ExchangeRatesCache _cache = new ExchangeRatesCache();
		private readonly ExchangeRatesFetchingService _fetch = new ExchangeRatesFetchingService();
		private readonly ConfigurationContainer _config = new ConfigurationContainer();

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

			// If no, check when we last pulled from the API.
			var lastFetch = _cache.GetLastFetch();
			var cacheExpiryTime = DateTime.UtcNow.AddMinutes(_config.CacheExpiryTime);

			// If it was more recently than out expiry time, gtfo.
			if (lastFetch.HasValue && lastFetch.Value > cacheExpiryTime)
			{
				// It isn't in the cache, and we've made a request recently, so no dice.
				return new ExchangeRatesResponse(null);
			}

			// Go get 'em.
			var latest = _fetch.GetLatest();

			// If there was data, store this data in the historical storage & cache.
			foreach (var rate in latest)
			{
				_cache.Add(rate);
			}

			// Save when we last did a fetch, so we don't spam an API somewhere.
			_cache.AddLastFetch(DateTime.UtcNow);

			// If we have the rate we are looking for, return it. If not, we end up with an empty result.
			var requestedRate = latest.SingleOrDefault(x => x.BaseCurrencyCode == request.BaseCurrencyCode &&
													   x.CurrencyCode == request.CurrencyCode);
			return new ExchangeRatesResponse(requestedRate);
		}
	}
}

using System.Diagnostics.Contracts;
using System.Linq;
using ExchangeRateService.Common.Requests;
using ExchangeRateService.Common.Responses;
using ExchangeRateService.Core.Caching;
using ExchangeRateService.Core.DataFetching;
using System;
using ExchangeRateService.Common.Models;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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
			else {
				//TODO: We're making the assumption here that our API fetches USD as the default base rate.
				// It's kinda bad, but I can't think of a better solution right now.
				var baseRate = _cache.Get(new ExchangeRatesRequest
				{
					BaseCurrencyCode = "USD",
					CurrencyCode = request.BaseCurrencyCode
				});

				var currRate = _cache.Get(new ExchangeRatesRequest
				{
					BaseCurrencyCode = "USD",
					CurrencyCode = request.CurrencyCode
				});

				if (baseRate != null && currRate != null)
				{
					var rate = currRate.ConversionRate / baseRate.ConversionRate;
					var exRate = new ExchangeRate(request.CurrencyCode, rate, request.BaseCurrencyCode, DateTime.UtcNow);
					_cache.Add(exRate);
					return new ExchangeRatesResponse(exRate);
				}
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
				// Cache this rate.
				_cache.Add(rate);
			}

			// Save when we last did a fetch, so we don't spam an API somewhere.
			_cache.AddLastFetch(DateTime.UtcNow);

			// If we have the rate we are looking for, return it. If not, we end up with an empty result.
			var requestedRate = latest.SingleOrDefault(x => x.BaseCurrencyCode == request.BaseCurrencyCode &&
													   x.CurrencyCode == request.CurrencyCode);

			// If it doesn't exist, but we can figure it out, figure it out and cache that too.
			if (requestedRate == null &&
			   latest.Any(x => x.CurrencyCode == request.BaseCurrencyCode) &&
			   latest.Any(x => x.CurrencyCode == request.CurrencyCode))
			{
				var baseRate = latest.Single(x => x.CurrencyCode == request.BaseCurrencyCode);
				var currRate = latest.Single(x => x.CurrencyCode == request.CurrencyCode);

				var rate = currRate.ConversionRate / baseRate.ConversionRate;
				var exRate = new ExchangeRate(request.CurrencyCode, rate, request.BaseCurrencyCode, DateTime.UtcNow);
				_cache.Add(exRate);

				requestedRate = exRate;
			}

			return new ExchangeRatesResponse(requestedRate);
		}
	}
}

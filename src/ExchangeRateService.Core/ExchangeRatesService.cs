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

			// Make sure we get all of the possible rates we can get out of this info.
			var allRates = GetAllPossibleExchangeRates(latest);

			// If there was data, store this data in the historical storage & cache.
			foreach (var rate in allRates)
			{
				// Cache this rate.
				_cache.Add(rate);
			}

			// Save when we last did a fetch, so we don't spam an API somewhere.
			_cache.AddLastFetch(DateTime.UtcNow);

			// If we have the rate we are looking for, return it. If not, we end up with an empty result.
			var requestedRate = allRates.SingleOrDefault(x => x.BaseCurrencyCode == request.BaseCurrencyCode &&
													   x.CurrencyCode == request.CurrencyCode);
			return new ExchangeRatesResponse(requestedRate);
		}

		//TODO: I need to find some way to make this significantly faster, currently this loop is doing (I think)
		//  180 to the power of 2 conversions, minus USD ones because the API does those for us.
		//  I've already started figuring out the inverse of each to approx. half the number of needed iterations,
		//  but its still a lot and takes forever (over 30s).
		private IEnumerable<ExchangeRate> GetAllPossibleExchangeRates(IEnumerable<ExchangeRate> apiRates)
		{
			var allExchangeRates = new List<ExchangeRate>(apiRates);

			// Get a list of all of the currency codes.
			var currencyCodes = apiRates.Select(x => x.CurrencyCode).Distinct().ToList();

			foreach (var baseRate in currencyCodes)
			{
				// Get the rate of the api call BaseCurrencyCode to this Base.
				var baseToBase = apiRates.SingleOrDefault(x => x.CurrencyCode == baseRate);

				foreach (var conversion in currencyCodes)
				{
					// If we're already in the list, gtfo.
					if (allExchangeRates.Any(x => x.BaseCurrencyCode == baseRate && x.CurrencyCode == conversion))
					{
						continue;
					}

					decimal conversionRate = 0;
					// If baseToBase is null, we're doing BaseCurrencyCode to BaseCurrencyCode, which == 1.
					if (baseToBase == null)
					{
						conversionRate = 1;
					}
					else
					{
						// Get the rate of the api call BaseCurrencyCode to this Currency.
						var conversionExistingRate = apiRates.SingleOrDefault(x => x.CurrencyCode == conversion);
						if (conversionExistingRate != null)
						{
							// Use both rates to find the rate between these two currencies.
							conversionRate = conversionExistingRate.ConversionRate / baseToBase.ConversionRate;
						}

					}

					// Stick this rate in the list.
					allExchangeRates.Add(new ExchangeRate(conversion, conversionRate, baseRate, DateTime.UtcNow));
					// Invert for speed.
					if (!allExchangeRates.Any(x => x.BaseCurrencyCode == conversion && x.CurrencyCode == baseRate))
					{
						allExchangeRates.Add(new ExchangeRate(baseRate, 1 / conversionRate, conversion, DateTime.UtcNow));
					}
				}
			}

			return allExchangeRates;
		}
	}
}

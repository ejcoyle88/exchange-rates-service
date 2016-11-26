using System;
using System.Runtime.Caching;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Common.Requests;

namespace ExchangeRateService.Core.Caching
{
	internal class ExchangeRatesCache
	{
		// Singleton, because MS said so... :(
		// https://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache.memorycache(v=vs.110).aspx
		private static MemoryCache _memoryCache = null;
		private static MemoryCache MemCache
		{
			get
			{
				return _memoryCache ?? (_memoryCache = MemoryCache.Default);
			}
		}

		private readonly ConfigurationContainer _configuration = new ConfigurationContainer();

		private static string GetCacheKey(ExchangeRate exchangeRate)
		{
			return GetCacheKey(exchangeRate.BaseCurrencyCode, exchangeRate.CurrencyCode);
		}

		private static string GetCacheKey(ExchangeRatesRequest request)
		{
			return GetCacheKey(request.BaseCurrencyCode, request.CurrencyCode);
		}

		private static string GetCacheKey(string baseCurrencyCode, string currencyCode)
		{
			return string.Format("{0}_{1}", baseCurrencyCode, currencyCode);
		}

		private CacheItemPolicy GetCacheItemPolicy()
		{
			var minutes = _configuration.CacheExpiryTime;
			var span = TimeSpan.FromMinutes(minutes);
			var offset = new DateTimeOffset(DateTime.UtcNow, span);
			return new CacheItemPolicy
			{
				AbsoluteExpiration = offset
			};
		}

		public ExchangeRate Get(ExchangeRatesRequest request)
		{
			var cacheKey = GetCacheKey(request);
			return MemCache.Get(cacheKey) as ExchangeRate;
		}

		public void Add(ExchangeRate exchangeRate)
		{
			var cacheKey = GetCacheKey(exchangeRate);
			var cacheItemPolicy = GetCacheItemPolicy();
			var cacheItem = new CacheItem(cacheKey, exchangeRate);
			MemCache.Add(cacheItem, cacheItemPolicy);
		}
	}
}

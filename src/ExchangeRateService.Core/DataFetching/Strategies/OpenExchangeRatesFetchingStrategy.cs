using System;
using System.Linq;
using System.Collections.Generic;
using ExchangeRateService.Common.Models;
using OpenExchangeRates;
using ExchangeRateService.Core.DataFetching.Strategies;

namespace ExchangeRateService.Core.DataFetching
{
	internal class OpenExchangeRatesFetchingStrategy : IDataFetchingStrategy
	{
		public IEnumerable<ExchangeRate> Fetch()
		{
			var exchangeRateData = Client.Get();
			return exchangeRateData.Rates.Select(r => new ExchangeRate(r.Key, r.Value, exchangeRateData.Base, DateTime.UtcNow));
		}
	}
}

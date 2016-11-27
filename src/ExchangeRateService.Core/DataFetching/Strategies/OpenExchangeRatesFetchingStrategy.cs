using System;
using System.Linq;
using System.Collections.Generic;
using ExchangeRateService.Common.Models;
using OpenExchangeRates;

namespace ExchangeRateService.Core.DataFetching.Strategies
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

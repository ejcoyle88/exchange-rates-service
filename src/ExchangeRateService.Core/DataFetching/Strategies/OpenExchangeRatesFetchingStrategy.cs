using System;
using System.Linq;
using System.Collections.Generic;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Common.Requests;
using OpenExchangeRates;

namespace ExchangeRateService.Core.DataFetching.Strategies
{
	internal class OpenExchangeRatesFetchingStrategy : IDataFetchingStrategy
	{
		public static string Reference
		{
			get
			{
				return "OpenExchangeRates";
			}
		}

        public IEnumerable<ExchangeRate> Fetch(ExchangeRatesRequest request)
        {
            ConfigurationContainer.EnsureAppSettingExists("OpenExchangeRates.ApiKey");

			var exchangeRateData = Client.Get();
			return exchangeRateData.Rates.Select(r => new ExchangeRate(r.Key, r.Value, exchangeRateData.Base, DateTime.UtcNow));
		}
	}
}

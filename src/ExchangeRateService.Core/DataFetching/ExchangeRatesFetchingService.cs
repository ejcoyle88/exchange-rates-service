using System;
using System.Collections.Generic;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Core.DataFetching.Strategies;
using System.Linq;
using ExchangeRateService.Common.Requests;
using ExchangeRateService.Core.Exceptions;

namespace ExchangeRateService.Core.DataFetching
{
	public class ExchangeRatesFetchingService
	{
		private readonly ConfigurationContainer _configuration = new ConfigurationContainer();
		private readonly Dictionary<string, IDataFetchingStrategy> _strategies = new Dictionary<string, IDataFetchingStrategy>
		{
			{ OpenExchangeRatesFetchingStrategy.Reference, new OpenExchangeRatesFetchingStrategy() },
            { GoogleFetchingStrategy.Reference, new GoogleFetchingStrategy() }
		};

		public IEnumerable<ExchangeRate> GetLatest(ExchangeRatesRequest request)
		{
			var dataFetchingStrategy = _configuration.DataFetchingStrategy;
			if (_strategies.Keys.All(x => !string.Equals(x, dataFetchingStrategy, StringComparison.InvariantCultureIgnoreCase)))
			{
				const string errMsgFormat = "Data fetching strategy not found: {0}.";
				var errMsg = string.Format(errMsgFormat, dataFetchingStrategy);
				throw new ConfigurationMissingException(errMsg);
			}
            return _strategies[dataFetchingStrategy].Fetch(request);
		}
	}
}

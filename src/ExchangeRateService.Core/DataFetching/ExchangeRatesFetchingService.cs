using System.Collections.Generic;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Core.DataFetching.Strategies;
using System.Linq;
using ExchangeRateService.Core.Exceptions;

namespace ExchangeRateService.Core.DataFetching
{
	public class ExchangeRatesFetchingService
	{
		private readonly ConfigurationContainer _configuration = new ConfigurationContainer();
		private readonly Dictionary<string, IDataFetchingStrategy> _strategies = new Dictionary<string, IDataFetchingStrategy>
		{
			{ "OpenExchangeRates", new OpenExchangeRatesFetchingStrategy() }
		};

		public IEnumerable<ExchangeRate> GetLatest()
		{
			var dataFetchingStrategy = _configuration.DataFetchingStrategy;
			if (!_strategies.Keys.Any(x => x == dataFetchingStrategy))
			{
				var errMsgFormat = "Data fetching strategy not found: {0}";
				var errMsg = string.Format(errMsgFormat, dataFetchingStrategy);
				throw new ConfigurationMissingException(errMsg);
			}
			return _strategies[dataFetchingStrategy].Fetch();
		}
	}
}

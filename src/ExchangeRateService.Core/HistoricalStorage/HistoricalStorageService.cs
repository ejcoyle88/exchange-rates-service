using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Core.Exceptions;
using ExchangeRateService.Core.HistoricalStorage.Strategies;

namespace ExchangeRateService.Core.HistoricalStorage
{
    internal class HistoricalStorageService
    {
        private readonly ConfigurationContainer _configuration = new ConfigurationContainer();

        private readonly Dictionary<string, IHistoricalStorageStrategy> _strategies = new Dictionary<string, IHistoricalStorageStrategy>
        {
            {"NONE", null},
            {MongoHistoricalStorage.Reference, new MongoHistoricalStorage()}
        };

        public void Store(IEnumerable<ExchangeRate> exchangeRates)
        {
            var historicalStorageStrategy = _configuration.HistoricalStorageStrategy;
            if (_strategies.Keys.All(x => !string.Equals(x, historicalStorageStrategy, StringComparison.InvariantCultureIgnoreCase)))
            {
                const string errMsgFormat = "Historical storage strategy not found: {0}.";
                var errMsg = string.Format(errMsgFormat, historicalStorageStrategy);
                throw new ConfigurationMissingException(errMsg);
            }

            var strategy = _strategies[historicalStorageStrategy];
            if (strategy != null)
            {
                strategy.Store(exchangeRates);
            }
        }
    }
}
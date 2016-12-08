using System.Collections.Generic;
using ExchangeRateService.Common.Models;

namespace ExchangeRateService.Core.HistoricalStorage
{
    internal interface IHistoricalStorageStrategy
    {
        void Store(IEnumerable<ExchangeRate> exchangeRates);
    }
}
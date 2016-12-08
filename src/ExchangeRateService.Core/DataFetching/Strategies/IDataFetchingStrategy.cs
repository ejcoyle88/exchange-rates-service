using System;
using System.Collections.Generic;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Common.Requests;

namespace ExchangeRateService.Core.DataFetching.Strategies
{
	internal interface IDataFetchingStrategy
	{
	    IEnumerable<ExchangeRate> Fetch(ExchangeRatesRequest request);
	}
}

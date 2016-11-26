using System;
using System.Collections.Generic;
using ExchangeRateService.Common.Models;
namespace ExchangeRateService.Core.DataFetching.Strategies
{
	internal interface IDataFetchingStrategy
	{
		IEnumerable<ExchangeRate> Fetch();
	}
}

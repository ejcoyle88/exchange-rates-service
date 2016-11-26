using System;
using ExchangeRateService.Common.Models;

namespace ExchangeRateService.Common.Responses
{
	public class ExchangeRatesResponse
	{
		public ExchangeRate ExchangeRate { get; private set; }

		public ExchangeRatesResponse(ExchangeRate exchangeRate)
		{
			ExchangeRate = exchangeRate;
		}
	}
}

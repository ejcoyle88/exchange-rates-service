using System;
using ExchangeRateService.Common.Models;

namespace ExchangeRateService.Common.Responses
{
	public class ExchangeRatesResponse
	{
		public ExchangeRate ExchangeRate { get; set; }

		public ExchangeRatesResponse(ExchangeRate exchangeRate)
		{
			ExchangeRate = exchangeRate;
		}

        public ExchangeRatesResponse() { }
	}
}

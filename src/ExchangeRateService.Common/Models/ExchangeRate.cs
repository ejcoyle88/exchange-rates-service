using System;
namespace ExchangeRateService.Common.Models
{
	public class ExchangeRate
	{
		public string CurrencyCode { get; private set; }
		public decimal ConversionRate { get; private set; }
		public string BaseCurrencyCode { get; private set; }
		public DateTime Timestamp { get; private set; }

		public ExchangeRate(string currencyCode,
						   decimal conversionRate,
						   string baseCurrency,
						   DateTime timestamp)
		{
			CurrencyCode = currencyCode;
			ConversionRate = conversionRate;
			BaseCurrencyCode = baseCurrency;
			Timestamp = timestamp;
		}
	}
}

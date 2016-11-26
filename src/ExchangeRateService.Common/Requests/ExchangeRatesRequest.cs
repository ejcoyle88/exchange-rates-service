using System;
namespace ExchangeRateService.Common.Requests
{
	/// <summary>
	/// Exchange rates request.
	/// To instantiate this class, please use the <see cref="T:ExchangeRateService.Common.ExchangeRatesRequestBuilder" /> class.
	/// </summary>
	public class ExchangeRatesRequest
	{
		public string BaseCurrencyCode { get; private set; }
		public string CurrencyCode { get; private set; }

		internal ExchangeRatesRequest(string baseCurrencyCode,
									string currencyCode)
		{
			BaseCurrencyCode = baseCurrencyCode;
			CurrencyCode = currencyCode;
		}
	}
}

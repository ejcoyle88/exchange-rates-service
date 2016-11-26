using System;
namespace ExchangeRateService.Common.Requests.Builders
{
	public class ExchangeRatesRequestBuilder
	{
		private string _baseCurrencyCode;
		private string _currencyCode;

		public ExchangeRatesRequestBuilder()
		{
			_baseCurrencyCode = string.Empty;
			_currencyCode = string.Empty;
		}

		public ExchangeRatesRequestBuilder From(string baseCurrencyCode)
		{
			_baseCurrencyCode = baseCurrencyCode;
			return this;
		}

		public ExchangeRatesRequestBuilder To(string currencyCode)
		{
			_currencyCode = currencyCode;
			return this;
		}

		public ExchangeRatesRequest Build()
		{
			if (string.IsNullOrWhiteSpace(_baseCurrencyCode) ||
			   string.IsNullOrWhiteSpace(_currencyCode))
			{
				const string errMsg = "Both a BaseCurrencyCode and a CurrencyCode is required to construct an ExchangeRatesRequest.";
				throw new InvalidOperationException(errMsg);
			}
			return new ExchangeRatesRequest(_baseCurrencyCode, _currencyCode);
		}
	}
}
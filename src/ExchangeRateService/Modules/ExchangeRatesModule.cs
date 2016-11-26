using ExchangeRateService.Core;
using Nancy;
using Nancy.ModelBinding;
using ExchangeRateService.Common.Requests;

namespace ExchangeRateService.Modules
{
	public class ExchangeRatesModule : NancyModule
	{
		private readonly IExchangeRatesService _exchangeRatesService;

		public ExchangeRatesModule()
			: base("/")
		{
			_exchangeRatesService = new ExchangeRatesService();

			Post["/"] = parameters =>
			{
				var request = this.Bind<ExchangeRatesRequest>();
				return _exchangeRatesService.GetExchangeRatesFor(request);
			};
		}
	}
}
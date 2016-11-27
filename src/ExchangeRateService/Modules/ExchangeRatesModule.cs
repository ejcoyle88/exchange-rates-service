using ExchangeRateService.Core;
using Nancy;
using Nancy.ModelBinding;
using ExchangeRateService.Common.Requests;
using Nancy.Validation;

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

				var validationResult = this.Validate(request);
				if (!validationResult.IsValid)
				{
					return Negotiate.WithModel(validationResult).WithStatusCode(HttpStatusCode.BadRequest);
				}

				return _exchangeRatesService.GetExchangeRatesFor(request);
			};
		}
	}
}
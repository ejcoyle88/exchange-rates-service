using System;
using ExchangeRateService.Common.Requests;
using FluentValidation;
using System.Web.UI.WebControls;

namespace ExchangeRateService.Validators
{
	public class ExchangeRatesRequestValidator : AbstractValidator<ExchangeRatesRequest>
	{
		public ExchangeRatesRequestValidator()
		{
			RuleFor(r => r).NotNull().WithMessage("You must specify a BaseCurrencyCode and CurrencyCode in the body of the request.");
			RuleFor(r => r.BaseCurrencyCode).NotEmpty().WithMessage("You must specify a BaseCurrencyCode.");
			RuleFor(r => r.CurrencyCode).NotEmpty().WithMessage("You must specify a CurrencyCode.");
		}
	}
}

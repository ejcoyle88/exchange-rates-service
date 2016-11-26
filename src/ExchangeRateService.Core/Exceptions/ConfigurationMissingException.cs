using System;
namespace ExchangeRateService.Core.Exceptions
{
	[Serializable]
	public class ConfigurationMissingException : Exception
	{
		public ConfigurationMissingException() { }
		public ConfigurationMissingException(string msg) : base(msg) { }
	}
}

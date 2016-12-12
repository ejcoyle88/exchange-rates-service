using System.Configuration;
using System.Linq;

namespace ExchangeRateService.Client
{
    public class ExchangeRateServiceClientFactory
    {
        public static IExchangeRateServiceClient GetClient()
        {
            var configuration = GetConfiguration();
            return new ExchangeRateServiceClient(configuration);
        }

        private static ExchangeRateServiceClientConfiguration GetConfiguration()
        {
            return new ExchangeRateServiceClientConfiguration
            {
                ApiLocation = TryGetAppSetting(ClientConfigurationConstants.ApiLocation)
            };
        }

        private static string TryGetAppSetting(string key)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                throw new ConfigurationErrorsException("Required AppSetting is missing: " + key);
            }
            return ConfigurationManager.AppSettings[key];
        }

        private static class ClientConfigurationConstants
        {
            public const string ApiLocation = "ExchangeRateService_Url";
        }
    }
}
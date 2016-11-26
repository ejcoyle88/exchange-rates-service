using System;
using System.Linq;
using System.Configuration;
using ExchangeRateService.Core.Exceptions;

namespace ExchangeRateService.Core
{
	internal class ConfigurationContainer
	{
		private static string GetAppSetting(string key)
		{
			if (!ConfigurationManager.AppSettings.AllKeys.Any(x => x == key))
			{
				const string errMsgFormat = "Missing required AppSettings key: {0}";
				var errMsg = string.Format(errMsgFormat, key);
				throw new ConfigurationMissingException(errMsg);
			}
			return ConfigurationManager.AppSettings[key];
		}

		private static class AppSettingKeys
		{
			public const string CACHE_EXPIRY_TIME = "CacheExpiryTime";
			public const string DATA_FETCHING_STRATEGY = "DataFetchingStrategy";
		}

		public int CacheExpiryTime
		{
			get
			{
				var appSetting = GetAppSetting(AppSettingKeys.CACHE_EXPIRY_TIME);
				return Convert.ToInt32(appSetting);
			}
		}

		public string DataFetchingStrategy
		{
			get
			{
				return GetAppSetting(AppSettingKeys.DATA_FETCHING_STRATEGY);
			}
		}
	}
}

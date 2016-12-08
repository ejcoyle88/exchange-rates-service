using System;
using System.Configuration;
using System.Linq;
using ExchangeRateService.Core.Exceptions;

namespace ExchangeRateService.Core
{
    internal class ConfigurationContainer
    {
        private int? _cacheExpiryTime;
        private string _dataFetchingStrategy;
        private string _historicalStorageStrategy;

        public int CacheExpiryTime
        {
            get
            {
                if (!_cacheExpiryTime.HasValue)
                {
                    var appSetting = GetAppSetting(AppSettingKeys.CACHE_EXPIRY_TIME);
                    var value = string.IsNullOrEmpty(appSetting) ? 60 : Convert.ToInt32(appSetting);
                    _cacheExpiryTime = value;
                }

                return _cacheExpiryTime.Value;
            }
        }

        public string DataFetchingStrategy
        {
            get
            {
                if (string.IsNullOrEmpty(_dataFetchingStrategy))
                {
                    var dataFetchingStrategy = GetAppSetting(AppSettingKeys.DATA_FETCHING_STRATEGY);
                    _dataFetchingStrategy = string.IsNullOrEmpty(dataFetchingStrategy) ? "google" : dataFetchingStrategy;
                }
                return _dataFetchingStrategy;
            }
        }

        public string HistoricalStorageStrategy
        {
            get
            {
                if (string.IsNullOrEmpty(_historicalStorageStrategy))
                {
                    var historicalStorageStrategy = GetAppSetting(AppSettingKeys.HISTORICAL_STORAGE_STRATEGY);
                    _historicalStorageStrategy = string.IsNullOrEmpty(historicalStorageStrategy) ? "none" : historicalStorageStrategy;
                }
                return _historicalStorageStrategy;
            }
        }

        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings.AllKeys.All(x => x != key) ? null : ConfigurationManager.AppSettings[key];
        }

        protected static string RequireAppSetting(string key)
        {
            var result = GetAppSetting(key);
            if (result == null)
            {
                ThrowConfigurationExceptionFor(key);
            }
            return result;
        }

        public static void EnsureAppSettingExists(string key)
        {
            RequireAppSetting(key);
        }

        private static void ThrowConfigurationExceptionFor(string key)
        {
            const string errMsgFormat = "Missing required AppSettings key: {0}";
            var errMsg = string.Format(errMsgFormat, key);
            throw new ConfigurationMissingException(errMsg);
        }

        private static class AppSettingKeys
        {
            public const string CACHE_EXPIRY_TIME = "CacheExpiryTime";
            public const string DATA_FETCHING_STRATEGY = "DataFetchingStrategy";
            public const string HISTORICAL_STORAGE_STRATEGY = "HistoricalStorageStrategy";
        }
    }
}
# exchange-rates-service-csharp
Pulls in exchange rates from an api, caches them for speed & makes sure it gets as many exchange rate out of the data returned from the api as possible.

## web.config options
### CacheExpiryTime
The number of minutes before the cache should expire. Default: 60 minutes.
### DataFetchingStrategy
The strategy class to call when the API should be hit. Possible values are:
  * OpenExchangeRates
  * Google

Defaults to Google
### HistoricalStorageStrategy
The strategy class to call when the data should be stored for archive purposes. Possible values are:
 * None
 * Mongo

Defaults to None
### MongoDbConnectionString
The connection string that will be used by the Mongo HistoricalStorageStrategy if this is used.
### MongoDbDatabaseName
The mongo database that will be used by the Mongo HistoricalStorageStrategy if this is used.
### OpenExchangeRates.ApiKey
The API key for OpenExchangeRates. Required if this DataFetchingStrategy is used.

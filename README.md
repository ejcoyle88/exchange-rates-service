# exchange-rates-service-csharp
Pulls in exchange rates from an api, caches them for speed & makes sure it gets as many exchange rate out of the data returned from the api as possible.

Defaults to using the [Open Exchange Rates](https://openexchangerates.org/) api. An API key is required inside the web.config.

## web.config options
### CacheExpiryTime
The number of minutes before the cache should expire.
### DataFetchingStrategy
The strategy class to call when the API should be hit. Possible values are:
  OpenExchangeRates
### OpenExchangeRates.ApiKey
The API key for OpenExchangeRates. Required if this DataFetchingStrategy is used.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ExchangeRateService.Common.Models;
using ExchangeRateService.Common.Requests;

namespace ExchangeRateService.Core.DataFetching.Strategies
{
    internal class GoogleFetchingStrategy : IDataFetchingStrategy
    {
        public static string Reference
        {
            get { return "Google"; }
        }

        private const string ApiUriFormat = "https://www.google.co.uk/finance/converter?a=1&from={0}&to={1}";

        public IEnumerable<ExchangeRate> Fetch(ExchangeRatesRequest request)
        {
            var httpRequest = CreateRequest(request);
            var httpResponse = ExecuteHttpRequest(httpRequest);
            var rate = ScrapeExchangeRate(httpResponse);
            
            var exchangeRate = new ExchangeRate(request.CurrencyCode, rate, request.BaseCurrencyCode, DateTime.UtcNow);
            return new List<ExchangeRate> { exchangeRate };
        }

        private static HttpWebRequest CreateRequest(ExchangeRatesRequest request)
        {
            var apiUri = string.Format(ApiUriFormat, request.BaseCurrencyCode, request.CurrencyCode);
            var httpRequest = (HttpWebRequest) WebRequest.Create(apiUri);
            httpRequest.Method = "GET";

            return httpRequest;
        }

        private static string ExecuteHttpRequest(WebRequest request)
        {
            var httpResponse = (HttpWebResponse) request.GetResponse();
                
            var responseStream = httpResponse.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }

            using (var streamReader = new StreamReader(responseStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static decimal ScrapeExchangeRate(string responseText)
        {
            const string spanTag = "<span class=bld>";
            var spanIndex = responseText.IndexOf(spanTag, StringComparison.Ordinal);
            var indexAfterSpan = spanIndex + 16;
            var rateString = responseText.Substring(indexAfterSpan).Split(' ')[0];
            return Convert.ToDecimal(rateString);
        }
    }
}
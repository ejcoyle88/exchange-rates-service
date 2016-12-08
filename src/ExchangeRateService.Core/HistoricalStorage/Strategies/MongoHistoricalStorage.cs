using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateService.Common.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace ExchangeRateService.Core.HistoricalStorage.Strategies
{
    internal class MongoHistoricalStorage : IHistoricalStorageStrategy
    {
        private readonly MongoConfigurationContainer _configuration = new MongoConfigurationContainer();
        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;

        public static string Reference
        {
            get { return "Mongo"; }
        }

        public void Store(IEnumerable<ExchangeRate> exchangeRates)
        {
            var dbModels = GetDbModelsFrom(exchangeRates);
            GetMongoCollection().InsertMany(dbModels);
        }

        private MongoClient GetMongoClient()
        {
            return _mongoClient ?? (_mongoClient = new MongoClient(_configuration.ConnectionString));
        }

        private IMongoDatabase GetMongoDatabase()
        {
            return _mongoDatabase ?? (_mongoDatabase = GetMongoClient().GetDatabase(_configuration.DatabaseName));
        }

        private IMongoCollection<ExchangeRateMongoModel> GetMongoCollection()
        {
            return GetMongoDatabase().GetCollection<ExchangeRateMongoModel>("exchangeRates");
        }

        private static IEnumerable<ExchangeRateMongoModel> GetDbModelsFrom(IEnumerable<ExchangeRate> exchangeRates)
        {
            return exchangeRates.Select(GetDbModelFrom);
        }

        private static ExchangeRateMongoModel GetDbModelFrom(ExchangeRate ex)
        {
            return new ExchangeRateMongoModel(ObjectId.GenerateNewId(), ex.CurrencyCode, ex.ConversionRate, ex.BaseCurrencyCode, ex.Timestamp);
        }

        private class ExchangeRateMongoModel : ExchangeRate
        {
            [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
            public ObjectId Id { get; private set; }

            public ExchangeRateMongoModel(ObjectId id,
                string currencyCode,
                decimal conversionRate,
                string baseCurrency,
                DateTime timestamp)
                : base(currencyCode, conversionRate, baseCurrency, timestamp)
            {
                Id = id;
            }
        }

        private class MongoConfigurationContainer : ConfigurationContainer
        {
            private string _connectionString;
            private string _databaseName;

            public string ConnectionString
            {
                get { return _connectionString ?? (_connectionString = RequireAppSetting("MongoDbConnectionString")); }
            }

            public string DatabaseName
            {
                get { return _databaseName ?? (_databaseName = RequireAppSetting("MongoDbDatabaseName")); }
            }
        }
    }
}
using NUnit.Framework;
using Moq;
using System;
using JsonDeserializer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Common.Models;
using System.Linq;

namespace UnitTests
{    
    public class JsonParserTests
    {
        private JsonParser _parser;
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {            
            if( _parser is null)
            {
                var mockLogger = new Mock<ILogger<JsonParser>>();
                ILogger<JsonParser> logger = mockLogger.Object;

                var copyAppSettings = new Dictionary<string, string>
                {
                    {"Api:PrivateBankUrl", "https://api.privatbank.ua/"},
                    {"Api:UrlParameter", "p24api/exchange_rates?json"},
                };

                _config = new ConfigurationBuilder()
                    .AddInMemoryCollection(copyAppSettings)
                    .Build();

                _parser = new JsonParser(logger, _config);
            }
        }

        [Test]
        public void ValidDateAndURL()
        {
            var testDate = new DateTime(2022, 1, 1);

            DayExchangeRates result = _parser.GetExchangeRates(testDate);

            Assert.IsNotNull(result);
            Assert.AreEqual(testDate.Date, result.Date);
            Assert.AreEqual("UAH", result.BaseCurrencyLit);
            Assert.AreEqual(26, result.ExchangeRate.Count);
            Assert.AreEqual(0.3639700, GetExchangeRateByName(result, "RUB").SaleRateNB);
            Assert.AreEqual(27.2782000, GetExchangeRateByName(result, "USD").SaleRateNB);
        }


        [Test]
        public void NotValidDate()
        {
            DateTime[] dates =
            {
                new DateTime(1000, 1, 1),
                DateTime.UtcNow + TimeSpan.FromDays(10)
            };

            foreach (var d in dates)
            {
                DayExchangeRates result = _parser.GetExchangeRates(d);

                Assert.IsNotNull(result);
                Assert.AreEqual(d.Date, result.Date);
                Assert.AreEqual("UAH", result.BaseCurrencyLit);
                Assert.AreEqual(0, result.ExchangeRate.Count);
            }
        }

        [Test]
        public void NotValidURL()
        {
            string temp = _config["Api:PrivateBankUrl"];

            string[] urls =
            {
                "wrongURL",
                "",
                "http://google.com"
            };

            foreach(var url in urls)
            {
                _config["Api:PrivateBankUrl"] = "wrongURL";

                var testDate = new DateTime(2022, 1, 1);

                DayExchangeRates result = _parser.GetExchangeRates(testDate);

                Assert.IsNull(result);
            }

            _config["Api:PrivateBankUrl"] = temp;
        }

        private ExchangeRate GetExchangeRateByName(DayExchangeRates dayExchangeRates, string name)
        {
            return dayExchangeRates.ExchangeRate.Where(c => c.Currency == name.ToUpper()).FirstOrDefault();
        }


    }
}
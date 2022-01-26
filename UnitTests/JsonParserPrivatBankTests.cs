using NUnit.Framework;
using Moq;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using API.ApiPrivatBank;
using Api.ApiPrivatBank.Models;

namespace UnitTests
{    
    public class JsonParserPrivatBankTests
    {
        private JsonParserPrivatBank _parser;
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {           
            var mockLogger = new Mock<ILogger<JsonParserPrivatBank>>();
            ILogger<JsonParserPrivatBank> logger = mockLogger.Object;

            var copyAppSettings = new Dictionary<string, string>
            {
                {"Api:PrivatBank:Url", "https://api.privatbank.ua/p24api/exchange_rates?json"}                   
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(copyAppSettings)
                .Build();

            _parser = new JsonParserPrivatBank(logger, _config);            
        }

        [Test]
        public void Valid_Date_And_URL()
        {
            var testDate = new DateTime(2022, 1, 1);

            DailyExchangeRates result = _parser.GetExchangeRates(testDate);

            Assert.IsNotNull(result);
            Assert.AreEqual(testDate.Date, result.Date);
            Assert.AreEqual("UAH", result.BaseCurrencyName);
            Assert.AreEqual(26, result.ExchangeRates.Count);
            Assert.AreEqual(0.3639700, GetExchangeRateByName(result, "RUB").Rate);
            Assert.AreEqual(27.2782000, GetExchangeRateByName(result, "USD").Rate);
        }


        [Test]
        public void Not_Valid_Date()
        {
            DateTime[] dates =
            {
                new DateTime(1000, 1, 1),
                DateTime.UtcNow + TimeSpan.FromDays(10)
            };

            foreach (var d in dates)
            {
                DailyExchangeRates result = _parser.GetExchangeRates(d);

                Assert.IsNotNull(result);
                Assert.AreEqual(d.Date, result.Date);
                Assert.AreEqual("UAH", result.BaseCurrencyName);
                Assert.AreEqual(0, result.ExchangeRates.Count);
            }
        }

        [Test]
        public void Not_Valid_URL()
        {
            string temp = _config["Api:PrivatBank:Url"];

            string[] urls =
            {
                "wrongURL",
                string.Empty                
            };

            foreach(var url in urls)
            {   
                _config["Api:PrivatBank:Url"] = url;

                var testDate = new DateTime(2022, 1, 1);

                DailyExchangeRates result = _parser.GetExchangeRates(testDate);

                Assert.IsNull(result);
            }

            _config["Api:PrivatBank:Url"] = temp;            
        }

        private ExchangeRate GetExchangeRateByName(DailyExchangeRates dayExchangeRates, string name)
        {
            return dayExchangeRates.ExchangeRates.Where(c => c.CurrencyName == name.ToUpper()).FirstOrDefault();
        }


    }
}
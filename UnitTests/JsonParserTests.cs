using NUnit.Framework;
using Moq;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using API.Common;
using API.Common.Interfaces;
using API.ApiPrivatBank;
using Api.ApiPrivatBank.Models;

namespace UnitTests
{    
    public class JsonParserTests
    {
        private JsonParserPrivatBank _parser;
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {            
            if( _parser is null)
            {
                var mockLogger = new Mock<ILogger<JsonParserPrivatBank>>();
                ILogger<JsonParserPrivatBank> logger = mockLogger.Object;

                var copyAppSettings = new Dictionary<string, string>
                {
                    {"Api:PrivateBankUrl", "https://api.privatbank.ua/"},
                    {"Api:UrlParameter", "p24api/exchange_rates?json"},
                };

                _config = new ConfigurationBuilder()
                    .AddInMemoryCollection(copyAppSettings)
                    .Build();

                _parser = new JsonParserPrivatBank(logger, _config);
            }
        }

        [Test]
        public void ValidDateAndURL()
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
        public void NotValidDate()
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
                _config["Api:PrivateBankUrl"] = url;

                var testDate = new DateTime(2022, 1, 1);

                DailyExchangeRates result = _parser.GetExchangeRates(testDate);

                Assert.IsNull(result);
            }

            _config["Api:PrivateBankUrl"] = temp;            
        }

        private ExchangeRate GetExchangeRateByName(DailyExchangeRates dayExchangeRates, string name)
        {
            return dayExchangeRates.ExchangeRates.Where(c => c.CurrencyName == name.ToUpper()).FirstOrDefault();
        }


    }
}
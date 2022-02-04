using NUnit.Framework;
using Moq;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using API.ApiPrivatBank;
using API.Common.Models;
using API.ApiPrivatBank.Models;
using RestSharp;

namespace UnitTests.ApiPrivatBank
{    
    public class ApiPrivatBankTests
    {     
        public CurrencyDataProviderPrivatBank GetProvider(IRestResponse response)
        {           
            var mockLogger = new Mock<ILogger<CurrencyDataProviderPrivatBank>>();
            ILogger<CurrencyDataProviderPrivatBank> logger = mockLogger.Object;

            var options = new ApiPrivatBankOptions()
            {
                Url = "http://NotEmpty.com"
            };
            
            var mockRestClient = new Mock<IRestClient>();

            mockRestClient.Setup(r => r.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            IRestClient client = mockRestClient.Object;
            
            return new CurrencyDataProviderPrivatBank(logger, options, client);
        }


        [Test]
        public void Valid_Json()
        {
            IRestResponse response = new RestResponse()
            {
                ResponseStatus = ResponseStatus.Completed,
                Content = Constants.ValidJson                
            };

            CurrencyDataProviderPrivatBank provider = GetProvider(response);

            var answer = provider.GetExchangeRates(new DateTime(2022, 2, 4));

            Assert.IsNotNull(answer);
            Assert.AreEqual(2, answer.ExchangeRates.Count);
            Assert.AreEqual(28.2701, answer.ExchangeRates.First().Rate);
            Assert.AreEqual(0.0026336, answer.ExchangeRates.Last().Rate);
            Assert.AreEqual(new DateTime(2022, 2, 4), answer.Date);
        }

        [Test]
        public void ResponseStatus_IsError()
        { 
            IRestResponse response = new RestResponse()
            {
                ResponseStatus = ResponseStatus.Error,
                Content = Constants.ValidJson
            };

            CurrencyDataProviderPrivatBank provider = GetProvider(response);

            var answer = provider.GetExchangeRates(new DateTime(2022, 2, 4));

            Assert.IsNull(answer);            
        }

        [Test]
        public void Broken_Structure_Json()
        {
            IRestResponse response = new RestResponse()
            {
                ResponseStatus = ResponseStatus.Completed,
                Content = Constants.BrokenStructureJson
            };

            CurrencyDataProviderPrivatBank provider = GetProvider(response);

            var answer = provider.GetExchangeRates(new DateTime(2022, 2, 4));

            Assert.IsNull(answer);
        }

        [Test] 
        public void Empty_Json()
        {
            IRestResponse response = new RestResponse()
            {
                ResponseStatus = ResponseStatus.Completed,
                Content = Constants.EmptyJson
            };

            CurrencyDataProviderPrivatBank provider = GetProvider(response);

            var answer = provider.GetExchangeRates(new DateTime(2022, 2, 4));

            Assert.IsNull(answer);
        }

        [Test]
        public void Empty_One_Value()
        {
            IRestResponse response = new RestResponse()
            {
                ResponseStatus = ResponseStatus.Completed,
                Content = Constants.EmptyValueJson
            };

            CurrencyDataProviderPrivatBank provider = GetProvider(response);

            var answer = provider.GetExchangeRates(new DateTime(2022, 2, 4));

            Assert.IsNull(answer);
        }
    }
}
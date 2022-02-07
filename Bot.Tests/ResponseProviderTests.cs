using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Bot.Services;
using Bot.Services.Strategies;
using CacheContext;
using API.Services.Factory;

namespace Bot.Tests
{
    public class ResponseProviderTests 
    {
        public ResponseProviderSuccessor GetResponseProvider()
        {            
            var mockLogger = new Mock<ILogger<ResponseProvider>>();
            ILogger<ResponseProvider> logger = mockLogger.Object;

            var mockCache = new Mock<ICacheManager>();
            ICacheManager cacheManager = mockCache.Object;

            var mockFactory = new Mock<ICurrencyDataFactory>();
            ICurrencyDataFactory factory = mockFactory.Object;

            return new ResponseProviderSuccessor(logger, cacheManager, factory);
        }

        [TestCase("usd 10.10.2020")]
        [Test]
        public void GetStrategy_DayAndMonthAreTwoDigit_ReturnsCorrectRequestStrategy(string request)
        {  
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(CorrectRequestStrategy), strategy);            
        }

        [TestCase("usd 1.1.2020")]
        [Test]
        public void GetStrategy_DayAndMonthAreOneDigit_ReturnsCorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(CorrectRequestStrategy), strategy);
        }

        [TestCase("usd 01.1.2020")]
        [Test]
        public void GetStrategy_MonthIsOneDigit_ReturnsCorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(CorrectRequestStrategy), strategy);
        }

        [TestCase("usd 1.01.2020")]
        [Test]
        public void GetStrategy_DayIsOneDigit_ReturnsCorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(CorrectRequestStrategy), strategy);
        }

        [TestCase("usd")]
        [Test]
        public void GetStrategy_OnlyNameCurrency_ReturnsCorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(CorrectRequestStrategy), strategy);
        }

        [TestCase("usd 10.120.2020")]
        [Test]
        public void GetStrategy_IncorrectNumberMonth_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }

        [TestCase("usd t1.10.2020")]
        [Test]
        public void GetStrategy_IncorrectNumberDay_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }

        [TestCase("usd 10.10.20")]
        [Test]
        public void GetStrategy_IncorrectNumberYear_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }

        [TestCase("us")]
        [Test]
        public void GetStrategy_OnlyTwoLetterCurrency_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }

        [TestCase("usdd")]
        [Test]
        public void GetStrategy_OnlyFourLetterCurrency_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }

        [TestCase("u8d")]
        [Test]
        public void GetStrategy_OnlyCurrencyWithNumber_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }



        [TestCase("руб")]
        [Test]
        public void GetStrategy_OnlyIncorrectNameCurrency_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }

        [TestCase("руб 10.10.2020")]
        [Test]
        public void GetStrategy_IncorrectNameCurrencyWithCorrectDate_ReturnsIncorrectRequestStrategy(string request)
        {
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(IncorrectRequestStrategy), strategy);
        }
       
        [Test]
        public void GetStrategy_StartCommand_ReturnsGreetingStrategy()
        {
            string request = Resources.Commands.StartCommand;
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(GreetingStrategy), strategy);
        }

        [Test]
        public void GetStrategy_HelpCommand_ReturnsCallHelpStrategy()
        {
            string request = Resources.Commands.HelpCommand;
            ResponseProviderSuccessor provider = GetResponseProvider();
            var strategy = provider.GetStrategyTestMethod(request);
            Assert.IsInstanceOf(typeof(CallHelpStrategy), strategy);
        }
    }
}
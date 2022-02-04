using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Bot.Services;
using Bot.Services.Strategies;
using CacheContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Services.Factory;

namespace UnitTests.Service_ResponseProvider
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

        [Test]
        public void Define_CorrectStrategy()
        {
            string[] requests =
            {
                "usd 10.10.2020",
                "usd 1.1.2020",
                "usd 01.1.2020",
                "usd 1.01.2020",
                "usd"
            };

            ResponseProviderSuccessor provider = GetResponseProvider();

            foreach(var r in requests)
            {
                var strategy = provider.GetStrategyTestMethod(r);                
                Assert.IsTrue(strategy is CorrectRequestStrategy);
            }
        }

        [Test]
        public void Define_IncorrectStrategy()
        {
            string[] requests =
            {
                "usd 10.120.2020",
                "usd t1.1.2020",
                "usd 01.1.20",
                "usd 0s1.01.2020",
                "ud",
                "us4",
                "usdd",
                "usd s",
                "usd tt.tt.tttt",
                "usd t.t.tttt"
            };

            ResponseProviderSuccessor provider = GetResponseProvider();

            foreach (var r in requests)
            {
                var strategy = provider.GetStrategyTestMethod(r);
                Assert.IsTrue(strategy is IncorrectRequestStrategy);
            }
        }

        [Test]
        public void Define_GreetingStrategy()
        {
            string request = Bot.Resources.Commands.StartCommand;

            ResponseProviderSuccessor provider = GetResponseProvider();

            var strategy = provider.GetStrategyTestMethod(request);

            Assert.IsTrue(strategy is GreetingStrategy);
        }

        [Test]
        public void Define_HelpStrategy()
        {
            string request = Bot.Resources.Commands.HelpCommand;

            ResponseProviderSuccessor provider = GetResponseProvider();

            var strategy = provider.GetStrategyTestMethod(request);

            Assert.IsTrue(strategy is CallHelpStrategy);
        }
    }
}
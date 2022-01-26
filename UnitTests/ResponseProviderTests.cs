using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Bot.Services;
using Bot.Services.Strategies;
using CacheContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Common;
using API.ApiPrivatBank;

namespace UnitTests
{
    public class ResponseProviderTests 
    {
        private ResponseProviderSuccessor _responseProvider;
        
        [SetUp]
        public void Setup()
        {
            if(_responseProvider is null)
            {
                var mockLogger = new Mock<ILogger<ResponseProvider>>();
                ILogger<ResponseProvider> logger = mockLogger.Object;

                var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) => 
                    {
                        services.AddMemoryCache();
                        services.AddScoped<CacheManager>();
                        services.AddScoped<JsonParserPrivatBank>();
                    })
                    .Build();

                var cacheManager = ActivatorUtilities.CreateInstance<CacheManager>(host.Services);
                
                var parser = ActivatorUtilities.CreateInstance<JsonParserPrivatBank>(host.Services);

                _responseProvider = new ResponseProviderSuccessor(logger, cacheManager, parser);
            }
        }

        [Test]
        public void TestOfDefineCorrectStrategy()
        {
            string[] requests =
            {
                "usd 10.10.2020",
                "usd 1.1.2020",
                "usd 01.1.2020",
                "usd 1.01.2020",
                "usd"
            };

            foreach(var r in requests)
            {
                var strategy = _responseProvider.GetStrategyTestMethod(r);                
                Assert.IsTrue(strategy is CorrectRequestStrategy);
            }
        }

        [Test]
        public void TestOfDefineIncorrectStrategy()
        {
            string[] requests =
            {
                "usd 10.120.2020",
                "usd t1.1.2020",
                "usd 01.1.20",
                "usd 0s1.01.2020",
                "ud",
                "us4",
                "usdd"
            };

            foreach (var r in requests)
            {
                var strategy = _responseProvider.GetStrategyTestMethod(r);
                Assert.IsTrue(strategy is IncorrectRequestStrategy);
            }
        }

        [Test]
        public void TestOfDefineGreetingStrategy()
        {
            string request = Bot.Resources.Commands.StartCommand;

            var strategy = _responseProvider.GetStrategyTestMethod(request);

            Assert.IsTrue(strategy is GreetingStrategy);
        }

        [Test]
        public void TestOfDefineHelpStrategy()
        {
            string request = Bot.Resources.Commands.HelpCommand;

            var strategy = _responseProvider.GetStrategyTestMethod(request);

            Assert.IsTrue(strategy is CallHelpStrategy);
        }
    }
}
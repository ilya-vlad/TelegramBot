using API.Services.Factory;
using Bot.Services;
using Bot.Services.Strategies;
using CacheContext;
using Microsoft.Extensions.Logging;

namespace UnitTests.Service_ResponseProvider
{
    public class ResponseProviderSuccessor : ResponseProvider
    {
        public ResponseProviderSuccessor(ILogger<ResponseProvider> logger, ICacheManager cache, ICurrencyDataFactory currencyFactory) 
            : base(logger, cache, currencyFactory)
        {           
        }
        
        public IResponseStrategy GetStrategyTestMethod(string text)
        {
            return base.GetStrategy(text);
        }
    }
}
using API.ApiPrivatBank;
using API.Common;
using API.Services.Factory;
using Bot.Services;
using Bot.Services.Strategies;
using CacheContext;
using Microsoft.Extensions.Logging;

namespace UnitTests
{
    public class ResponseProviderSuccessor : ResponseProvider
    {
        public ResponseProviderSuccessor(ILogger<ResponseProvider> logger, CacheManager cache, IJsonParserFactory parserFactory) 
            : base(logger, cache, parserFactory)
        {           
        }
        
        public IResponseStrategy GetStrategyTestMethod(string text)
        {
            return base.GetStrategy(text);
        }
    }
}
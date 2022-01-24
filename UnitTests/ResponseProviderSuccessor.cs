using Bot.Services;
using Bot.Services.Strategies;
using CacheContext;
using JsonDeserializer;
using Microsoft.Extensions.Logging;

namespace UnitTests
{
    public class ResponseProviderSuccessor : ResponseProvider
    {
        public ResponseProviderSuccessor(ILogger<ResponseProvider> logger, CacheManager cache, JsonParser parser) 
            : base(logger, cache, parser)
        {           
        }
        
        public IResponseStrategy GetStrategyTestMethod(string text)
        {
            return base.GetStrategy(text);
        }
    }
}
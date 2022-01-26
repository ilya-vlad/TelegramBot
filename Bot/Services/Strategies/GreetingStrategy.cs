using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public class GreetingStrategy : BaseResponseStrategy<GreetingStrategy>
    {
        public GreetingStrategy(CacheManager cache, IJsonParser parser) 
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {            
            return Resources.Messages.Greeting;
        }
    }
}

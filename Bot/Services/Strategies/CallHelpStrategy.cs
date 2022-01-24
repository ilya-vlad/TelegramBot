using CacheContext;
using JsonDeserializer;
using System;

namespace Bot.Services.Strategies
{
    public class CallHelpStrategy : BaseResponseStrategy<CallHelpStrategy>
    {
        public CallHelpStrategy(CacheManager cache, JsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            return $"{Resources.Messages.Help}";
        }
    }
}

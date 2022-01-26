using API.ApiPrivatBank;
using API.Common;
using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public class CallHelpStrategy : BaseResponseStrategy<CallHelpStrategy>
    {
        public CallHelpStrategy(CacheManager cache, IJsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            return $"{Resources.Messages.Help}";
        }
    }
}

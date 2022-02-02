using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public class IncorrectRequestStrategy : BaseResponseStrategy<IncorrectRequestStrategy>
    {
        public IncorrectRequestStrategy(CacheManager cache, ICurrencyDataProvider parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            return Resources.Messages.IncorrectRequest;
        }
    }
}

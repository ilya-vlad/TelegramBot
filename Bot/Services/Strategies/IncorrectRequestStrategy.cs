using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public class IncorrectRequestStrategy : BaseResponseStrategy
    {
        public IncorrectRequestStrategy(ICacheManager cache, ICurrencyDataProvider currencyProvider)
            : base(cache, currencyProvider)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            return Resources.Messages.IncorrectRequest;
        }
    }
}

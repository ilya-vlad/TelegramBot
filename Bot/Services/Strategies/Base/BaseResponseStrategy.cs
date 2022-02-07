using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public abstract class BaseResponseStrategy : IResponseStrategy
    {       
        protected readonly ICacheManager _cache;
        protected readonly ICurrencyDataProvider _currencyProvider;

        public BaseResponseStrategy(ICacheManager cache, ICurrencyDataProvider currencyProvider)
        {            
            _cache = cache;
            _currencyProvider = currencyProvider;            
        }

        public abstract string GetResponse(string currency, DateTime date);
    }
}

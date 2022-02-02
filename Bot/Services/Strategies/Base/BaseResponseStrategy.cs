using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public abstract class BaseResponseStrategy<T> : IResponseStrategy where T : class
    {       
        protected readonly CacheManager _cache;
        protected readonly ICurrencyDataProvider _parser;

        public BaseResponseStrategy(CacheManager cache, ICurrencyDataProvider parser)
        {            
            _cache = cache;
            _parser = parser;            
        }

        public abstract string GetResponse(string currency, DateTime date);
    }
}

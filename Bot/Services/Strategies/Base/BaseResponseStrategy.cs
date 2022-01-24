using CacheContext;
using JsonDeserializer;
using System;

namespace Bot.Services.Strategies
{
    public abstract class BaseResponseStrategy<T> : IResponseStrategy where T : class
    {       
        protected readonly CacheManager _cache;
        protected readonly JsonParser _parser;

        public BaseResponseStrategy(CacheManager cache, JsonParser parser)
        {            
            _cache = cache;
            _parser = parser;            
        }

        public abstract string GetResponse(string currency, DateTime date);
    }
}

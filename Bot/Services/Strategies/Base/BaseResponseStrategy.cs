using API.ApiPrivatBank;
using API.Common;
using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public abstract class BaseResponseStrategy<T> : IResponseStrategy where T : class
    {       
        protected readonly CacheManager _cache;
        protected readonly IJsonParser _parser;

        public BaseResponseStrategy(CacheManager cache, IJsonParser parser)
        {            
            _cache = cache;
            _parser = parser;            
        }

        public abstract string GetResponse(string currency, DateTime date);
    }
}

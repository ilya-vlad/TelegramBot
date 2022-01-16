using Bot.Services.Cache;


namespace Bot.Services.Strategies
{
    public abstract class BaseResponseStrategy<T> : IResponseStrategy where T : class
    {       
        protected readonly CacheService _cache;
        protected readonly JsonParser _parser;

        public BaseResponseStrategy(CacheService cache, JsonParser parser)
        {            
            _cache = cache;
            _parser = parser;            
        }

        public abstract string GetResponse(long chatId, string text);
    }
}

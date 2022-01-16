using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace Bot.Services.Cache.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly IMemoryCache _cache;
        protected readonly ILogger<T> _logger;

        public BaseRepository(ILogger<T> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
    }
}

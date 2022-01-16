using Bot.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace Bot.Services.Cache.Repositories
{
    public class UserInputDataRepository : BaseRepository<UserInputDataRepository>
    {
        public UserInputDataRepository(ILogger<UserInputDataRepository> logger, IMemoryCache cache) : base(logger, cache)
        {
        }

        public void Add(UserInputData data)
        {
            if (data == null)
            {
                return;
            }

            if(!_cache.TryGetValue(data.ChatId, out _))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(data.ChatId, data, cacheEntryOptions);

                _logger.LogInformation($"Added input data of user with chatID = {data.ChatId} to cache.");
            }
        }

        public UserInputData GetByChatId(long chatId)
        {
            UserInputData userInputData;

            if (_cache.TryGetValue(chatId, out userInputData))
            {
                return userInputData;
            }

            return null;
        }

        public void RemoveByChatId(long chatId)
        {
            _cache.Remove(chatId);
            _logger.LogInformation($"Removed input data of user with chatID = {chatId} from cache.");
        }
    }
}
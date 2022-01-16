using Bot.Common;
using Bot.Services.Cache;
using System;


namespace Bot.Services.Strategies
{
    public class CorrectDateStrategy : BaseResponseStrategy<CorrectDateStrategy>
    {
        public CorrectDateStrategy(CacheService cache, JsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(long chatID, string text)
        {
            var date = DateTime.Parse(text);
            var userData = new UserInputData()
            {
                ChatId = chatID,
                Date = date
            };

            _cache.UserInputData.Add(userData);

            if (_cache.ExchangeRates.IsEmpty(date))
            {
                _parser.ParseToCache(date);
            }

            return "Enter the currency.";            
        }
    }
}

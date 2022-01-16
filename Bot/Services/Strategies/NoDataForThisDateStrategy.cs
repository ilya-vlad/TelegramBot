using Bot.Services.Cache;


namespace Bot.Services.Strategies
{
    public class NoDataForThisDateStrategy : BaseResponseStrategy<NoDataForThisDateStrategy>
    {
        public NoDataForThisDateStrategy(CacheService cache, JsonParser parser) 
            : base(cache, parser)
        {
        }

        public override string GetResponse(long chatID, string text)
        {
            _cache.UserInputData.RemoveByChatId(chatID);
            return "Exchange rate data not found.\nEnter another date.";
        }
    }
}

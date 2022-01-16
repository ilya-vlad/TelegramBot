using Bot.Services.Cache;


namespace Bot.Services.Strategies
{
    public class UncorrectCurrencyStrategy : BaseResponseStrategy<UncorrectCurrencyStrategy>
    {
        public UncorrectCurrencyStrategy(CacheService cache, JsonParser parser) 
            : base(cache, parser)
        {
        }

        public override string GetResponse(long chatID, string text)
        {
            return "Wrong currency. Try again.";
        }
    }
}

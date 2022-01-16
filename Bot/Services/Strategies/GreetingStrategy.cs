using Bot.Services.Cache;


namespace Bot.Services.Strategies
{
    public class GreetingStrategy : BaseResponseStrategy<GreetingStrategy>
    {
        public GreetingStrategy(CacheService cache, JsonParser parser) 
            : base(cache, parser)
        {
        }

        public override string GetResponse(long chatID, string text)
        {
            return "Enter the date.";
        }
    }
}

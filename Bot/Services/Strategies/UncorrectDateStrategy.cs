using Bot.Services.Cache;


namespace Bot.Services.Strategies
{
    public class UncorrectDateStrategy : BaseResponseStrategy<UncorrectDateStrategy>
    {
        public UncorrectDateStrategy(CacheService cache, JsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(long chatID, string text)
        {
            return "Wrong date. Try again.";
        }
    }
}



namespace Bot.Services.Strategies
{
    public interface IResponseStrategy
    {
        public string GetResponse(long chatID, string text);
    }
}

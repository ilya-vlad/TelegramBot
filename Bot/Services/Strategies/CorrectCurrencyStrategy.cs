using Bot.Common;
using Bot.Services.Cache;


namespace Bot.Services.Strategies
{
    public class CorrectCurrencyStrategy : BaseResponseStrategy<CorrectCurrencyStrategy>
    {
        public CorrectCurrencyStrategy(CacheService cache, JsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(long chatID, string text)
        {
            UserInputData userInputData = _cache.UserInputData.GetByChatId(chatID);
            CurrencyItem currency = _cache.ExchangeRates.GetByDate(userInputData.Date).GetCurrency(text);
            _cache.UserInputData.RemoveByChatId(chatID);

            var response = $"Exchange rate on {userInputData.Date.ToShortDateString()}\n" +
            $"1 {currency.NameCurrency} = {currency.ValueCurrency} UAH\n\n" +
                $"Please, enter the date again.";
            return response;
        }
    }
}

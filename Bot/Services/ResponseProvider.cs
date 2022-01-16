using Bot.Common;
using Bot.Services.Cache;
using Bot.Services.Strategies;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;


namespace Bot.Services
{
    public class ResponseProvider
    {
        private readonly ILogger<ResponseProvider> _logger;
        private readonly CacheService _cache;
        private readonly JsonParser _parser;
        private IResponseStrategy _strategy;

        public ResponseProvider(ILogger<ResponseProvider> logger, CacheService cache, JsonParser parser)
        {
            _logger = logger;
            _cache = cache;
            _parser = parser;
        }

        public string GetResponseMessage(long chatId, string userText)
        {
            DefineStrategy(chatId, userText);

            return _strategy.GetResponse(chatId, userText);
        }

        private void DefineStrategy(long chatId, string userText)
        {
            UserInputData userInputData = _cache.UserInputData.GetByChatId(chatId);

            if(userText == "/start")
            {
                _strategy = new GreetingStrategy(_cache, _parser);                
            }
            else if(userInputData == null)
            {
                if (DateTime.TryParse(userText, out _))
                {
                    _strategy = new CorrectDateStrategy(_cache, _parser);
                    _logger.LogInformation($"User entered a valid date.");
                }
                else
                {
                    _strategy = new UncorrectDateStrategy(_cache, _parser);
                    _logger.LogInformation($"User entered invalid date.");
                }             
            }
            else
            {
                ExchangeRatesOfOneDay exchangeRates = _cache.ExchangeRates.GetByDate(userInputData.Date);
                CurrencyItem requestCurrency = exchangeRates.GetCurrency(userText);

                if (!exchangeRates.Currencies.Any())
                {
                    _strategy = new NoDataForThisDateStrategy(_cache, _parser);
                    _logger.LogInformation($"No exchange rates found for the specified date.");
                }
                else if (requestCurrency == null)
                {
                    _strategy = new UncorrectCurrencyStrategy(_cache, _parser);
                    _logger.LogInformation($"User entered invalid name of currency.");
                }
                else
                {                    
                    _strategy = new CorrectCurrencyStrategy(_cache, _parser);
                    _logger.LogInformation($"User entered a valid name of currency.");
                }
            }
        }
    }
}

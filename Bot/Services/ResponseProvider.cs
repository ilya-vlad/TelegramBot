using API.Common.Interfaces;
using API.Services.Factory;
using Bot.Services.Strategies;
using CacheContext;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Bot.Services
{
    public class ResponseProvider
    {
        private readonly ILogger<ResponseProvider> _logger;
        private readonly ICacheManager _cache;
        private readonly ICurrencyDataProvider _currencyProvider;
        private IResponseStrategy _strategy;

        private string _currency;
        private DateTime _date;

        public ResponseProvider(ILogger<ResponseProvider> logger, ICacheManager cache, ICurrencyDataFactory currencyFactory)
        {
            _logger = logger;
            _cache = cache;
            _currencyProvider = currencyFactory.GetCurrencyDataProvider();
        }

        public string GetResponseMessage(string userText)
        {            
            _strategy = GetStrategy(userText);

            _logger.LogInformation($"Set {_strategy.GetType().Name}");

            string response = _strategy.GetResponse(_currency, _date);

            return $"{response}\n\n" +
                $"{Resources.Messages.TryAgain}\n" +
                $"{Resources.Messages.ExampleRequest}";
        }


        protected IResponseStrategy GetStrategy(string userText)
        {
            if (userText.Equals(Resources.Commands.StartCommand))
            {
                return new GreetingStrategy(_cache, _currencyProvider);
            }
            else if (userText.Equals(Resources.Commands.HelpCommand))
            {
                return new CallHelpStrategy(_cache, _currencyProvider);
            }    
            //other strategies

            try
            {
                return IsValidInput(userText) == true 
                    ? new CorrectRequestStrategy(_cache, _currencyProvider) 
                    : new IncorrectRequestStrategy(_cache, _currencyProvider);               
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"{ex.Message}");
            }

            return new IncorrectRequestStrategy(_cache, _currencyProvider);
        }

        private bool IsValidInput(string userText)
        {
            string patternWithDate = @"^[a-z]{3} \d{1,2}.\d{1,2}.\d{4}$";
            string patternWithoutDate = @"^[a-z]{3}$";

            if (Regex.IsMatch(userText, patternWithDate, RegexOptions.IgnoreCase))
            {
                var tokens = userText.Split(' ');

                _currency = tokens[0];

                if (DateTime.TryParse(tokens[1], out _date))
                {
                    return true;
                }
            }
            else if (Regex.IsMatch(userText, patternWithoutDate, RegexOptions.IgnoreCase))
            {
                _currency = userText;
                _date = DateTime.UtcNow;

                return true;
            }

            return false;
        }
    }
}
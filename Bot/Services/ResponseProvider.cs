using Bot.Services.Strategies;
using CacheContext;
using JsonDeserializer;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Bot.Services
{
    public class ResponseProvider
    {
        private readonly ILogger<ResponseProvider> _logger;
        private readonly CacheManager _cache;
        private readonly JsonParser _parser;
        private IResponseStrategy _strategy;

        private string _currency;
        private DateTime _date;

        public ResponseProvider(ILogger<ResponseProvider> logger, CacheManager cache, JsonParser parser)
        {
            _logger = logger;
            _cache = cache;
            _parser = parser;
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
                return new GreetingStrategy(_cache, _parser);
            }
            else if (userText.Equals(Resources.Commands.HelpCommand))
            {
                return new CallHelpStrategy(_cache, _parser);
            }    
            //other strategies

            try
            {
                return IsValidInput(userText) == true 
                    ? new CorrectRequestStrategy(_cache, _parser) 
                    : new IncorrectRequestStrategy(_cache, _parser);               
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"{ex.Message}");
            }

            return new IncorrectRequestStrategy(_cache, _parser);
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
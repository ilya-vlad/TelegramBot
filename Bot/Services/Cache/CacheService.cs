using Bot.Services.Cache.Repositories;


namespace Bot.Services.Cache
{
    public class CacheService
    {
        private ExchangeRatesRepository _exchangeRatesRepo;
        private UserInputDataRepository _userInputData;

        public ExchangeRatesRepository ExchangeRates => _exchangeRatesRepo;

        public UserInputDataRepository UserInputData => _userInputData;

        public CacheService(ExchangeRatesRepository exchangeRatesRepo, UserInputDataRepository userInputDataRepo)
        {
            _exchangeRatesRepo = exchangeRatesRepo;
            _userInputData = userInputDataRepo;
        }
    }
}
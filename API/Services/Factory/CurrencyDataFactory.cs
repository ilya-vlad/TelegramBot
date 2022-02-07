using API.ApiPrivatBank;
using API.ApiRapid;
using API.Common.Interfaces;
using API.Services.Factory.Models;
using Microsoft.Extensions.Logging;
using System;


namespace API.Services.Factory
{
    public class CurrencyDataFactory : ICurrencyDataFactory
    {
        private readonly ILogger<CurrencyDataFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ApiFactoryOptions _options;

        public CurrencyDataFactory(ILogger<CurrencyDataFactory> logger, IServiceProvider serviceProvider, ApiFactoryOptions options)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public ICurrencyDataProvider GetCurrencyDataProvider()
        {
            try
            { 
                return _options.ApiCurrent switch
                {
                    ApiEnum.PrivatBank => (ICurrencyDataProvider)_serviceProvider.GetService(typeof(CurrencyDataProviderPrivatBank)),
                    ApiEnum.RapidApi => (ICurrencyDataProvider)_serviceProvider.GetService(typeof(CurrencyDataProviderRapid)),
                    _ => throw new ArgumentOutOfRangeException(nameof(_options.ApiCurrent), $"Api of '{_options.ApiCurrent}' is not supported."),
                };
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return null;
            }
            
        }
    }
}

using API.ApiPrivatBank;
using API.ApiRapid;
using API.Common.Interfaces;
using API.Services.Factory.Models;
using Microsoft.Extensions.Configuration;
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
                string apiName = _options.ApiCurrent;

                if (string.IsNullOrEmpty(apiName))
                {
                    throw new ArgumentNullException(nameof(apiName), "Current api is null");
                }

                return apiName switch
                {
                    "PrivatBank" => (ICurrencyDataProvider)_serviceProvider.GetService(typeof(CurrencyDataProviderPrivatBank)),
                    "RapidApi" => (ICurrencyDataProvider)_serviceProvider.GetService(typeof(CurrencyDataProviderRapid)),
                    _ => throw new ArgumentOutOfRangeException(nameof(apiName), $"Api of '{apiName}' is not supported."),
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

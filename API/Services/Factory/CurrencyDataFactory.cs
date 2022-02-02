using API.ApiPrivatBank;
using API.ApiRapid;
using API.Common.Interfaces;
using API.Services.Factory.Models;
using Microsoft.Extensions.Configuration;
using System;


namespace API.Services.Factory
{
    public class CurrencyDataFactory : ICurrencyDataFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApiFactoryOptions _options;

        public CurrencyDataFactory(IServiceProvider serviceProvider, ApiFactoryOptions options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public ICurrencyDataProvider GetCurrencyDataProvider()
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
                _ => throw new ArgumentOutOfRangeException(nameof(apiName), $"Api of {apiName} is not supported."),
            };
        }
    }
}

using API.ApiPrivatBank;
using API.ApiRapid;
using API.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;


namespace API.Services.Factory
{
    public class JsonParserFactory : IJsonParserFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;

        public JsonParserFactory(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }
        public IJsonParser GetJsonParser()
        {
            string apiName = _config.GetValue<string>("Api:Current");

            if (string.IsNullOrEmpty(apiName))
            {
                throw new ArgumentNullException(nameof(apiName), "Current api is null");
            }

            switch (apiName)
            {
                case "PrivatBank" : return (IJsonParser)_serviceProvider.GetService(typeof(JsonParserPrivatBank));
                case "RapidApi" : return (IJsonParser)_serviceProvider.GetService(typeof(JsonParserRapid));
                default: throw new ArgumentOutOfRangeException(nameof(apiName), $"Api of {apiName} is not supported.");
            }
        }
    }
}

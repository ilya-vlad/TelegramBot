using Serilog;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Bot.Services;
using CacheContext;
using API.ApiPrivatBank;
using API.Common.Interfaces;
using API.ApiRapid;
using API.Services.Factory;
using API.ApiPrivatBank.Models;
using API.ApiRapid.Models;
using Bot.Models;
using CacheContext.Models;
using API.Services.Factory.Models;
using RestSharp;

namespace Bot
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting...");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddMemoryCache();                    
                    services.AddScoped<ICacheManager, CacheManager>();
                    services.AddScoped<ResponseProvider>();
                    services.AddSingleton<TelegramBot>();

                    services.AddScoped<IRestClient, RestClient>();
                    
                    services.AddSingleton<ICurrencyDataFactory, CurrencyDataFactory>();

                    services.AddSingleton<CurrencyDataProviderPrivatBank>();
                    services.AddSingleton<ICurrencyDataProvider, CurrencyDataProviderPrivatBank>( 
                        s => s.GetService<CurrencyDataProviderPrivatBank>());

                    services.AddSingleton<CurrencyDataProviderRapid>();
                    services.AddSingleton<ICurrencyDataProvider, CurrencyDataProviderRapid>( 
                        s => s.GetService<CurrencyDataProviderRapid>());



                    ApiFactoryOptions optionsFactory = context.Configuration
                        .GetSection(OptionsKeys.Api).Get<ApiFactoryOptions>();

                    ApiPrivatBankOptions optionsPrivatBank = context.Configuration
                       .GetSection(OptionsKeys.ApiPrivatBank).Get<ApiPrivatBankOptions>();

                    ApiRapidOptions optionsRapid = context.Configuration
                        .GetSection(OptionsKeys.ApiRapid).Get<ApiRapidOptions>();

                    TelegramBotOptions optionsBot = context.Configuration
                        .GetSection(OptionsKeys.Telegram).Get<TelegramBotOptions>();

                    CacheManagerOptions optionsCache = context.Configuration
                        .GetSection(OptionsKeys.Cache).Get<CacheManagerOptions>();

                    services.AddSingleton(optionsFactory);
                    services.AddSingleton(optionsPrivatBank);
                    services.AddSingleton(optionsRapid);
                    services.AddSingleton(optionsBot);
                    services.AddSingleton(optionsCache);
                })
                .UseSerilog()
                .Build();

            var tBot = ActivatorUtilities.CreateInstance<TelegramBot>(host.Services);

            try
            {
                tBot.InitBot().Wait();
            }
            catch(AggregateException e)
            {
                Log.Logger.Error($"{e.Message}\nPlease, check telegram token.");
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}

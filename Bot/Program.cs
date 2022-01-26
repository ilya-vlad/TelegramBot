﻿using Serilog;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Bot.Services;
using CacheContext;
using API.Common;
using API.ApiPrivatBank;
using API.Common.Interfaces;
using API.ApiRapid;

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
                    services.AddScoped<CacheManager>();
                    services.AddScoped<ResponseProvider>();
                    services.AddScoped<IJsonParser, JsonParserRapid>();
                    //services.AddScoped<IJsonParser, JsonParserPrivatBank>();
                    services.AddSingleton<TelegramBot>();
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

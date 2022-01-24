using NUnit.Framework;
using Moq;
using System;
using CacheContext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Threading;

namespace UnitTests
{
    public class CacheManagerTests
    {
        private CacheManager _cacheManager;
        private IConfiguration _config;
        private IMemoryCache _memoryCache;
        
        private readonly double _storageTimeInMin = 0.1; //6 sec

        [SetUp]
        public void Setup()
        {
            if (_cacheManager is null)
            {
                var mockLogger = new Mock<ILogger<CacheManager>>();
                ILogger<CacheManager> logger = mockLogger.Object;

                var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) => services.AddMemoryCache())
                    .Build();

                _memoryCache = ActivatorUtilities.CreateInstance<MemoryCache>(host.Services);
                
                var copyAppSettings = new Dictionary<string, string>
                {
                    {"Cache:StorageTimeInMin", _storageTimeInMin.ToString(CultureInfo.InvariantCulture)}
                };

                _config = new ConfigurationBuilder()
                    .AddInMemoryCollection(copyAppSettings)
                    .Build();

                _cacheManager = new CacheManager(logger, _memoryCache, _config);                
            }
        }

        [Test]
        public void TestOfAddToCache()
        {
            DayExchangeRates _testItem = new() { Date = GetRandomDate() };

            _cacheManager.Add(_testItem);

            _memoryCache.TryGetValue(_testItem.Date, out var expectedItem);

            Assert.AreEqual(_testItem, expectedItem);
        }

        [Test]
        public void TestOfGetFromCache()
        {
            DayExchangeRates _testItem = new() { Date = GetRandomDate() };

            _memoryCache.Set(_testItem.Date, _testItem);

            var result = _cacheManager.GetByDate(_testItem.Date);

            Assert.AreEqual(_testItem, result);
        }

        [Test]
        public void TestOfContains()
        {
            DayExchangeRates _testItem = new() { Date = GetRandomDate() };

            _memoryCache.Set(_testItem.Date, _testItem);

            var result = _cacheManager.Contains(_testItem.Date);

            Assert.True(result);
        }

        [Test]
        public void TestOfStorageTimeInCache()
        {
            DayExchangeRates _testItem = new() { Date = GetRandomDate() };

            _cacheManager.Add(_testItem);

            Thread.Sleep(TimeSpan.FromMinutes(_storageTimeInMin) - TimeSpan.FromSeconds(1));

            var expectedTrue = _memoryCache.TryGetValue(_testItem.Date, out _);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            var expectedFalse = _memoryCache.TryGetValue(_testItem.Date, out _);

            Assert.IsTrue(expectedTrue);
            Assert.IsFalse(expectedFalse);
        }


        private DateTime GetRandomDate()
        {
            var r = new Random();            
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(r.Next(range));
        }
    }
}

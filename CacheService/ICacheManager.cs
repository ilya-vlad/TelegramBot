
using API.Common.Models;
using System;

namespace CacheContext
{
    public interface ICacheManager
    {
        void Add(DailyExchangeRates exchangeRates);

        DailyExchangeRates GetByDate(DateTime date);        

        bool Contains(DateTime date);
    }
}

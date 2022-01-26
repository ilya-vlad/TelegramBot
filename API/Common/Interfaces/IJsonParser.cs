using Api.ApiPrivatBank.Models;
using System;


namespace API.Common.Interfaces
{
    public interface IJsonParser
    {
        public DailyExchangeRates GetExchangeRates(DateTime date);
    }
}

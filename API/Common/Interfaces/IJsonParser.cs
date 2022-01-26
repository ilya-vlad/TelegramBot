using Api.ApiPrivatBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface IJsonParser
    {
        public DailyExchangeRates GetExchangeRates(DateTime date);
    }
}

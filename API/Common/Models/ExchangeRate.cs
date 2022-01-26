using API.Common;
using API.Common.Interfaces;

namespace Api.ApiPrivatBank.Models
{
    public class ExchangeRate
    {
        public string CurrencyName { get; set; }

        public double Rate { get; set; }
    }
}

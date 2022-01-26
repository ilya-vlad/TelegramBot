using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.ApiRapid.Models
{
    public class ExchangeRateRapid
    {
        [JsonProperty("currency")]
        public string CurrencyName { get; set; }

        public double Rate { get; set; }
    }
}

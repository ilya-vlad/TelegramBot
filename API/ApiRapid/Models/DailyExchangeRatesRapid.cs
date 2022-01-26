using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.ApiRapid.Models
{
    public class DailyExchangeRatesRapid
    {
        public DateTime Date { get; set; }
        
        public int Timestamp { get; set; }


        [JsonProperty("base")]
        public string BaseCurrency { get; set; }

        public bool Success { get; set; }

        public bool Historical { get; set; }

        public List<ExchangeRateRapid> Rates { get; set; }
    }
}

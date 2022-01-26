using Newtonsoft.Json;


namespace API.ApiRapid.Models
{
    public class ExchangeRateRapid
    {
        [JsonProperty("currency")]
        public string CurrencyName { get; set; }

        public double Rate { get; set; }
    }
}



namespace Api.ApiPrivatBank.Models
{
    public class ExchangeRatePrivateBank
    {
        public string BaseCurrency { get; set; }

        public double PurchaseRateNB { get; set; }        

        public double? SaleRate { get; set; }

        public double? PurchaseRate { get; set; }

        public double SaleRateNB { get ; set; }

        public string Currency { get; set ; }
    }
}

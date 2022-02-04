

namespace UnitTests.ApiPrivatBank
{
    static public class Constants
    {
        public const string ValidJson = "{\"date\":\"04.02.2022\",\"bank\":\"PB\",\"baseCurrency\":980," +
                "\"baseCurrencyLit\":\"UAH\",\"exchangeRate\":" +
                "[{\"baseCurrency\":\"UAH\",\"currency\":\"USD\",\"saleRateNB\":28.2701000," +
                "\"purchaseRateNB\":28.2701000,\"saleRate\":28.4500000,\"purchaseRate\":28.0500000}," +
                "{\"baseCurrency\":\"UAH\",\"currency\":\"UZS\",\"saleRateNB\":0.0026336," +
                "\"purchaseRateNB\":0.0026336}]}";

        public const string BrokenStructureJson = "{\"date\":\"04.02.2022\",\"bank\":\"PB\",";
        public const string EmptyJson = "";

        public const string EmptyValueJson = "{\"date\":\"04.02.2022\",\"bank\":\"PB\"," +
            "\"baseCurrency\":980,\"baseCurrencyLit\":\"UAH\",\"exchangeRate\":" +
            "[{\"baseCurrency\":\"UAH\",\"currency\":\"USD\",\"saleRateNB\":," + //←←←

            "\"purchaseRateNB\":28.2701000,\"saleRate\":28.4500000,\"purchaseRate\":28.0500000}]}";
    }
}
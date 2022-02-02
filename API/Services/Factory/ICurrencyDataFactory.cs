using API.Common.Interfaces;


namespace API.Services.Factory
{
    public interface ICurrencyDataFactory
    {
        public ICurrencyDataProvider GetCurrencyDataProvider();
    }
}

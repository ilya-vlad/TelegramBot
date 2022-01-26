using API.Common.Interfaces;


namespace API.Services.Factory
{
    public interface IJsonParserFactory
    {
        public IJsonParser GetJsonParser();
    }
}

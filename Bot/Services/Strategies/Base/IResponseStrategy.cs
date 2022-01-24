

using System;

namespace Bot.Services.Strategies
{
    public interface IResponseStrategy
    {
        public string GetResponse(string currency, DateTime date);
    }
}

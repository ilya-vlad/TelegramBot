﻿using CacheContext;
using JsonDeserializer;
using System;

namespace Bot.Services.Strategies
{
    public class GreetingStrategy : BaseResponseStrategy<GreetingStrategy>
    {
        public GreetingStrategy(CacheManager cache, JsonParser parser) 
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {            
            return Resources.Messages.Greeting;
        }
    }
}

﻿using API.ApiPrivatBank;
using API.Common;
using API.Common.Interfaces;
using CacheContext;
using System;

namespace Bot.Services.Strategies
{
    public class IncorrectRequestStrategy : BaseResponseStrategy<IncorrectRequestStrategy>
    {
        public IncorrectRequestStrategy(CacheManager cache, IJsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            return Resources.Messages.IncorrectRequest;
        }
    }
}

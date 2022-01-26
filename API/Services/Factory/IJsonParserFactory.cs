using API.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Factory
{
    public interface IJsonParserFactory
    {
        public IJsonParser GetJsonParser();
    }
}

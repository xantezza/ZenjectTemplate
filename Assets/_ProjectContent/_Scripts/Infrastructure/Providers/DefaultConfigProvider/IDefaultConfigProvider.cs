using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Providers.DefaultConfigProvider
{
    public interface IDefaultConfigProvider
    {
        public IDictionary<string, JToken> CachedConfig { get; }
    }
}
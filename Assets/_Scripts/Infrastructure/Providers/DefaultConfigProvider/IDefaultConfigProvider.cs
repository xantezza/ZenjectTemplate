using Newtonsoft.Json.Linq;

namespace Infrastructure.Providers.DefaultConfigProvider
{
    public interface IDefaultConfigProvider
    {
        public JToken CachedConfig { get; }
    }
}
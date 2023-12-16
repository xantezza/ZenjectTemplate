using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders
{
    [NoReflectionBaking]
    public class IdBinder
    {
        BindInfo.BindInfo _bindInfo;

        public IdBinder(BindInfo.BindInfo bindInfo)
        {
            _bindInfo = bindInfo;
        }

        public void WithId(object identifier)
        {
            _bindInfo.Identifier = identifier;
        }
    }
}



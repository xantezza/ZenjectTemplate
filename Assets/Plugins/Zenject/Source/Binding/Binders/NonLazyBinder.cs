using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders
{
    [NoReflectionBaking]
    public class NonLazyBinder : IfNotBoundBinder
    {
        public NonLazyBinder(BindInfo.BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public IfNotBoundBinder NonLazy()
        {
            BindInfo.NonLazy = true;
            return this;
        }

        public IfNotBoundBinder Lazy()
        {
            BindInfo.NonLazy = false;
            return this;
        }
    }
}

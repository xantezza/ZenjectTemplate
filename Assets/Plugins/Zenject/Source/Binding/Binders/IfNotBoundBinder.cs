using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders
{
    [NoReflectionBaking]
    public class IfNotBoundBinder
    {
        public IfNotBoundBinder(BindInfo.BindInfo bindInfo)
        {
            BindInfo = bindInfo;
        }

        // Do not use this
        public BindInfo.BindInfo BindInfo
        {
            get;
            private set;
        }

        public void IfNotBound()
        {
            BindInfo.OnlyBindIfNotBound = true;
        }
    }
}


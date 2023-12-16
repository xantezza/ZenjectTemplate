using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Main;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Finalizers
{
    [NoReflectionBaking]
    public class NullBindingFinalizer : IBindingFinalizer
    {
        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get { return BindingInheritanceMethods.None; }
        }

        public void FinalizeBinding(DiContainer container)
        {
            // Do nothing
        }
    }
}


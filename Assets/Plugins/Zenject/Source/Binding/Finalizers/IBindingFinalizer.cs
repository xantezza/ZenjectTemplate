using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Main;

namespace Plugins.Zenject.Source.Binding.Finalizers
{
    public interface IBindingFinalizer
    {
        BindingInheritanceMethods BindingInheritanceMethod
        {
            get;
        }

        void FinalizeBinding(DiContainer container);
    }
}

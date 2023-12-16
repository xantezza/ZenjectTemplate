using System;
using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Internal;
using Plugins.Zenject.Source.Main;
using Plugins.Zenject.Source.Providers;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Finalizers
{
    [NoReflectionBaking]
    public class SingleProviderBindingFinalizer : ProviderBindingFinalizer
    {
        readonly Func<DiContainer, Type, IProvider> _providerFactory;

        public SingleProviderBindingFinalizer(
            BindInfo.BindInfo bindInfo, Func<DiContainer, Type, IProvider> providerFactory)
            : base(bindInfo)
        {
            _providerFactory = providerFactory;
        }

        protected override void OnFinalizeBinding(DiContainer container)
        {
            if (BindInfo.ToChoice == ToChoices.Self)
            {
                Assert.IsEmpty(BindInfo.ToTypes);

                RegisterProviderPerContract(container, _providerFactory);
            }
            else
            {
                // Empty sometimes when using convention based bindings
                if (!BindInfo.ToTypes.IsEmpty())
                {
                    RegisterProvidersForAllContractsPerConcreteType(
                        container, BindInfo.ToTypes, _providerFactory);
                }
            }
        }
    }
}

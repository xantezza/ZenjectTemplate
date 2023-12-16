using System;
using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Main;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders.Factory.FactoryFromBinder.Untyped
{
    [NoReflectionBaking]
    public class FactoryFromBinderUntyped : FactoryFromBinderBase
    {
        public FactoryFromBinderUntyped(
            DiContainer bindContainer, Type contractType, BindInfo.BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindContainer, contractType, bindInfo, factoryBindInfo)
        {
        }

        // TODO - add similar methods found in FactoryFromBinder<>
    }
}

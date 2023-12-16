using System;
using System.Collections.Generic;
using Plugins.Zenject.Source.Injection;
using Plugins.Zenject.Source.Main;
using Plugins.Zenject.Source.Providers;
using Zenject;

namespace Plugins.Zenject.Source.Binding.BindInfo
{
    [NoReflectionBaking]
    public class FactoryBindInfo
    {
        public FactoryBindInfo(Type factoryType)
        {
            FactoryType = factoryType;
            Arguments = new List<TypeValuePair>();
        }

        public Type FactoryType
        {
            get; private set;
        }

        public Func<DiContainer, IProvider> ProviderFunc
        {
            get; set;
        }

        public List<TypeValuePair> Arguments
        {
            get;
            set;
        }
    }
}

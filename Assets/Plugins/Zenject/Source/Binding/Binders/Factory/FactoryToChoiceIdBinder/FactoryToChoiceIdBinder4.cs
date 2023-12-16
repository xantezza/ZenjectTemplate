using Plugins.Zenject.Source.Binding.Binders.Factory.FactoryArgumentsToChoiceBinder;
using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Main;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders.Factory.FactoryToChoiceIdBinder
{
    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract>
    {
        public FactoryToChoiceIdBinder(
            DiContainer bindContainer, BindInfo.BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
    }
}

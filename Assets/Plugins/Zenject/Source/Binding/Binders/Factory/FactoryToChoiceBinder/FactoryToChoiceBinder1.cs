using Plugins.Zenject.Source.Binding.Binders.Factory.FactoryFromBinder;
using Plugins.Zenject.Source.Binding.BindInfo;
using Plugins.Zenject.Source.Internal;
using Plugins.Zenject.Source.Main;
using Zenject;

namespace Plugins.Zenject.Source.Binding.Binders.Factory.FactoryToChoiceBinder
{
    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TContract> : FactoryFromBinder<TParam1, TContract>
    {
        public FactoryToChoiceBinder(
            DiContainer bindContainer, BindInfo.BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
            : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Note that this is the default, so not necessary to call
        public FactoryFromBinder<TParam1, TContract> ToSelf()
        {
            Assert.IsEqual(BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        public FactoryFromBinder<TParam1, TConcrete> To<TConcrete>()
            where TConcrete : TContract
        {
            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes.Clear();
            BindInfo.ToTypes.Add(typeof(TConcrete));

            return new FactoryFromBinder<TParam1, TConcrete>(BindContainer, BindInfo, FactoryBindInfo);
        }
    }
}

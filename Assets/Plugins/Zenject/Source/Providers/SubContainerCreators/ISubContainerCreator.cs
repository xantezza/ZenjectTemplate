using System;
using System.Collections.Generic;
using Plugins.Zenject.Source.Injection;
using Plugins.Zenject.Source.Main;

namespace Plugins.Zenject.Source.Providers.SubContainerCreators
{
    public interface ISubContainerCreator
    {
        DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context, out Action injectAction);
    }
}

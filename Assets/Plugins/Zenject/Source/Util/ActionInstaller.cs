using System;
using Plugins.Zenject.Source.Install;
using Plugins.Zenject.Source.Main;

namespace Plugins.Zenject.Source.Util
{
    public class ActionInstaller : Installer<ActionInstaller>
    {
        readonly Action<DiContainer> _installMethod;

        public ActionInstaller(Action<DiContainer> installMethod)
        {
            _installMethod = installMethod;
        }

        public override void InstallBindings()
        {
            _installMethod(Container);
        }
    }
}

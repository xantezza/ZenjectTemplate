using Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureFactoriesInstaller : MonoInstaller
    {
        [SerializeField] private ModalPopupFactory modalPopupFactory;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StatesFactory>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<GameLoopStateMachineFactory>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<InitializationStateMachineFactory>().FromNew().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<ModalPopupFactory>().FromInstance(modalPopupFactory).AsSingle().NonLazy();
        }
    }
}
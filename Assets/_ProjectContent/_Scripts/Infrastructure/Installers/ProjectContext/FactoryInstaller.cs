using Infrastructure.Services.Modals;
using Infrastructure.StateMachines;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.InitializationStateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.ProjectContext
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField] private ModalsFactory _modalsFactory;

        public override void InstallBindings()
        {
            Container.Bind<StatesFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<GameLoopStateMachineFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<InitializationStateMachineFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<ModalsFactory>().FromInstance(_modalsFactory).AsSingle().NonLazy();
        }
    }
}
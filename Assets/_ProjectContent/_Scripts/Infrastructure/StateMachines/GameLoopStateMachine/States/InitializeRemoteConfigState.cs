using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configs;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeRemoteConfigState : BaseGameLoopState, IEnterableState
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        public override string StateName => nameof(InitializeRemoteConfigState);

        [Inject]
        public InitializeRemoteConfigState(
            GameLoopStateMachine stateMachine,
            IConditionalLoggingService conditionalLoggingService) : base(stateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeDebugState>();
        }

        public async void Enter()
        {
            await InitializeRemoteSettings();
        }

        private async UniTask InitializeRemoteSettings()
        {
            await FetchDataAsync();
        }

        public Task FetchDataAsync()
        {
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                _conditionalLoggingService.LogError("Retrieval hasn't finished.", LogTag.RemoteSettings);
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            using var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                _conditionalLoggingService.LogError(
                    $"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}",
                    LogTag.RemoteSettings);
                return;
            }

            remoteConfig.ActivateAsync()
                .ContinueWithOnMainThread(
                    _ =>
                    {
                        _conditionalLoggingService.Log(
                            $"Remote data loaded.",
                            LogTag.RemoteSettings);

                        var processedDictionary = FirebaseRemoteConfig.DefaultInstance.AllValues.ToDictionary(
                                keyValuePair => keyValuePair.Key, 
                                keyValuePair => JToken.Parse(keyValuePair.Value.StringValue)
                                ); 
                        
                        Remote.InitializeByRemote(processedDictionary, _conditionalLoggingService);
                        ToNextState();
                    });
        }
    }
}
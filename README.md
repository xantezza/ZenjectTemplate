<span style="font-family:monospace;">

## ```Unity Project Template``` with ready-made functionality and industrial standart technology stack

### ```Architecture on Zenject + Initialization/GameLoop Finite-State Machine, UniRx, UniTasks, Addressables```
- Detailed Conditional Logging
- Analytics, Remote Config
- JSON/Binary Save Services
- Audio, Settings, Window Systems
- Runtime Debugging Tools
- Editor Windows and other utilities to improve QOL
 
### ```3rdParty Plugins```

- Zenject
- UniTask
- UniRx
- LitMotion
- TriInspector
- Graphy - Ultimate Stats Monitor
- IngameDebugConsole
- SerializedCollections

***

 ### You can clone the entire repository and start working directly in it:

Route Unity Services and add JSON Key-Value in `Settings => Services => Dashboard => Remote Config`:
	
`infrastructure_config` with data
```
{
  	"FakeMinimalLoadTime": 0.2,
}
```
You can split production and dev configs usage by adding `dev` environment and uncomment section in `InitializeRemoteConfigState.cs:69`

Use `InfrastructureConfig.cs` as reference for new configs.

To easier configure them use  `Assets/_ProjectContent/Resources/ConfigUtility.asset`

To Disable/Enable Logging and Debug Tools in builds go to `Player Settings => Other Settings => Scripting Define Symbols` and Remove/Add DEV string

 ### Or you can paste only `Assets/` folder inside existing project:

- All above
- Dependencies that must be added to `Packages/manifest.json`
	```
    "com.annulusgames.lit-motion": "https://github.com/AnnulusGames/LitMotion.git?path=src/LitMotion/Assets/LitMotion",
    "com.codewriter.triinspector": "https://github.com/codewriter-packages/Tri-Inspector.git",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
    "com.unity.addressables": "1.21.21",
    "com.unity.remote-config": "4.1.0",
    "com.unity.services.analytics": "5.1.1",
	```
- Go to `Windows/Asset Management/Addressables/Groups/` and press wide middle button `Create Default Group`

- You need to add `Assets/_ProjectContent/Scenes/0_EntryPoint` in `Builds Settings/Scenes In Builds` in with build index 0(!!!)

</span>
    

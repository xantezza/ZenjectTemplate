<span style="font-family:monospace;">

# Unity Project template with ready-made functionality and industrial standart technology stack

## [Demo on itch.io](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

## Architecture on Zenject + GameLoop Finite-State Machine, UniRx, UniTasks, Addressables
- Detailed Conditional Logging
- Unity Analytics, Unity RemoteConfig (with caching)
- JSON/Binary Save Service
- Modal Windows System
- Runtime Debugging tools
- Editor Windows and other utilities to improve QOL
 
### ```Assets/Plugins/```

- Zenject
- UniRx
- Demigiant DOTween
- Graphy - Ultimate Stats Monitor
- IngameDebugConsole
- SerializedCollections

 ### You can clone the entire repository and start working directly in it:

Route Unity Services and add JSON Key-Value in `Settings => Services => Dashboard => Remote Config`:
	
`infrastructure_config` with data
```
{
  	"FakeTimeBeforeLoad": 0.0,
  	"FakeMinimalLoadTime": 0.2,
	"FakeTimeAfterLoad": 0.2
}
```
You can split production and dev configs usage by adding `dev` environment and uncomment section in `InitializeRemoteConfigState.cs:69`

Use `InfrastructureConfig.cs` as reference for new configs.

To easier configure them use  `Assets/_ProjectContent/Resources/ConfigUtility.asset`

To Disable/Enable Logging and Debug Tools in builds go to `Player Settings => Other Settings => Scripting Define Symbols` and Remove/Add DEV string

 ### Or you can paste only `Asstets/` folder inside existing project:

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

## [Demo on itch.io](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

[![image](https://github.com/xantezza/ZenjectTemplate/assets/74206629/0c785e38-cf0b-4760-8b44-83febe659efc)](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

</span>
    

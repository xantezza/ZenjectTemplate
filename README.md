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

 ### You can clone the entire repository and start working directly in it (for troubleshooting you can use the guide below)
 
 ### Or paste only ```Asstets/``` folder inside existing project but then you need to:

- Dependencies that must be added to ```Packages/manifest.json```
	```
    "com.codewriter.triinspector": "https://github.com/codewriter-packages/Tri-Inspector.git",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
    "com.unity.addressables": "1.21.21",
    "com.unity.remote-config": "4.1.0",
    "com.unity.services.analytics": "5.1.1",
	```
- Go to ```Windows/Asset Management/Addressables/Groups/``` 

	- if there no groups press wide middle button ```Create Default Group```, otherwise everything is fine.

- Add DEV Scripting Define to let work Debug functionality in dev builds (you can remove it later for production builds)

- Route Unity Services and add next environments:
	- production (default)
	- dev (used when DEV Scripting Define added)
	
	Then fill both of them with example ```infrastructure_config``` with data
	```
	{
  		"FakeTimeBeforeLoad": 0.0,
  		"FakeMinimalLoadTime": 0.2,
  		"FakeTimeAfterLoad": 0.2
	}
	```

	Use InfrastructureConfig.cs as reference for new configs.
	To easier configure them use  ```Assets/_ProjectContent/Resources/ConfigUtility.asset```

- You need to add ```Assets/_ProjectContent/Scenes/0_EntryPoint``` in ```Builds Settings/Scenes In Builds``` in with build index 0(!!!)

	You can add other scenes to use ```Windows/SceneSelector```

- If you have some unexpected troubles you can contact me https://t.me/xantezza




## [Demo on itch.io](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

[![image](https://github.com/xantezza/ZenjectTemplate/assets/74206629/0c785e38-cf0b-4760-8b44-83febe659efc)](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

</span>
    

<span style="font-family:monospace;">

# Unity Project Template with ready-to-go functionality

### I have personally implemented many GameJam [games](https://xantezza.itch.io/) using this template.

### ```Architecture on Zenject, State Machines, UniRx, UniTasks```

- Analytics, Remote Config
- JSON/Binary Save Services
- Audio, Settings, Window Services
- Detailed Logging
- Runtime Debugging Tools, Cheat Commands
- Editor Windows, Menu Items, Build Helpers and utilities to improve QOL
 
### ```Plugins```
- Zenject
- UniTask
- UniRx
- LitMotion
- TriInspector
- Graphy
- IngameDebugConsole
- SerializedCollections

***

 ### After cloning the repository and opening the project, you need to:

- Route Unity Services and in `Settings => Services => Dashboard => Remote Config` add JSON Key-Value
`infrastructure_config` with data
```
{
  	"FakeMinimalLoadTime": 0.2,
}
```
- Use `InfrastructureConfig.cs` as reference for new configs.
- To easier configure them use  `Assets/_ProjectContent/Resources/ConfigUtility.asset`

</span>
    

Unity project template with implemented functionality necessary for almost any project:

- Ready-To-Grow architecture on Zenject + GameLoopState FSM
- FireBase Analytics + RemoteConfig + Autocaching
- Debugging plug-ins to make QA easier
- Exhaustive logging
- Json/Binary SaveService
- EditorWindows and other utilities to improve QOL
- Addressables
- UniTasks

[DEMO](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

[![image](https://github.com/xantezza/ZenjectTemplate/assets/74206629/b2ac04e7-be44-480a-94e0-8ca7eb11d553)](https://xantezza.itch.io/zenjecttemplate?secret=UttPjfN9suIcAZPYfNQrxg4MsT8)

If adding *.unitypackage to existing project don't forget to add dependencies to <ProjectName>/Packages/manifest.json

    "com.codewriter.triinspector": "https://github.com/codewriter-packages/Tri-Inspector.git",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
    "com.unity.2d.sprite": "1.0.0",
    "com.unity.addressables": "1.21.19",
    "com.unity.ide.rider": "3.0.27",
    "com.unity.textmeshpro": "3.0.6",
    "com.unity.ugui": "2.0.0",

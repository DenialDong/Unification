using System;
using System.IO;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 初始化资源包
/// </summary>
internal class FsmInitialize : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    async void IStateNode.OnEnter()
    {
        Logger.Log("初始化资源包！");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "初始化资源包！").Send();

        await InitPackage();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

    private async UniTask InitPackage()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        var playMode = PatchManager.Instance.PlayMode;

        // 创建资源包
        string packageName = PatchManager.Instance.BundlePackageName;
        var package = YooAssets.TryGetPackage(packageName);
        if (package == null)
        {
            package = YooAssets.CreatePackage(packageName);
            YooAssets.SetDefaultPackage(package);
        }

        // 编辑器下的模拟模式
        InitializationOperation initializationOperation = null;
        if (playMode == EPlayMode.EditorSimulateMode)
        {
            var createParameters = new EditorSimulateModeParameters();
            createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // 单机运行模式
        if (playMode == EPlayMode.OfflinePlayMode)
        {
            var createParameters = new OfflinePlayModeParameters();
            createParameters.DecryptionServices = new GameDecryptionServices();
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // 联机运行模式
        if (playMode == EPlayMode.HostPlayMode)
        {
            string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            var createParameters = new HostPlayModeParameters();
            createParameters.DecryptionServices = new GameDecryptionServices();
            createParameters.QueryServices = new GameQueryServices();
            createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // WebGL运行模式
        if (playMode == EPlayMode.WebPlayMode)
        {
            string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            var createParameters = new WebPlayModeParameters();
            createParameters.QueryServices = new GameQueryServices();
            createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        await initializationOperation;
        if (initializationOperation.Status == EOperationStatus.Succeed)
        {
            _machine.ChangeState<FsmUpdateVersion>();
        }
        else
        {
            Logger.Warning($"{initializationOperation.Error}");
            EventMgr.Get<EmptyEvent>().Set(HEventType.InitializeFailed).Send();
        }
    }

    /// <summary>
    /// 获取资源服务器地址
    /// </summary>
    private string GetHostServerURL()
    {
        string hostServerIP = "http://192.168.110.97:9090"; //模拟器地址
        string appVersion = "v.1.0";
#if UNITY_EDITOR
        {
            return $"file:///{Application.streamingAssetsPath}/yoo/{PatchManager.Instance.BundlePackageName}";
            hostServerIP = "http://127.0.0.1"; //本地测试地址
        }
#endif

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/CDN/WebGL/KopPackage/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{hostServerIP}/CDN/Android/{appVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{hostServerIP}/CDN/WebGL/KopPackage/{appVersion}";
		else
			return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
    }
}
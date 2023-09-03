using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

public class ResMgr:Singleton<ResMgr>
{
    private ResourcePackage _curPackage;

    private ResourcePackage CurPackage
    {
        get
        {
            if (_curPackage == null)
            {
                _curPackage = YooAssets.TryGetPackage(PatchManager.Instance.BundlePackageName);
                if (_curPackage == null)
                {
                    _curPackage = YooAssets.CreatePackage(PatchManager.Instance.BundlePackageName);
                    YooAssets.SetDefaultPackage(_curPackage);
                }
            }

            return _curPackage;
        }
    }

    public async UniTask InitYooAssets(EPlayMode mode)
    {
        // 初始化资源系统
        YooAssets.Initialize();
        // 创建默认的资源包
        _curPackage = YooAssets.CreatePackage("KopPackage");
        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(_curPackage);

        await InitWebGLParameters();
        Logger.Log("inittalize webgl finish");
    }

    private async UniTask InitWebGLParameters()
    {
        var initParameters = new EditorSimulateModeParameters();
        initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("KopPackage");
        await CurPackage.InitializeAsync(initParameters);

        // string defaultHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
        // string fallbackHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
        // var initParameters = new WebPlayModeParameters();
        // initParameters.QueryServices = new GameQueryServices();
        // initParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
        // var initOperation = CurPackage.InitializeAsync(initParameters);
        // await initOperation;

        // if (initOperation.Status == EOperationStatus.Succeed)
        // {
        //     Debug.Log("资源包初始化成功！");
        // }
        // else
        // {
        //     Debug.LogError($"资源包初始化失败：{initOperation.Error}");
        // }
    }


    public async UniTask<T> LoadAssetAsync<T>(string path) where T : Object
    {
        Logger.Log($"res mgr load {path}");
        return await Load<T>(path);
    }

    public AssetOperationHandle LoadAssetAsyncHandle<T>(string path) where T : Object
    {
        var loadPath = GetLoadPath(path);
        AssetOperationHandle handle = CurPackage.LoadAssetAsync<T>(loadPath);
        return handle;
    }

    public AssetOperationHandle LoadAssetAsyncHandle(string path, Type type)
    {
        var loadPath = GetLoadPath(path);
        AssetOperationHandle handle = CurPackage.LoadAssetAsync(loadPath, type);
        return handle;
    }


    private async UniTask<T> Load<T>(string path) where T : UnityEngine.Object
    {
        var loadPath = GetLoadPath(path);
        AssetOperationHandle handle = CurPackage.LoadAssetAsync<T>(loadPath);
        await handle.Task;
        T result = handle.AssetObject as T;
        return result;
    }

    private string GetLoadPath(string path)
    {
        return $"Assets/HotfixResources/{path}";
    }
}
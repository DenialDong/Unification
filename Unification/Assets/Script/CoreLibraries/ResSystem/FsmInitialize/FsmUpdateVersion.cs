using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 更新资源版本号
/// </summary>
internal class FsmUpdateVersion : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    async void IStateNode.OnEnter()
    {
        Logger.Log("获取最新的资源版本 !");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "获取最新的资源版本！").Send();

        await GetStaticVersion();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

    private async UniTask GetStaticVersion()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        var package = YooAssets.GetPackage(PatchManager.Instance.BundlePackageName);
        var operation = package.UpdatePackageVersionAsync();
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            PatchManager.Instance.PackageVersion = operation.PackageVersion;
            Logger.Log($"远端最新版本为: {operation.PackageVersion}");
            _machine.ChangeState<FsmUpdateManifest>();
        }
        else
        {
            Logger.Warning(operation.Error);
            EventMgr.Get<EmptyEvent>().Set(HEventType.PackageVersionUpdateFailed).Send();
        }
    }
}
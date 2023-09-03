using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 更新资源清单
/// </summary>
public class FsmUpdateManifest : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }
    async void IStateNode.OnEnter()
    {
        Logger.Log("更新资源清单！");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "更新资源清单！").Send();

        await UpdateManifest();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    private async UniTask UpdateManifest()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));


        bool savePackageVersion = true;
        var package = YooAssets.GetPackage(PatchManager.Instance.BundlePackageName);
        var operation = package.UpdatePackageManifestAsync(PatchManager.Instance.PackageVersion, savePackageVersion);
        await operation;

        if(operation.Status == EOperationStatus.Succeed)
        {
            _machine.ChangeState<FsmCreateDownloader>();
        }
        else
        {
            Logger.Warning(operation.Error);
            EventMgr.Get<EmptyEvent>().Set(HEventType.PatchManifestUpdateFailed).Send();
        }
    }
}
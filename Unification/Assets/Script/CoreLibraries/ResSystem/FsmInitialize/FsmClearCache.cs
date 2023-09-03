using Cysharp.Threading.Tasks;

/// <summary>
/// 清理未使用的缓存文件
/// </summary>
internal class FsmClearCache : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    async void IStateNode.OnEnter()
    {
        Logger.Log("清理未使用的缓存文件！");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "清理未使用的缓存文件！").Send();
        var package = YooAsset.YooAssets.GetPackage(PatchManager.Instance.BundlePackageName);
        var operation = package.ClearUnusedCacheFilesAsync();
        await operation.ToUniTask();
        _machine.ChangeState<FsmPatchDone>();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

}
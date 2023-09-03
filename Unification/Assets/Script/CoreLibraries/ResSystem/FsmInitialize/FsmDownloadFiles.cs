using System.Collections;
using Cysharp.Threading.Tasks;
using YooAsset;

/// <summary>
/// 下载更新文件
/// </summary>
public class FsmDownloadFiles : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    async void IStateNode.OnEnter()
    {
        Logger.Log("开始下载补丁文件！");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "开始下载补丁文件！").Send();

        await BeginDownload();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

    private async UniTask BeginDownload()
    {
        var downloader = PatchManager.Instance.Downloader;

        // 注册下载回调
        downloader.OnDownloadErrorCallback = DownloadError;
        downloader.OnDownloadProgressCallback = OnDownloadProgress;
        downloader.BeginDownload();
        await downloader;

        // 检测下载结果
        if (downloader.Status != EOperationStatus.Succeed)
        {
            Logger.Log($"下载失败！");
            return;
        }

        _machine.ChangeState<FsmPatchDone>();
    }

    private void DownloadError(string fileName, string error)
    {
        EventMgr.Get<EmptyEvent>().Set(HEventType.WebFileDownloadFailed).Send();
    }

    private void OnDownloadProgress(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes,
        long currentDownloadBytes)
    {
        EventMgr.Get<LongArrayEvent>().Set(HEventType.DownloadProgressUpdate, totalDownloadCount, currentDownloadCount,
            totalDownloadBytes, currentDownloadBytes).Send();
    }
}
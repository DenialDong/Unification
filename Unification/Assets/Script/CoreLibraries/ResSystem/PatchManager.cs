using Cysharp.Threading.Tasks;
using YooAsset;

public class PatchManager : Singleton<PatchManager>
{
    public string BundlePackageName
    {
        get { return "KopPackage"; }
    }

    /// <summary>
    /// 运行模式
    /// </summary>
    public EPlayMode PlayMode { set; get; }

    /// <summary>
    /// 包裹的版本信息
    /// </summary>
    public string PackageVersion { set; get; }

    /// <summary>
    /// 下载器
    /// </summary>
    public ResourceDownloaderOperation Downloader { set; get; }


    private bool _isRun = false;
    private StateMachine _machine;

    /// <summary>
    /// 开启流程
    /// </summary>
    public async UniTask Run(EPlayMode playMode)
    {
        if (_isRun == false)
        {
            _isRun = true;
            PlayMode = playMode;
            RegisterEvents();
            Logger.Log("开启补丁更新流程...");
            _machine = new StateMachine(this);
            _machine.AddNode<FsmPatchPrepare>();
            _machine.AddNode<FsmInitialize>();
            _machine.AddNode<FsmUpdateVersion>();
            _machine.AddNode<FsmUpdateManifest>();
            _machine.AddNode<FsmCreateDownloader>();
            _machine.AddNode<FsmDownloadFiles>();
            _machine.AddNode<FsmDownloadOver>();
            _machine.AddNode<FsmClearCache>();
            _machine.AddNode<FsmPatchDone>();
            _machine.Run<FsmPatchPrepare>();
            
            while (true)
            {
                await UniTask.WaitForEndOfFrame();
                _machine.Update();
                if (_machine.CurrentNode == typeof(FsmPatchDone).FullName)
                {
                    UnRegisterEvents();
                    break;
                }
            }
        }
        else
        {
            Logger.Warning("补丁更新已经正在进行中!");
        }
    }

    private void RegisterEvents()
    {
        EventMgr.Instance.Register(HEventType.UserTryInitialize, EventCallBack);
        EventMgr.Instance.Register(HEventType.UserBeginDownloadWebFiles, EventCallBack);
        EventMgr.Instance.Register(HEventType.UserTryUpdatePackageVersion, EventCallBack);
        EventMgr.Instance.Register(HEventType.UserTryUdpatePatchManifest, EventCallBack);
        EventMgr.Instance.Register(HEventType.UserTryDownloadWebFiles, EventCallBack);
    }

    private void UnRegisterEvents()
    {
        EventMgr.Instance.UnRegister(HEventType.UserTryInitialize, EventCallBack);
        EventMgr.Instance.UnRegister(HEventType.UserBeginDownloadWebFiles, EventCallBack);
        EventMgr.Instance.UnRegister(HEventType.UserTryUpdatePackageVersion, EventCallBack);
        EventMgr.Instance.UnRegister(HEventType.UserTryUdpatePatchManifest, EventCallBack);
        EventMgr.Instance.UnRegister(HEventType.UserTryDownloadWebFiles, EventCallBack);
    }

    private void EventCallBack(HEvent hEvent)
    {
        if (hEvent == null)
            return;

        EmptyEvent emptyEvent = hEvent as EmptyEvent;
        if (emptyEvent == null)
        {
            return;
        }
        if (emptyEvent.EventType == HEventType.UserTryInitialize)
        {
            _machine.ChangeState<FsmInitialize>();
        }
        else if (emptyEvent.EventType == HEventType.UserBeginDownloadWebFiles)
        {
            _machine.ChangeState<FsmDownloadFiles>();
        }
        else if (emptyEvent.EventType == HEventType.UserTryUpdatePackageVersion)
        {
            _machine.ChangeState<FsmUpdateVersion>();
        }
        else if (emptyEvent.EventType == HEventType.UserTryUdpatePatchManifest)
        {
            _machine.ChangeState<FsmUpdateManifest>();
        }
        else if (emptyEvent.EventType == HEventType.UserTryDownloadWebFiles)
        {
            _machine.ChangeState<FsmCreateDownloader>();
        }
    }
}
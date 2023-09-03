using System;
using FairyGUI;
using UnityEngine;

public class CheckUpdatePanel : GComponent
{
    protected virtual string PkgName
    {
        get { return "CheckUpdate"; }
    }

    /// <summary>
    /// 组件名
    /// </summary>
    protected virtual string CompName
    {
        get { return "CheckUpdatePanel"; }
    }

    private GComponent view;

    private GProgressBar _progressBar;
    private GTextField _labTitle;
    private GTextField _patchTips;


    private GComponent messageBox;
    private GButton messageOkBtn;
    private Action okAction;

    public void ShowPanel()
    {
        UIPackage.AddPackage($"UIPackage/{PkgName}", LoadMethod);
        view = UIPackage.CreateObject(PkgName, CompName).asCom;
        GRoot.inst.AddChild(view);

        this.InitElement();
    }

    private void ShowMessageBox(string message, Action okAction)
    {
        this.okAction = okAction;
        if (messageBox == null)
        {
            messageBox = UIPackage.CreateObject(PkgName, "MsgBoxDlg").asCom;
            GRoot.inst.AddChild(messageBox);
            messageOkBtn = messageBox.GetChild("btn_ok").asButton;
            messageBox.onClick.Add(this.OkBtnClick);
        }
    }

    private void CloseMessageBox()
    {
        if (messageBox != null)
        {
            GRoot.inst.RemoveChild(messageBox);
            messageBox.Dispose();
            messageBox = null;
        }
    }

    private void OkBtnClick()
    {
        CloseMessageBox();
        this.okAction?.Invoke();
    }


    public void Destroy()
    {
        if (this.view != null)
        {
            GRoot.inst.RemoveChild(this.view);
            this.view.Dispose();
            this.view = null;
        }
    }

    private object LoadMethod(string name, string extension, System.Type type, out DestroyMethod destroyMethod)
    {
        destroyMethod = DestroyMethod.Unload;
        return Resources.Load(name, type);
    }


    private void InitElement()
    {
        Logger.Log($"update panel init element");
        _progressBar = view.GetChild("bar").asProgress;
        _labTitle = view.GetChild("lab_Title").asTextField;
        _patchTips = view.GetChild("tipsLabel").asTextField;


        EventMgr.Instance.Register(HEventType.InitializeFailed, UpdateEvent);
        EventMgr.Instance.Register(HEventType.PatchStatesChange, UpdateEvent);
        EventMgr.Instance.Register(HEventType.FoundUpdateFiles, UpdateEvent);
        EventMgr.Instance.Register(HEventType.DownloadProgressUpdate, UpdateEvent);
        EventMgr.Instance.Register(HEventType.PackageVersionUpdateFailed, UpdateEvent);
        EventMgr.Instance.Register(HEventType.PatchManifestUpdateFailed, UpdateEvent);
        EventMgr.Instance.Register(HEventType.WebFileDownloadFailed, UpdateEvent);
        EventMgr.Instance.Register(HEventType.BeginGame, UpdateEvent);
    }

    private void UpdateProgress(HEvent hEvent)
    {
        var evt = hEvent as IntArrayEvent;
        var cur = evt.Value[0];
        var max = evt.Value[1];
        this._progressBar.value = (float)cur / max;
    }


    private void UpdateEvent(HEvent hEvent)
    {
        if (hEvent.EventType == HEventType.InitializeFailed)
        {
            Action callBack = () => { EventMgr.Get<EmptyEvent>().Set(HEventType.UserTryInitialize).Send(); };
            ShowMessageBox($"Failed to initialize package!", callBack);
        }
        else if (hEvent.EventType == HEventType.PatchStatesChange)
        {
            var evt = hEvent as StringEvent;
            _patchTips.text = evt.Value;
        }
        else if (hEvent.EventType == HEventType.FoundUpdateFiles)
        {
            Action callBack = () => { EventMgr.Get<EmptyEvent>().Set(HEventType.UserBeginDownloadWebFiles).Send(); };
            LongArrayEvent msg = hEvent as LongArrayEvent;
            float sizeMB = msg.Value[1] / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            ShowMessageBox($"Found upate patch files,Total count {msg.Value[0]} Total size {totalSizeMB}MB", callBack);
        }
        else if (hEvent.EventType == HEventType.DownloadProgressUpdate)
        {
            var msg = hEvent as LongArrayEvent;
            var totalDownloadCount = msg.Value[0];
            var currentDownloadCount = msg.Value[1];
            var totalDownloadSize = msg.Value[2];
            var currentDownloadSize = msg.Value[3];

            _progressBar.value = (float)currentDownloadCount / totalDownloadCount;

            string currentSize = (currentDownloadSize / 1048576).ToString("f1");
            string totalSize = (totalDownloadSize / 1048576).ToString("f1");
            _patchTips.text = $"{currentDownloadCount}/{totalDownloadCount}   {currentSize}MB/{totalSize}MB";
        }
        else if (hEvent.EventType == HEventType.PackageVersionUpdateFailed)
        {
            Action callBack = () => { EventMgr.Get<EmptyEvent>().Set(HEventType.UserTryUpdatePackageVersion).Send(); };

            ShowMessageBox($"Failed to update static version,please check the network status.", callBack);
        }
        else if (hEvent.EventType == HEventType.PatchManifestUpdateFailed)
        {
            Action callBack = () => { EventMgr.Get<EmptyEvent>().Set(HEventType.UserTryUdpatePatchManifest).Send(); };
            ShowMessageBox($"Failed to update patch manifest,please check the network status.", callBack);
        }
        else if (hEvent.EventType == HEventType.WebFileDownloadFailed)
        {
            StringEvent msg = hEvent as StringEvent;
            Action callBack = () => { EventMgr.Get<EmptyEvent>().Set(HEventType.UserTryDownloadWebFiles).Send(); };
            ShowMessageBox($"Failed to download file : {msg.Value}", callBack);
        }
        else if (hEvent.EventType == HEventType.BeginGame)
        {
            this.Destroy();
        }
    }
}
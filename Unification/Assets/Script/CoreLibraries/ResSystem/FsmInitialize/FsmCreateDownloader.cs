﻿using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 创建文件下载器
/// </summary>
public class FsmCreateDownloader : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    async void IStateNode.OnEnter()
    {
        Logger.Log("创建补丁下载器！");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "创建补丁下载器！").Send();
        await CreateDownloader();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

    async UniTask CreateDownloader()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));


        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
        PatchManager.Instance.Downloader = downloader;

        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("Not found any download files !");
            _machine.ChangeState<FsmDownloadOver>();
        }
        else
        {
            //A total of 10 files were found that need to be downloaded
            Debug.Log($"Found total {downloader.TotalDownloadCount} files that need download ！");

            // 发现新更新文件后，挂起流程系统
            // 注意：开发者需要在下载前检测磁盘空间不足
            
            int totalDownloadCount = downloader.TotalDownloadCount;
            long totalDownloadBytes = downloader.TotalDownloadBytes;
            EventMgr.Get<LongArrayEvent>().Set(HEventType.NotifyUpdateFiles, totalDownloadCount, totalDownloadBytes);
        }
    }
}
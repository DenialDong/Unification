public enum HEventType
{
    //更新相关事件
    UserTryInitialize,
    UserBeginDownloadWebFiles,
    UserTryUpdatePackageVersion,
    UserTryUdpatePatchManifest,
    UserTryDownloadWebFiles,
    
    InitializeFailed,
    PackageVersionUpdateFailed,
    PatchManifestUpdateFailed,
    WebFileDownloadFailed,
    
    //更新进度
    DownloadProgressUpdate,
    NotifyUpdateFiles,
    FoundUpdateFiles,
    PatchStatesChange,
    //开始游戏
    BeginGame,
    
}
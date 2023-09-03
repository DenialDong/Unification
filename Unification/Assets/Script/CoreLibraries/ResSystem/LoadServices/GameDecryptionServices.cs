

using System;
using System.IO;
using YooAsset;

/// <summary>
/// 资源文件解密服务类
/// </summary>
public class GameDecryptionServices : IDecryptionServices
{
    public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
    {
        return 32;
    }

    public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
    {
        throw new NotImplementedException();
    }

    public Stream LoadFromStream(DecryptFileInfo fileInfo)
    {
        BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return bundleStream;
    }

    public uint GetManagedReadBufferSize()
    {
        return 1024;
    }
}
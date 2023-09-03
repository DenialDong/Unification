using Cysharp.Threading.Tasks;

public class SubUIMenuMgr : BaseSubLayerMgr
{
    public async UniTask<AsyncWindow> ShowMenu(string menuName, params object[] param)
    {
        var asyncWindow = await this.ShowUIAsync(menuName, SceneLayerType.Menu, param);
        return asyncWindow;
    }
}
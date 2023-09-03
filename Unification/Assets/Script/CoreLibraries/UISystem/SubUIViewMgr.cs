using Cysharp.Threading.Tasks;

public class SubUIViewMgr : BaseSubLayerMgr
{
    
    /// <summary>
    /// 当前显示的view
    /// </summary>
    private AsyncWindow _currentView;

    /// <summary>
    /// 切换显示view  只能同时存在一个
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async UniTask<AsyncWindow> ChangeView(string viewName, params object[] param)
    {
        var newWindow = await this.ShowUIAsync(viewName, SceneLayerType.View, param);
        if (newWindow == null)
        {   
            Logger.Error($"Change view to create a null window  :{viewName}");
            return null;
        }

        int viewIndex = 1;
        if (_currentView != null)
        {
            _currentView.Close();
        }

        _currentView = newWindow;

        return _currentView;
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FairyGUI;

public class BaseSubLayerMgr
{
    private readonly Dictionary<string, AsyncWindow> _classMap = new();

    public virtual void Dispose()
    {
        ReleaseAllLayer();
    }

    protected void ReleaseAllLayer()
    {
        foreach (KeyValuePair<string, AsyncWindow> kv in _classMap)
        {
            kv.Value.Destory();
        }
    }

    #region 关闭view  menu window 方法

    public async UniTask CloseWindowAsync(AsyncWindow window)
    {
        if (!window.onStage) return;
        window.Hide();
        window.Destory();
        Logger.Log($"close window and remove from class map :{window.GetType().Name}");
        var name = window.GetType().Name;
        _classMap.Remove(name);
    }

    public async UniTask CloseWindowAsync(string window)
    {
        if (!_classMap.TryGetValue(window, out var asyncWindow))
        {
            return;
        }

        CloseWindowAsync(asyncWindow).Forget();
    }

    #endregion


    #region 显示面板基础方法 私有的

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="param"></param>
    protected async UniTask<AsyncWindow> ShowUIAsync(string windowName, SceneLayerType layerType, params object[] param)
    {
        if (!_classMap.TryGetValue(windowName, out var asyncWindow))
        {
            asyncWindow = await CreateUIAsync(windowName);
            if (asyncWindow == null)
            {
                Logger.Error($"Create window async failed!");
                return null;
            }

            asyncWindow.LayerType = layerType;
            _classMap.Add(windowName, asyncWindow);
        }

        asyncWindow.Options = param.ToList();

        var _popupLayer = GetLayer(asyncWindow.LayerType);
        if (asyncWindow.onStage)
        {
            asyncWindow.SetParent(_popupLayer);
            asyncWindow.OnRefresh();
            asyncWindow.OnAnimation();

            BringToFront(asyncWindow, _popupLayer);
            asyncWindow.StateShown?.TrySetResult(UIState.Shown);
            return asyncWindow;
        }

        asyncWindow.ShowModal();
        asyncWindow.SetParent(_popupLayer);
        asyncWindow.OnRefresh();
        asyncWindow.OnAnimation();

        var curState = await asyncWindow.StateShown.Task;
        Logger.Log($"show Window async state {curState.ToString()}");
        return asyncWindow;
    }

    protected async UniTask<AsyncWindow> CreateUIAsync(string windowName)
    {
        var asyncWindow = UIManager.Instance.CreateClassByName<AsyncWindow>(windowName);
        if (asyncWindow == null)
        {
            return null;
        }

        var state = await asyncWindow.StateLoaded.Task;
        if (state != UIState.Loaded)
        {
            return null;
        }

        asyncWindow.Bind();
        return asyncWindow;
    }


    /// <summary>
    /// 面板放置到前面
    /// </summary>
    /// <param name="win"></param>
    /// <param name="layerRoot"></param>
    protected void BringToFront(Window win, GComponent layerRoot)
    {
        var cnt = layerRoot.numChildren;
        var i = cnt - 1;
        for (; i >= 0; i--)
        {
            var g = layerRoot.GetChildAt(i);
            if (g == win)
                return;
            if (g is Window)
                break;
        }

        if (i >= 0)
            layerRoot.SetChildIndex(win, i);
    }

    /// <summary>
    /// 获取面包放置的层级节点
    /// </summary>
    /// <param name="uiLayer"></param>
    /// <returns></returns>
    protected GComponent GetLayer(SceneLayerType uiLayer)
    {
        return UISceneMgr.Instance.curScene.GetLayerByType(uiLayer);
    }

    #endregion
}
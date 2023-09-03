using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{

    #region 显示view

    /// <summary>
    /// 切换显示view  只能同时存在一个
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async UniTask<AsyncWindow> ChangeView(string viewName, params object[] param)
    {
        var subMgr = UISceneMgr.Instance.curScene.subUIViewMgr;
        return await subMgr.ChangeView(viewName, param);
    }

    #endregion

    #region 显示Menu

    public async UniTask<AsyncWindow> ShowMenu(string menuName, params object[] param)
    {
        var subMgr = UISceneMgr.Instance.curScene.SubUIMenuMgr;
        var asyncWindow = await subMgr.ShowMenu(menuName, param);
        return asyncWindow;
    }

    #endregion

    #region 显示window

    public async UniTask<AsyncWindow> ShowWindow(string pageName, params object[] param)
    {
        var subMgr = UISceneMgr.Instance.curScene.SubUIWindowMgr;
        var asyncWindow = await subMgr.ShowWindow(pageName, param);
        return asyncWindow;
    }

    public async UniTask<AsyncWindow> PushWindow(string pageName, params object[] param)
    {
        var subMgr = UISceneMgr.Instance.curScene.SubUIWindowMgr;
        var asyncWindow = await subMgr.PushWindow(pageName, ChangePageType.Push, param);
        return asyncWindow;
    }

    public async UniTask<AsyncWindow> ChangeWindow(string pageName, params object[] param)
    {
        var subMgr = UISceneMgr.Instance.curScene.SubUIWindowMgr;
        var asyncWindow = await subMgr.ChangeWindow(pageName, ChangePageType.Change, param);
        return asyncWindow;
    }

    public async UniTask<AsyncWindow> PopWindow()
    {
        var subMgr = UISceneMgr.Instance.curScene.SubUIWindowMgr;
        return await subMgr.PopWindow();
    }

    #endregion

    #region 显示飘字，飘奖励之列

    public async UniTask ShowMsg(string msg)
    {
        var msgParent = UILayerMgr.Instance.GetLayerByType(UILayer.Msg);
        //TODO
        Logger.Log($"显示飘字 {msg}. 逻辑待实现");
    }

    #endregion

    #region 显示引导相关面板逻辑

    public async UniTask ShowGuide(int guideId)
    {
        var msgParent = UILayerMgr.Instance.GetLayerByType(UILayer.Msg);
        //TODO
        Logger.Log($"显示引导ID {guideId}. 逻辑待实现");
    }

    #endregion
    
    #region 关闭view  menu window 方法

    public async UniTask CloseWindowAsync(AsyncWindow window)
    {
        var subMgr = this.GetSubUIMgr(window.LayerType);
        subMgr.CloseWindowAsync(window).Forget();
    }

    public async UniTask CloseWindowAsync(string window, SceneLayerType type)
    {
        var subMgr = this.GetSubUIMgr(type);
        subMgr.CloseWindowAsync(window).Forget();
    }

    #endregion

    public BaseSubLayerMgr GetSubUIMgr(SceneLayerType type)
    {
        switch (type)
        {
            case SceneLayerType.View:
                return UISceneMgr.Instance.curScene.subUIViewMgr;
            case SceneLayerType.Menu:
                return UISceneMgr.Instance.curScene.SubUIMenuMgr;
            case SceneLayerType.Window:
                return UISceneMgr.Instance.curScene.SubUIWindowMgr;
            default:
                Logger.Error($"get sub ui mgr is null : {type.ToString()}");
                return null;
        }
    }

    /// <summary>
    /// 根据类名实例化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">类名（可包含命名空间，例如G.ClassName）</param>
    /// <param name="param"> 构造函数参数</param>
    /// <returns></returns>
    public T CreateClassByName<T>(string className, object[] param = null)
    {
        Type t = Type.GetType(className);
        if (t == null)
        {
            Logger.Error($"window type {className} no found!");
            return default;
        }

        T instance = (T)Activator.CreateInstance(t, param);
        return instance;
    }
}
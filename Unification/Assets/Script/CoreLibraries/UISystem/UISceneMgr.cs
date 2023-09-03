using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;


public class UISceneMgr : Singleton<UISceneMgr>
{
    private Stack<BaseUIScene> _popArr;
    public BaseUIScene curScene;
    public string curSceneName;


    protected override void OnInit()
    {
        _popArr = new Stack<BaseUIScene>();
        InitModule();
    }


    public void Run(string sceneName, object data = null)
    {
        ShowScene(sceneName, data);
    }

    public void Push(string sceneName, object data = null)
    {
        ShowScene(sceneName, data, true);
    }

    private async void ShowScene(string sceneName, object data = null, bool toPush = false)
    {
        if (curScene != null && curScene.ClassName == sceneName) return;
        UISceneModuleInfo moduleInfo = GetModuleInfo(sceneName);
        if (moduleInfo == null)
        {
            return;
        }

        curSceneName = sceneName;
        if (moduleInfo.preResList != null)
        {
            // //TODO
            // foreach (var item in moduleInfo.preResList)
            // {
            //     await FGUIPackageUtitlits.AddPackageAsync(item);
            // }

            OnSceneLoaded(moduleInfo, data, toPush);
            //ResMgr.inst.load(moduleInfo.preResList, this.OnUILoaded.bind(this, moduleInfo, data, toPush));
        }
        else
        {
            OnSceneLoaded(moduleInfo, data, toPush);
        }
    }

    private void OnSceneLoaded(UISceneModuleInfo moduleInfo, object data, bool toPush)
    {
        if (toPush && curScene != null)
        {
            _popArr.Push(curScene);
            curScene.RemoveSelf();
        }
        else
        {
            CheckDestroyLastScene(!toPush);
        }

        curScene = UIManager.Instance.CreateClassByName<BaseUIScene>(moduleInfo.name);
        curScene.gameObjectName = moduleInfo.name;
        
        // Vector2 size = UIManager.Instance.SetFitSize(curScene);
        // Logger.Log($"groot width:{GRoot.inst.width}   h:{GRoot.inst.height}");
        // curScene.SetXY((GRoot.inst.width - size.x) / 2, (GRoot.inst.height - size.y) / 2);

        var parent = UILayerMgr.Instance.GetLayerByType(UILayer.UIScene);
        curScene.SetParent(parent);
        if (data != null)
        {
            curScene.SetData(data);
        }
    }

    private void CheckDestroyLastScene(bool destroy = false)
    {
        if (curScene != null)
        {
            UISceneModuleInfo lastModuleInfo = this.GetModuleInfo(curScene.gameObjectName);
            if (destroy)
            {
                //销毁上一个场景
                curScene.Close();
                if (!lastModuleInfo.cacheEnabled && lastModuleInfo.preResList != null)
                {
                    //TODO
                    foreach (var item in lastModuleInfo.preResList)
                    {
                        UIPackage.RemovePackage(item);
                    }
                    //ResMgr.Instance.releaseResModule(this.curScene.className);//释放资源
                }
            }
        }
    }

    /// <summary>
    /// 返回上一个场景
    /// </summary>
    public void PopScene()
    {
        if (_popArr.Count <= 0)
        {
            Logger.Error("场景已经pop到底了....");
            return;
        }

        CheckDestroyLastScene(true);
        curScene = _popArr.Pop();
        curSceneName = curScene.gameObjectName;
        curScene.AddSelfToOldParent();
    }


    #region 场景模块数据

    public Dictionary<string, UISceneModuleInfo> moduleDic;

    /// <summary>
    /// 初始化所有模块
    /// </summary>
    private void InitModule()
    {
        moduleDic = new Dictionary<string, UISceneModuleInfo>();
        moduleDic["LoginScene"] = new UISceneModuleInfo("LoginScene", false, new List<string>() { });
        moduleDic["LoadingScene"] = new UISceneModuleInfo("LoadingScene", false, new List<string>() { });
        moduleDic["HomeScene"] = new UISceneModuleInfo("HomeScene", true, new List<string>() { });
        // moduleDic["MapEditorScene"] = new UISceneModuleInfo("MapEditorScene", false, new List<string>() { });
        // moduleDic["RoleScene"] = new UISceneModuleInfo("RoleScene", false, new List<string>() { });
        moduleDic["LoopListScene"] = new UISceneModuleInfo("LoopListScene", false, new List<string>() { });
    }

    /// <summary>
    /// 获取模块信息
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    public UISceneModuleInfo GetModuleInfo(string moduleName)
    {
        moduleDic.TryGetValue(moduleName, out UISceneModuleInfo info);
        return info;
    }

    #endregion
}
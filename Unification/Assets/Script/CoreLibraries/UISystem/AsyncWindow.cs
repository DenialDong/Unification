using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;
// using YooAsset;

public abstract class  AsyncWindow : Window
{
    //
    public UniTaskCompletionSource<UIState> StateLoaded { get; set; }
    public UniTaskCompletionSource<UIState> StateInit { get; set; }
    public UniTaskCompletionSource<UIState> StateShown { get; set; }
    public UniTaskCompletionSource<UIState> StateHide { get; set; }

    // 窗口所在的层, 默认为UILayer.Window
    public SceneLayerType LayerType { get; set; }

    public virtual bool NeedAnimation
    {
        get { return false; }
    }

    /// <summary>
    /// 包名
    /// </summary>
    protected virtual string PkgName
    {
        get { return ""; }
    }

    /// <summary>
    /// 组件名
    /// </summary>
    protected virtual string CompName
    {
        get { return "Root"; }
    }


    // Show传递的参数, 每次调用OnShow之后清空
    public List<object> Options { get; set; } = new();

    //面板Gcomponent
    public GComponent View { get; set; }
    public GComponent _oldParent { get; set; }

    public bool hasDestory = false;

    public bool isBinded = false;


    public AsyncWindow()
    {
        Ctor();
        StateLoaded = new();
        StateInit = new();
        StateShown = new();
        StateHide = new();
        CreateWindowAsync().Forget();
    }

    private async UniTask CreateWindowAsync()
    {
        // await AddWindowPackageAsync(PkgName);

        View = UIPackage.CreateObject(PkgName, CompName).asCom;
        this.name = $"{PkgName}_Window";
        this.gameObjectName = $"{PkgName}_Window";
        this.View.name = $"{PkgName}_Comp";
        this.View.gameObjectName = $"{PkgName}_Comp";
        this.AddChild(this.View);
        Logger.Log($"window {PkgName} is loaded");
        StateLoaded?.TrySetResult(UIState.Loaded);
    }

    protected virtual void Ctor()
    {
    }

    public abstract void InitProperty(GObject go);
    protected abstract void InitElement();

    // 绑定事件, 默认在OnInit中调用
    public abstract void Bind();

    // 解绑事件, 默认在OnHide中调用
    protected abstract void UnBind();

    public abstract void OnRefresh();

    protected override void OnInit()
    {
        InitProperty(this.View);
        InitElement();
        Bind();

        Logger.Log($"window {PkgName} is inited");
        StateInit?.TrySetResult(UIState.Inited);
    }

    protected override void OnShown()
    {
        if (!NeedAnimation)
        {
            Logger.Log($"window {PkgName} is shown");
            StateShown?.TrySetResult(UIState.Shown);
        }
    }

    protected override void OnHide()
    {
        Logger.Log($"window {PkgName} is hide");
        StateHide?.TrySetResult(UIState.Hide);
    }

    public void ShowModal(float alpha = 0.4f)
    {
        if (this.LayerType == SceneLayerType.Window)
        {
            GGraph bgMask = new GGraph();
            Color modalLayerColor = new Color(0, 0, 0, alpha);
            bgMask.DrawRect(GRoot.inst.width, GRoot.inst.height, 1, modalLayerColor, modalLayerColor);
            bgMask.onClick.Add(() => { Close(); });

            AddChildAt(bgMask, 0);
            bgMask.pivotX = bgMask.pivotY = 0.5f;
            bgMask.SetXY(0, 0);
        }
    }

    public virtual void OnAnimation()
    {
        if (NeedAnimation)
        {
            View.SetPivot(0.5f, 0.5f);
            View.TweenScale(new Vector2(1.1f, 1.1f), 0.15f).OnComplete(() =>
            {
                View.TweenScale(new Vector2(1f, 1f), 0.15f).OnComplete(() =>
                {
                    Logger.Log($"play open ui anim {PkgName}   uiState showAnim");
                    StateShown?.TrySetResult(UIState.Shown);
                });
            });
        }
    }

    public void Close(bool checkPop = true)
    {
        UnBind();
        UIManager.Instance.CloseWindowAsync(this);
        if (checkPop && this.LayerType == SceneLayerType.Window)
        {
            UIManager.Instance.PopWindow();
        }
    }

    public void Destory()
    {
        if (hasDestory) return;
        hasDestory = true;

        Logger.Log($"onDestroy: {PkgName}");
        RemoveSelf();
        View.Dispose();
        Dispose();
        // ReleaseHandles();
    }

    public void RemoveSelf()
    {
        RemoveFromParent();
    }

    private void RemoveFromParent()
    {
        if (parent != null)
            parent.RemoveChild(this);
    }

    /// <summary>
    /// 设置view的父级
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(GComponent parent)
    {
        _oldParent = parent;
        parent.AddChild(this);
    }

    //填加到就父级（用于界面回退管理，开发者请勿自己掉用）
    public void AddSelfToOldParent()
    {
        SetParent(_oldParent);
    }

    public void Visible(bool enter)
    {
        this.visible = enter;
    }

/*
 *    #region 资源相关
   
       // 资源句柄列表
       private List<AssetOperationHandle> _handles = new List<AssetOperationHandle>(100);
   
       private async UniTask AddWindowPackageAsync(string pkgName)
       {
           if (string.IsNullOrEmpty(PkgName))
           {
               Logger.Error("The package name is NULL!");
               return;
           }
   
           await AddPackageAsync($"UIPackage/{pkgName}");
           var mainPackage = UIPackage.GetByName(pkgName);
           var dependences = mainPackage.dependencies;
           foreach (var item in dependences)
           {
               await AddPackageAsync($"UIPackage/{item["name"]}");
           }
       }
   
       public void ReleaseHandles()
       {
           foreach (var handle in _handles)
           {
               handle.Release();
           }
   
           _handles.Clear();
       }
   
       public async UniTask AddPackageAsync(string pkgName)
       {
           var handle = ResMgr.Instance.LoadAssetAsyncHandle<TextAsset>($"{pkgName}_fui");
           _handles.Add(handle);
           //加载描述文件
           await handle;
           TextAsset desTextAsset = handle.AssetObject as TextAsset;
           //加载UI图集
           UIPackage.AddPackage(desTextAsset.bytes, pkgName, LoadPackageInternalAsync);
       }
   
   
       private async void LoadPackageInternalAsync(string name, string extension, System.Type type,
           PackageItem item)
       {
           var texHandle = ResMgr.Instance.LoadAssetAsyncHandle<Texture>($"{name}");
           _handles.Add(texHandle);
           await texHandle;
           Texture tex = texHandle.AssetObject as Texture;
           item.owner.SetItemAsset(item, tex, DestroyMethod.Unload);
       }
   
       #endregion
 */
 
}
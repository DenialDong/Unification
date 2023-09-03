using System;
using Cysharp.Threading.Tasks;
using FairyGUI;


public class BaseUIScene : GComponent
{
    public GComponent layer;
    public GComponent menu;
    public GComponent page;
    public GComponent scriptLayer;

    public UniTaskCompletionSource<SceneState> SceneLoaded;
    public UniTaskCompletionSource<SceneState> SceneShow;
    public UniTaskCompletionSource<SceneState> SceneHide;


    protected string scriptClassLayer;
    private bool _isFirstEnter = true;
    protected object _moduleParam;
    public string mainClassView;

    public SubUIViewMgr subUIViewMgr;
    public SubUIWindowMgr SubUIWindowMgr;
    public SubUIMenuMgr SubUIMenuMgr;


    public GComponent _oldParent { get; set; }

    public BaseUIScene()
    {
        SceneLoaded = new();
        SceneShow = new();
        SceneHide = new();
        subUIViewMgr = new();
        SubUIWindowMgr = new();
        SubUIMenuMgr = new();

        Ctor();
        onAddedToStage.Add(Init);
    }

    protected virtual void Ctor()
    {
    }

    protected virtual void OnEnter()
    {
    }

    protected virtual void OnFirstEnter()
    {
    }

    protected virtual void OnExit()
    {
    }


    private void Init()
    {
        onAddedToStage.Remove(Init);
        SceneLoaded?.TrySetResult(SceneState.Loaded);
        InitLayer();
        if (!string.IsNullOrEmpty(mainClassView))
        {
            UIManager.Instance.ChangeView(mainClassView);
        }
    }

    private void InitLayer()
    {
        layer = AddGCom2GRoot($"UI_{SceneLayerType.View.ToString()}");
        menu = AddGCom2GRoot($"UI_{SceneLayerType.Menu.ToString()}");
        page = AddGCom2GRoot($"UI_{SceneLayerType.Window.ToString()}");

        scriptLayer = AddGCom2GRoot($"UI_ScriptLayer");

        _doEnter();
    }

    public GComponent GetLayerByType(SceneLayerType type)
    {
        switch (type)
        {
            case SceneLayerType.View:
                return layer;
            case SceneLayerType.Menu:
                return menu;
            case SceneLayerType.Window:
                return page;
            default:
                return page;
        }
    }

    private GComponent AddGCom2GRoot(string name)
    {
        GComponent newNode = new();
        newNode.gameObjectName = name;
        newNode.name = name;
        UISceneMgr.Instance.curScene.AddChild(newNode);
        newNode.pivotX = newNode.pivotY = 0.5f;
        newNode.SetXY(0,0);
        // UIManager.Instance.SetFitSize(newNode);
        return newNode;
    }

    private void _doEnter()
    {
        Logger.Log("进入 " + ClassName);
        OnEnter();
        if (_isFirstEnter)
        {
            _isFirstEnter = false;
            OnFirstEnter();
        }

        SceneShow?.TrySetResult(SceneState.Show);
    }

    public void SetData(Object data)
    {
        _moduleParam = data;
    }

    public string ClassName
    {
        get { return gameObjectName; }
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

    /**重置到主界面（会清掉当前堆栈中的所有界面） */
    /**显示指定界面（替换模式） */
    /**显示指定界面（入栈模式） */
    /**layer出栈 */
    /// <summary>
    /// 将场景添加到GRoot根节点（用于界面回退管理，开发者请勿调用）**/
    /// </summary>
    public void AddSelfToOldParent()
    {
        // foreach (var item in GetChildren())
        // {
        //     EachChildByParent((GComponent)item, true);
        // }

        _doEnter();
        SetParent(_oldParent);
    }

    /// <summary>
    /// 从父级移除（用于界面回退管理，开发者请勿调用）**/
    /// </summary>
    public void RemoveSelf()
    {
        foreach (var item in GetChildren())
        {
            EachChildByParent((GComponent)item);
        }

        _dispose();
        UISceneMgr.Instance.curScene.RemoveFromParent();
    }

    private void EachChildByParent(GComponent _parent, bool isEnter = false)
    {
        foreach (var item in _parent.GetChildren())
        {
            // (item as AsyncWindow).Visible(isEnter);
            // if (isEnter)
            // {
            //     (item as AsyncWindow).Show();
            // }
            // else
            // {
            //     (item as AsyncWindow).Hide();
            // }
        }
    }

    private void _dispose()
    {
        Logger.Log("退出" + ClassName);
        OnExit();
        SceneHide?.TrySetResult(SceneState.Hide);
    }

    private void Destroy()
    {
        subUIViewMgr.Dispose();
        subUIViewMgr = null;
        SubUIWindowMgr.Dispose();
        SubUIWindowMgr = null;
        SubUIMenuMgr.Dispose();
        SubUIMenuMgr = null;
        Logger.Log("onDestroy " + ClassName);
        Dispose();
    }

    public void Close()
    {
        _dispose();
        Destroy();
    }
}
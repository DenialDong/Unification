using FairyGUI;


public enum UILayer
{
    BaseScene,
    UIScene,
    Msg, //提示信息，飘字，
    Guide, //引导
    Toast, //
}

public class UILayerMgr : Singleton<UILayerMgr>
{
    private GComponent _baseScene;
    private GComponent _uiScene;
    private GComponent _msgLayer;
    private GComponent _guideLayer;
    private GComponent _toastLayer;

    protected override void OnInit()
    {
        GRoot.inst.SetContentScaleFactor(640, 1280, UIContentScaler.ScreenMatchMode.MatchHeight);
        InitLayer();

        UIManager.Instance.Init();
        UISceneMgr.Instance.Init();
    }   

    private void InitLayer()
    {
        _baseScene = AddGCom2GRoot($"UI_{UILayer.BaseScene.ToString()}");
        _uiScene = AddGCom2GRoot($"UI_{UILayer.UIScene.ToString()}");
        _msgLayer = AddGCom2GRoot($"UI_{UILayer.Msg.ToString()}");
        _guideLayer = AddGCom2GRoot($"UI_{UILayer.Guide.ToString()}");
        _toastLayer = AddGCom2GRoot($"UI_{UILayer.Toast.ToString()}");
    }

    private GComponent AddGCom2GRoot(string name)
    {
        GComponent newNode = new GComponent();
        newNode.gameObjectName = name;
        newNode.name = name;
        GRoot.inst.AddChild(newNode);
        return newNode;
    }

    public GComponent GetLayerByType(UILayer UILayer)
    {
        switch (UILayer)
        {
            case UILayer.BaseScene:
                return _baseScene;
            case UILayer.UIScene:
                return _uiScene;
            case UILayer.Msg:
                return _msgLayer;
            case UILayer.Guide:
                return _guideLayer;
            case UILayer.Toast:
                return _toastLayer;
            default:
                return null;
        }
    }
}
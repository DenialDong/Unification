using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class SubUIWindowMgr : BaseSubLayerMgr
{
    
    private AsyncWindow _currentWindow;
    private Stack<AsyncWindow> _popArr = new Stack<AsyncWindow>();
    private Queue<WindowQueueInfo> waitLastShowPages = new Queue<WindowQueueInfo>();
    private Queue<WindowQueueInfo> waitFirstShowPages = new Queue<WindowQueueInfo>();

    public async UniTask<AsyncWindow> ShowWindow(string pageName, params object[] param)
    {
        var asyncWindow = await _showWindow(pageName, ChangePageType.Run, param);
        return asyncWindow;
    }

    public async UniTask<AsyncWindow> PushWindow(string pageName, params object[] param)
    {
        var asyncWindow = await _showWindow(pageName, ChangePageType.Push, param);
        return asyncWindow;
    }

    public async UniTask<AsyncWindow> ChangeWindow(string pageName, params object[] param)
    {
        var asyncWindow = await _showWindow(pageName, ChangePageType.Change, param);
        return asyncWindow;
    }

    public async UniTask<AsyncWindow> PopWindow()
    {
        AsyncWindow popWindow = null;
        //优先弹出队列，弹出完毕再检测之前的面板
        if (waitFirstShowPages.Count > 0)
        {
            var waitShowPage = waitFirstShowPages.Dequeue();
            popWindow = await ShowWindow(waitShowPage.pageName, waitShowPage.data);
            _currentWindow = popWindow;
            return popWindow;
        }

        if (_popArr.Count <= 0)
        {
            //弹出完毕检查是不是有等待弹出的界面，然后弹出来
            if (waitLastShowPages.Count > 0)
            {
                var waitShowPage = waitLastShowPages.Dequeue();
                popWindow = await ShowWindow(waitShowPage.pageName, waitShowPage.data);
                _currentWindow = popWindow;
                return popWindow;
            }

            Logger.Log("pop bottom !~!");
            _currentWindow = null;
            return null;
        }

        CheckDestroyLastWindow(false);
        _currentWindow = _popArr.Pop();
        if (_currentWindow.visible == false)
        {
            _currentWindow.AddSelfToOldParent();
        }

        return _currentWindow;
    }


    private async UniTask<AsyncWindow> _showWindow(string pageName, ChangePageType type = ChangePageType.Push,
        params object[] param)
    {
        //允许打开相同的界面  如果不允许后面再处理这里

        if (type == ChangePageType.Change)
        {
            CheckDestroyLastWindow(false);
        }

        if (_currentWindow != null)
        {
            if (type != ChangePageType.Change)
                _popArr.Push(_currentWindow);
            if (type == ChangePageType.Push || type == ChangePageType.Change)
            {
                _currentWindow.RemoveSelf();
            }
        }

        _currentWindow = await this.ShowUIAsync(pageName, SceneLayerType.Window, param);

        return _currentWindow;
    }


    private void CheckDestroyLastWindow(bool checkPop)
    {
        if (_currentWindow != null && !_currentWindow.hasDestory)
        {
            _currentWindow.Close(checkPop);
        }
    }

}
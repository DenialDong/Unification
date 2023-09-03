using UnityEngine;

/// <summary>
/// 流程准备工作
/// </summary>
internal class FsmPatchPrepare : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }
    void IStateNode.OnEnter()
    {
        // 加载更新面板
        Logger.Log($"打开更新面板");

        var updatePanel = new CheckUpdatePanel();
        updatePanel.ShowPanel();


        _machine.ChangeState<FsmInitialize>();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}
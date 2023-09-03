
/// <summary>
/// 流程更新完毕
/// </summary>
internal class FsmPatchDone : IStateNode
{
    void IStateNode.OnCreate(StateMachine machine)
    {
    }

    void IStateNode.OnEnter()
    {
        Logger.Log("开始游戏！");
        EventMgr.Get<StringEvent>().Set(HEventType.PatchStatesChange, "开始游戏！").Send();

        // 开启游戏流程
        EventMgr.Get<EmptyEvent>().Set(HEventType.BeginGame).Send();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }
}
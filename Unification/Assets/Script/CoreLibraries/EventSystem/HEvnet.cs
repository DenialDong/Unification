public class HEvent
{
    public HEventType EventType { get; protected set; }
    public void Send()
    {
        EventMgr.Send(this);
    }
    public void Send(HEventType evtType)
    {
        EventMgr.Send(evtType);
    }
}
using System;

public sealed class EventMgr : Singleton<EventMgr>
{
    public EventDelegate<HEventType, HEvent> Delegate;

    protected override void OnInit()
    {
        base.OnInit();
        Delegate = new EventDelegate<HEventType, HEvent>();
    }

    protected override void OnUnInit()
    {
        Delegate.Clear();
        base.OnUnInit();
    }


    public void Register(HEventType etype, Action<HEvent> action)
    {
        Delegate.Register(etype, action);
    }

    public void UnRegister(HEventType etype, Action<HEvent> action)
    {
        Delegate.UnRegister(etype, action);
    }

    public static T Get<T>() where T : HEvent, new()
    {
        var t = EventMgr.Instance.Delegate.GetEvent<T>();
        if (t == null)
        {
            t = new T();
        }
        return t;
    }

    public static void Send(HEvent data)
    {
        EventMgr.Instance.Delegate.Broadcast(data.EventType, data);
    }
    public static void Send(HEventType etype)
    {
        EventMgr.Instance.Delegate.Broadcast(etype, null);
    }

}
using System;
using System.Collections.Generic;

public class EventDelegate<Tkey, TValue>
{
    private Dictionary<Tkey, Action<TValue>> m_Event;
    private Dictionary<Type, Queue<TValue>> m_Cache;

    public EventDelegate()
    {
        this.m_Event = new Dictionary<Tkey, Action<TValue>>();
        this.m_Cache = new Dictionary<Type, Queue<TValue>>();
    }

    public void Clear()
    {
        this.m_Event.Clear();
        this.m_Cache.Clear();
    }

    public virtual void Register(Tkey etype, Action<TValue> action)
    {
        if (!this.m_Event.ContainsKey(etype))
            this.m_Event.Add(etype, action);
        else
            this.m_Event[etype] += action;
    }

    public virtual void UnRegister(Tkey etype, Action<TValue> action)
    {
        Action<TValue> action1;
        if (!this.m_Event.TryGetValue(etype, out action1))
            return;
        Action<TValue> action2 = action1 - action;
        this.m_Event.Remove(etype);
        if (action2 != null)
            this.m_Event.Add(etype, action2);
    }

    public bool HasRegister(Tkey etype) => this.m_Event.ContainsKey(etype);

    public void Broadcast(Tkey k, TValue t)
    {
        Action<TValue> action;
        if (this.m_Event.TryGetValue(k, out action) && action != null)
            action(t);
        Queue<TValue> objQueue;
        if (!this.m_Cache.TryGetValue(k.GetType(), out objQueue))
        {
            objQueue = new Queue<TValue>();
            this.m_Cache.Add(k.GetType(), objQueue);
        }
        objQueue.Enqueue(t);
    }

    public T GetEvent<T>() where T : TValue
    {
        Queue<TValue> objQueue;
        return this.m_Cache.TryGetValue(typeof (T), out objQueue) && objQueue.Count > 0 ? (T) (object) objQueue.Dequeue() : default (T);
    }
}
using System.Numerics;

internal sealed class EmptyEvent : HEvent
{
    public EmptyEvent Set(HEventType evt)
    {
        EventType = evt;
        return this;
    }
}

internal sealed class IntEvent : HEvent
{
    public int Value { get; private set; }

    public IntEvent Set(HEventType et, int val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class IntArrayEvent : HEvent
{
    public int[] Value { get; private set; }

    public IntArrayEvent Set(HEventType et, params int[] val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class FloatEvent : HEvent
{
    public float Value { get; private set; }

    public FloatEvent Set(HEventType et, float val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class FloatArrayEvent : HEvent
{
    public float[] Value { get; private set; }

    public FloatArrayEvent Set(HEventType et, params float[] val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class StringEvent : HEvent
{
    public string Value { get; private set; }

    public StringEvent Set(HEventType et, string val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class StringArrayEvent : HEvent
{
    public string[] Value { get; private set; }


    public StringArrayEvent Set(HEventType et, params string[] val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}


internal sealed class ObjectEvent : HEvent
{
    public object Value { get; private set; }

    public ObjectEvent Set(HEventType et, object val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class ObjectArrayEvent : HEvent
{
    public object[] Value { get; private set; }

    public ObjectArrayEvent Set(HEventType et, params object[] val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class BoolEvent : HEvent
{
    public bool Value { get; private set; }

    public BoolEvent Set(HEventType et, bool val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class Vector2Event : HEvent
{
    public Vector2 Value { get; private set; }

    public Vector2Event Set(HEventType et, Vector2 val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class Vector3Event : HEvent
{
    public Vector3 Value { get; private set; }

    public Vector3Event Set(HEventType et, Vector3 val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class LongEvent : HEvent
{
    public long Value { get; private set; }

    public LongEvent Set(HEventType et, long val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal sealed class LongArrayEvent : HEvent
{
    public long[] Value { get; private set; }

    public LongArrayEvent Set(HEventType et, params long[] val)
    {
        Value = val;
        EventType = et;
        return this;
    }
}

internal class AnyEvent : HEvent
{
    public Google.Protobuf.WellKnownTypes.Any any { get; private set; }

    public AnyEvent Set(HEventType et, Google.Protobuf.WellKnownTypes.Any any)
    {
        this.any = any;
        EventType = et;
        return this;
    }
}
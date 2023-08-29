using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : class, new()
{
    protected bool m_Init = false;
    private static readonly T m_Instance = new T();

    protected Singleton()
    {
        if ((object) Singleton<T>.m_Instance != null)
            throw new Exception(Singleton<T>.m_Instance.ToString() + " can not be created again.");
        this.m_Init = false;
    }
    static Singleton()
    {
    }
    public static T Instance => Singleton<T>.m_Instance;

    protected virtual void OnInit()
    {
    }

    protected virtual void OnUnInit()
    {
    }

    public void Init()
    {
        if (this.m_Init)
            return;
        this.m_Init = true;
        this.OnInit();
    }

    public void UnInit()
    {
        if (!this.m_Init)
            return;
        this.m_Init = false;
        this.OnUnInit();
    }
}
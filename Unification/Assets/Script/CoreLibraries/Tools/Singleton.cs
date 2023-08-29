using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    protected bool m_Init = false;


    protected Singleton()
    {
        if (null != m_Instance)
        {
            throw new Exception(m_Instance.ToString() + @" can not be create again.");
        }

        m_Init = false;
    }

    private static T m_Instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (m_Instance != null)
                return m_Instance;
            lock (_lock)
            {
                m_Instance ??= new T();
                m_Instance.Init();
            }

            return m_Instance;
        }
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnUnInit()
    {
    }

    public void Init()
    {
        if (!m_Init)
        {
            m_Init = true;
            OnInit();
        }
    }

    public void UnInit()
    {
        if (m_Init)
        {
            m_Init = false;
            OnUnInit();
        }
    }
}
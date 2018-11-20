using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XGame.Client.Base.Pattern;

public class XEventManager : XSingleton<XEventManager>
{
    public delegate void XGlobalEventHandler(EEvent evt, params object[] args);

    private HashSet<XGlobalEventHandler>[] m_AllGlobalHandler;

    public XEventManager()
    {
        m_AllGlobalHandler = new HashSet<XGlobalEventHandler>[(int)EEvent.End];
		for(int i=0; i<m_AllGlobalHandler.Length; i++)
		{
			m_AllGlobalHandler[i] = new HashSet<XGlobalEventHandler>();
		}
    }

    public void Init()
    {
    }

    public void AddHandler(XGlobalEventHandler handler, params EEvent[] events)
    {
        foreach (EEvent e in events)
        {
            m_AllGlobalHandler[(int)e].Add(handler);
        }
    }

    public void DelHandler(EEvent e, XGlobalEventHandler handler)
    {
        m_AllGlobalHandler[(int)e].Remove(handler);
    }

    public void ClearHandler(EEvent e)
    {
        m_AllGlobalHandler[(int)e].Clear();
    }

    public void ClearAllHandler()
    {
		for(int i=0; i<m_AllGlobalHandler.Length; i++)
		{
			this.ClearHandler((EEvent)i);
		}
    }

    public void SendEvent(EEvent e, params object[] args)
    {
        foreach (XGlobalEventHandler handler in m_AllGlobalHandler[(int)e])
        {
            handler(e, args);
        }
    }
}

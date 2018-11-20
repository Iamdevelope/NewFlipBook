//using System;
//using System.Collections.Generic;
//using XGame.Client.Base.Pattern;
//
//interface IEventCtrl
//{
//    void Init();
//	void Breathe();
//}
//
//class XEventCtrlPool : XSingleton<XEventCtrlPool>
//{
//    private List<IEventCtrl> m_EvtCtrlPool = new List<IEventCtrl>();
//
//    public void Init()
//    {
//        m_EvtCtrlPool.Add(new XECServerRet());
//        m_EvtCtrlPool.Add(XUICtrlManager.SP);
//
//		foreach (IEventCtrl ctrl in m_EvtCtrlPool)
//		{
//            ctrl.Init();
//		}
//    }
//	
//	public void Breathe()
//	{
//		foreach(IEventCtrl ctrl in m_EvtCtrlPool)
//		{
//			ctrl.Breathe();
//		}
//	}
//}

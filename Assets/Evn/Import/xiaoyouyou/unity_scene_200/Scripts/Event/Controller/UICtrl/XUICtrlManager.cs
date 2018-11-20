//using System;
//using UnityEngine;
//using XGame.Client.Base.Pattern;
//using System.Collections;
//using System.Collections.Generic;
//
//class XUICtrlManager : XSingleton<XUICtrlManager>, IEventCtrl
//{
//    public SortedList<EUIPanel, XUICtrlBase> m_UICtrls;
//    public Hashtable m_UI2Ctrl;
//
//    public XUICtrlManager()
//    {
//        m_UICtrls = new SortedList<EUIPanel, XUICtrlBase>();
//        m_UI2Ctrl = new Hashtable();
//
//        XEventManager.SP.AddHandler(Show, EEvent.UI_Show);
//        XEventManager.SP.AddHandler(Hide, EEvent.UI_Hide);
//        XEventManager.SP.AddHandler(Toggle, EEvent.UI_Toggle);
//        XEventManager.SP.AddHandler(ReqOriginal, EEvent.UI_ReqOriginal);
//        XEventManager.SP.AddHandler(ShowMainUI, EEvent.UI_ShowMainUI);
//        XEventManager.SP.AddHandler(HideAllUI, EEvent.UI_HideAllUI);
//
//        XEventManager.SP.AddHandler(OnOriginal, EEvent.UI_OnOriginal);
//        XEventManager.SP.AddHandler(OnCreated, EEvent.UI_OnCreated);
//        XEventManager.SP.AddHandler(OnShow, EEvent.UI_OnShow);
//        XEventManager.SP.AddHandler(OnHide, EEvent.UI_OnHide);
//    }
//
//    public void Init()
//    {
//        m_UICtrls.Add(EUIPanel.eLoginUI, new XUTLoginUI());
//        m_UICtrls.Add(EUIPanel.eServerListUI, new XUTServerListUI());
//        m_UICtrls.Add(EUIPanel.eCharOper, new XUTCharacterOperation());
//        m_UICtrls.Add(EUIPanel.ePlayerHead, new XUIPlayerHead());
//        m_UICtrls.Add(EUIPanel.eNpcHead, new XUINpcHead());
//        m_UICtrls.Add(EUIPanel.eMonsterHead, new XUIMonsterHead());
//        m_UICtrls.Add(EUIPanel.eObjectHalf, new XUTObjectHalf());
//        m_UICtrls.Add(EUIPanel.eChatWindow, new XUTChatWindow());
//        m_UICtrls.Add(EUIPanel.eMainPlayerInfo, new XUTMainPlayerInfo());
//        m_UICtrls.Add(EUIPanel.eFunctionButton, new XUTFunctionButton());
//        m_UICtrls.Add(EUIPanel.eNpcDialog, new XUTNpcDialog());
//        m_UICtrls.Add(EUIPanel.eScenePassed, new XUTScenePassed());
//        m_UICtrls.Add(EUIPanel.eMissionTip, new XUTMissionTip());
//        m_UICtrls.Add(EUIPanel.eSelectScene, new XUTSelectScene());
//        m_UICtrls.Add(EUIPanel.eSkillOpertation, new XUTSkillOperation());
//        m_UICtrls.Add(EUIPanel.eToolTipA, new XUTToolTipA());
//        m_UICtrls.Add(EUIPanel.eCenterTip, new XUTCenterTip());
//        m_UICtrls.Add(EUIPanel.eFightWin, new XUTFightWin());
//        m_UICtrls.Add(EUIPanel.eFightFail, new XUTFightFail());
//        m_UICtrls.Add(EUIPanel.eRoleInformation, new XUIRoleInformation());
//        m_UICtrls.Add(EUIPanel.eBagWindow, new XUIBagWindow());
//        m_UICtrls.Add(EUIPanel.eWorldMap, new XUTWorldMap());
//        m_UICtrls.Add(EUIPanel.ePopMenu, new XUIPopMenu());
//        m_UICtrls.Add(EUIPanel.eCursor, new XUICursor());
//        m_UICtrls.Add(EUIPanel.eMessageBox, new XUTMessageBox());
//        m_UICtrls.Add(EUIPanel.eFunctionBottomTR, new XUIFunctionBottomTR());
//        m_UICtrls.Add(EUIPanel.eSmallMap, new XUISmallMap());
//        m_UICtrls.Add(EUIPanel.eStrengthenWindow, new XUIStrengthenWindow());
//        m_UICtrls.Add(EUIPanel.eSelectScene_Pop, new XUTSelectScene_Pop());
//        m_UICtrls.Add(EUIPanel.eLoadSceneUI, new XUTLoadScene());
//        m_UICtrls.Add(EUIPanel.eFightHeadShow, new XUTFightHeadShow());
//        m_UICtrls.Add(EUIPanel.ePassInfo, new XUTPassInfo());
//        m_UICtrls.Add(EUIPanel.eToolTipB, new XUTToolTipB());
//        m_UICtrls.Add(EUIPanel.eFormation, new XUTFormation());
//        m_UICtrls.Add(EUIPanel.eMissionDialog, new XUTMissionDialog());
//        m_UICtrls.Add(EUIPanel.eInputMessageBox, new XUTInputMessageBox());
//        m_UICtrls.Add(EUIPanel.eProduct, new XUTProduct());
//        m_UICtrls.Add(EUIPanel.eReadTip, new XUTReadTip());
//        m_UICtrls.Add(EUIPanel.eShopDialog, new XUTShopDialog());
//        m_UICtrls.Add(EUIPanel.eAuction, new XUTAuction());
//        m_UICtrls.Add(EUIPanel.eFuncUnLock, new XUTFuncUnLock());
//        m_UICtrls.Add(EUIPanel.eFriend, new XUTFriend());
//        m_UICtrls.Add(EUIPanel.eMail, new XUTMail());
//        m_UICtrls.Add(EUIPanel.eMailBox, new XUTMailBox());
//        m_UICtrls.Add(EUIPanel.eStorage, new XUTStorage());
//        m_UICtrls.Add(EUIPanel.eGMWindow, new XUTGMWindow());
//        m_UICtrls.Add(EUIPanel.eTargetInfo, new XUTTargetInfo());
//        m_UICtrls.Add(EUIPanel.eXianDH, new XUTXianDH());
//        m_UICtrls.Add(EUIPanel.ePrivateChat, new XUTPrivateChat());
//        m_UICtrls.Add(EUIPanel.eMoneyTree, new XUTMoneyTreeUI());
//        m_UICtrls.Add(EUIPanel.eShanHT, new XUTShanHT());
//		m_UICtrls.Add(EUIPanel.eCutSceneDialog,new XUICutScenePanel());
//        m_UICtrls.Add(EUIPanel.eOnlineReawrd, new XUTOnlineReward());
//		m_UICtrls.Add(EUIPanel.ePVPResult, new XUTPVPResult());
//		m_UICtrls.Add(EUIPanel.ePractice, new XUTPractise());
//        m_UICtrls.Add(EUIPanel.eFriendCharmRank, new XUTFriendCharmRank());
//        m_UICtrls.Add(EUIPanel.eFriendFlowerHouse, new XUTFriendFlowerHouse());
//
//        //在这里讲每个界面的显示模式设置上，以后UI需要配置的属性多起来的话，可以放到配置文件中
//        //现在先放置到这里，不设置的话，切换模式的话，对该UI不做任何处理
//        m_UICtrls[EUIPanel.eChatWindow].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL | 1 << (int)UI_MODE.UI_MODE_STATIC_SUB_SCENE | 1 << (int)UI_MODE.UI_MODE_STATIC_BATTLE;
//        m_UICtrls[EUIPanel.eMainPlayerInfo].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL | 1 << (int)UI_MODE.UI_MODE_STATIC_SUB_SCENE;
//        m_UICtrls[EUIPanel.eFunctionButton].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL | 1 << (int)UI_MODE.UI_MODE_STATIC_SUB_SCENE;
//        m_UICtrls[EUIPanel.eNpcDialog].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eScenePassed].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eSelectScene].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eSkillOpertation].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eToolTipA].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eCenterTip].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL | 1 << (int)UI_MODE.UI_MODE_STATIC_SUB_SCENE | 1 << (int)UI_MODE.UI_MODE_STATIC_BATTLE;
//        m_UICtrls[EUIPanel.eFightWin].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eFightFail].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eRoleInformation].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eBagWindow].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eWorldMap].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.ePopMenu].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        //m_UICtrls[EUIPanel.eFunctionBottomTR].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL;
//        m_UICtrls[EUIPanel.eSmallMap].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL;
//        m_UICtrls[EUIPanel.eStrengthenWindow].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eSelectScene_Pop].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eFightHeadShow].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_BATTLE;
//        m_UICtrls[EUIPanel.ePassInfo].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_SUB_SCENE;
//        m_UICtrls[EUIPanel.eMissionTip].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL | 1 << (int)UI_MODE.UI_MODE_STATIC_SUB_SCENE;
//        m_UICtrls[EUIPanel.eFormation].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eMissionDialog].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eShopDialog].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eFuncUnLock].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eFriend].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eMail].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eMailBox].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eStorage].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eGMWindow].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eTargetInfo].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eXianDH].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.ePrivateChat].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eMoneyTree].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eShanHT].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//		m_UICtrls[EUIPanel.eCutSceneDialog].UIMode = 1 << (int)UI_MODE.UI_MODE_CUTSCENE;
//        m_UICtrls[EUIPanel.eOnlineReawrd].UIMode = 1 << (int)UI_MODE.UI_MODE_STATIC_NORMAL;
//		m_UICtrls[EUIPanel.ePVPResult].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//		m_UICtrls[EUIPanel.ePractice].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eFriendCharmRank].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//        m_UICtrls[EUIPanel.eFriendFlowerHouse].UIMode = 1 << (int)UI_MODE.UI_MODE_DYNAMIC;
//    }
//
//    public void Breathe()
//    {
//        foreach (XUICtrlBase uiCtrl in m_UICtrls.Values)
//        {
//            uiCtrl.Breathe();
//        }
//    }
//
//    void ShowMainUI(EEvent evt, params object[] args)
//    {
//        Show(EEvent.UI_Show, EUIPanel.eChatWindow, null);
//        Show(EEvent.UI_Show, EUIPanel.eMainPlayerInfo, null);
//        Show(EEvent.UI_Show, EUIPanel.eFunctionButton, null);
//        Show(EEvent.UI_Show, EUIPanel.eFunctionBottomTR, null);
//        Show(EEvent.UI_Show, EUIPanel.eSmallMap, null);
//        Show(EEvent.UI_Show, EUIPanel.eMissionTip, null);
//    }
//
//    void HideAllUI(EEvent evt, params object[] args)
//    {
//
//    }
//
//    public void ShowByMode(UI_MODE mode)
//    {
//        //隐藏当前dymaic标记的UI
//        foreach (KeyValuePair<EUIPanel, XUICtrlBase> index in m_UICtrls)
//        {
//            if (index.Value.UIMode == (int)UI_MODE.UI_MODE_DYNAMIC)
//            {
//                index.Value.Hide();
//            }
//
//        }
//
//        //弹出所有相应标记的对象,关闭非NO_OP标记的UI
//        foreach (KeyValuePair<EUIPanel, XUICtrlBase> index in m_UICtrls)
//        {
//            if ((index.Value.UIMode & (1 << (int)mode)) > 0)
//            {
//                Show(EEvent.UI_Show, index.Key);
//            }
//            else
//            {
//                if (index.Value.UIMode != (int)UI_MODE.UI_MODE_NO_OP)
//                {
//                    index.Value.Hide();
//                }
//            }
//        }
//    }
//
//    void Show(EEvent evt, params object[] args)
//    {
//        EUIPanel ePanel = (EUIPanel)args[0];
//        if (!m_UICtrls.ContainsKey(ePanel))
//            return;
//
//        if (!m_UICtrls[ePanel].IsCanShow())
//            return;
//
//        if (m_UICtrls[ePanel].Show())
//            return;
//
//        XWeUIManager.SP.CreateUI(ePanel);
//    }
//
//    void Hide(EEvent evt, params object[] args)
//    {
//        EUIPanel ePanel = (EUIPanel)args[0];
//        if (!m_UICtrls.ContainsKey(ePanel))
//            return;
//
//        m_UICtrls[ePanel].Hide();
//    }
//
//    void Toggle(EEvent evt, params object[] args)
//    {
//        EUIPanel ePanel = (EUIPanel)args[0];
//        if (!m_UICtrls.ContainsKey(ePanel))
//            return;
//
//        if (m_UICtrls[ePanel].Toggle())
//            return;
//
//        XWeUIManager.SP.CreateUI(ePanel);
//    }
//
//    void ReqOriginal(EEvent evt, params object[] args)
//    {
//        EUIPanel ePanel = (EUIPanel)args[0];
//        if (!m_UICtrls.ContainsKey(ePanel))
//            return;
//
//        if (m_UICtrls[ePanel].ReqOriginal(args[1]))
//            return;
//
//        XWeUIManager.SP.OriginalUI(ePanel);
//    }
//
//    void OnOriginal(EEvent evt, params object[] args)
//    {
//        EUIPanel ePanel = (EUIPanel)args[1];
//        if (!m_UICtrls.ContainsKey(ePanel))
//            return;
//
//        m_UICtrls[ePanel].OnOriginal(args[0]);
//    }
//
//    void OnCreated(EEvent evt, params object[] args)
//    {
//        EUIPanel ePanel = (EUIPanel)args[1];
//        if (!m_UICtrls.ContainsKey(ePanel))
//            return;
//
//        m_UI2Ctrl.Add(args[0], m_UICtrls[ePanel]);
//        if (m_UICtrls[ePanel] == null)
//            return;
//        m_UICtrls[ePanel].OnCreated(args[0]);
//    }
//
//    void OnShow(EEvent evt, params object[] args)
//    {
//        if (!m_UI2Ctrl.ContainsKey(args[0]))
//            return;
//
//        (m_UI2Ctrl[args[0]] as XUICtrlBase).OnShow();
//    }
//
//    void OnHide(EEvent evt, params object[] args)
//    {
//        if (!m_UI2Ctrl.ContainsKey(args[0]))
//            return;
//
//        (m_UI2Ctrl[args[0]] as XUICtrlBase).OnHide();
//    }
//}

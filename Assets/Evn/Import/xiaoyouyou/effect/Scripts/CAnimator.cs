using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public enum eActionState
{
    None,					//无状态
    Idle,					//idle
    Wait,              //等待状态
    Turning,                //纯转向
    Walk,                   //Walk  和run同属于Move
    BossRamble,             //BOSS漫步
    Run,					//run
    Roll,                   //翻滚
    Charge,             //蓄力
    BossSneer,              //BOSS嘲讽    
    Attack,					//攻击    
    Behit,					//受击
    Down,					//击倒
    BeatFly,				//击飞
    Floating,           //浮空
    GrabHit,            //抓取受击
    Ragdoll,            //布娃娃状态
    BossWeakness,           //BOSS虚弱
    /*    BeGrab,               //被抓取*/
    Assault,				//冲锋
    Skill,                  //持续施法
    Hook,                       //勾取
    Grab,                   //抓取状态
    Stun,					//击晕
    Variation,				//变形
    Shapeshift,             //变身
    Dead,					//死亡
    Resurrect,              //复活
    SwitchPartner,        //切换小伙伴
    RideJump,               //坐骑跳进来
    BeginRide,              //开始上马,马儿嘶叫
    EndRide,                //马儿嘶叫,开始下马
    BeginFly,               //开始和宠物合体
    EndFly,                 //结束和宠物合体
    PetBeginFly,            //宠物开始合体
    PetFlyFight,            //宠物的飞行战斗状态,合体后,宠物自身进入这个状态
    Birth,                  //出生状态
    IdelInHome,     //家园待机
    RunInHome,      //家园跑
}
public class CAnimatorStateInfo
{
    public bool bStartPlay;             //开始播放动作
    public float aniSpeed;          
    public AnimatorStateInfo pAnimatorStateInfo;
}


public struct stStateData
{
    private static Dictionary<eActionState, string> m_ActionDict = new Dictionary<eActionState, string>();

    public static void Init()
    {
        m_ActionDict.Add(eActionState.None, "null");
        m_ActionDict.Add(eActionState.Idle, "idle");
        m_ActionDict.Add(eActionState.Turning, "idle");
        m_ActionDict.Add(eActionState.Walk, "walk");
        m_ActionDict.Add(eActionState.Run, "run");
        m_ActionDict.Add(eActionState.Assault, "run");
        m_ActionDict.Add(eActionState.Roll, "miss");
        m_ActionDict.Add(eActionState.Stun, "yun");
        m_ActionDict.Add(eActionState.Variation, "");
        m_ActionDict.Add(eActionState.Behit, "hit");
        m_ActionDict.Add(eActionState.Down, "down");
        m_ActionDict.Add(eActionState.BeatFly, "fly");
        m_ActionDict.Add(eActionState.Dead, "die");
        m_ActionDict.Add(eActionState.BossRamble, "walk");
        m_ActionDict.Add(eActionState.BossSneer, "provoke");
        m_ActionDict.Add(eActionState.BossWeakness, "yun");
        m_ActionDict.Add(eActionState.SwitchPartner, "jump");

        m_ActionDict.Add(eActionState.RideJump, "qicheng_jump");
        m_ActionDict.Add(eActionState.BeginRide, "up_down");
        m_ActionDict.Add(eActionState.EndRide, "up_down");

        m_ActionDict.Add(eActionState.BeginFly, "kongzhan_begin");
        m_ActionDict.Add(eActionState.EndFly, "kongzhan_end");
        //m_ActionDict.Add(eActionState.Shapeshift, "idle");
        m_ActionDict.Add(eActionState.IdelInHome,"xiuxian");
        m_ActionDict.Add(eActionState.RunInHome,"pao");
    }

    public static string GetActionName(eActionState state)
    {
        string sAniname = "";
        m_ActionDict.TryGetValue(state, out sAniname);

        return sAniname;
    }

    public static eActionState GetState(string actionName)
    {
        foreach (KeyValuePair<eActionState, string> kvp in m_ActionDict)
        {
            if (kvp.Value == actionName)
            {
                return kvp.Key;
            }
        }
        return eActionState.None;
    }
}

public class CAnimator
{
    private Animator m_pAnimator;

    private CAnimatorStateInfo m_pAniInfo;
    private eActionState m_eActionState;

    private int m_nLayer;           //动作的层级
    private string m_szAniName;


    private bool m_bPlayAction;     

    private const string BaseLayerName = "Base Layer";


    public CAnimator(Animator animator,int layer = 0)
    {
        m_pAnimator = animator;
        m_nLayer = 0;
        m_eActionState = eActionState.None;


        m_pAniInfo = new CAnimatorStateInfo();
        m_pAniInfo.bStartPlay = false;
 
        m_bPlayAction = false;
    }

    public void Reset()
    {
        m_szAniName = "";
    }

    public bool Enabled
    {
        get 
        {
            return m_pAnimator.enabled;
        }
        set
        {
            m_pAnimator.enabled = value;
        }
    }

    public string PlayActionName
    {
        get
        {
            return m_szAniName;
        }
    }

    //bRepeat为true时允许重复播放相同的动作
    public void PlayAction(string aniName, float speed = 1.0f,bool bRepeat = false)
    {
        if (null == m_pAnimator || !m_pAnimator.enabled)
        {
            return;
        }
        if (string.IsNullOrEmpty(aniName) || aniName == "0")
        {
            return;
        }

        if (!bRepeat && m_szAniName == aniName)
        {
            return;
        }

        //将上一个动作停止
        if (m_szAniName != aniName && !string.IsNullOrEmpty(m_szAniName))
        {
            m_pAnimator.SetBool(Animator.StringToHash(m_szAniName), false);
        }

        //播放新动作        
        m_pAnimator.SetBool(Animator.StringToHash(aniName), true);

        if (!m_pAnimator.GetBool(Animator.StringToHash(aniName)))
        {
            Debug.LogError(" Error! can not find animation : " + aniName + "  transform name : " + m_pAnimator.transform.name);
            return;
        }

        m_pAnimator.speed = speed;
        m_bPlayAction = true;
        m_szAniName = aniName;
        m_pAniInfo.bStartPlay = false;
    }


    public void Update() 
    {
        if (null == m_pAnimator || !m_pAnimator.enabled) return;

        AnimatorStateInfo state = m_pAnimator.GetNextAnimatorStateInfo(0);
       
        //避免anystate多次触发
        if (m_bPlayAction)
        {
            if (state.nameHash == Animator.StringToHash(BaseLayerName + "." + m_szAniName))            
            {
                m_pAnimator.SetBool(Animator.StringToHash(m_szAniName), false);

                m_bPlayAction = false;
                m_pAniInfo.bStartPlay = true;
                m_pAniInfo.pAnimatorStateInfo = state;
                m_pAniInfo.aniSpeed = m_pAnimator.speed;
            }
        }
    }



    public CAnimatorStateInfo GetAnimatorInfo() 
    {
        return m_pAniInfo;
    }


    public float Speed
    {
        get
        {
            if (null == m_pAnimator || !m_pAnimator.enabled ) return 0f;

            return m_pAnimator.speed;
        }
        set
        {
            if (null == m_pAnimator || !m_pAnimator.enabled) return;

            m_pAnimator.speed = value;
        }
    }

    public void StopAnimation()
    {
        if (null == m_pAnimator || !m_pAnimator.enabled) return;

//         string szAniName = stStateData.GetActionName(eActionState.Idle);
//         m_pAnimator.SetBool(szAniName, true);                        

        m_pAnimator.SetBool(m_szAniName, false);
    }


}
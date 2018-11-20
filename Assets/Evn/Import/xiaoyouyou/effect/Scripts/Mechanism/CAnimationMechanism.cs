using UnityEngine;
using System.Collections;
using System;

public class CAnimationMechanism : CBaseMechanism {

    public bool m_triggerForOther = false;
    public bool m_loop;
    

    void Awake()
    {
        m_type = 3;
    }

    protected override void Start()
    {
        base.Start();

        if (m_debug && m_animation != null)
        {
            foreach (AnimationState state in m_animation)
            {
                if (m_triggerForOther)
                {
                    state.wrapMode = WrapMode.Default;
                }
                else
                {
                    state.wrapMode = WrapMode.Loop;
                }
            }

            if (!m_triggerForOther)
                m_animation.Play();
        }
    }

    public override void Init(string[] args)
    {
        base.Init(args);
        m_triggerForOther = Convert.ToBoolean(args[0]);
        m_loop = Convert.ToBoolean(args[1]);

        if (m_animation != null)
        {
            foreach (AnimationState state in m_animation)
            {
                if (m_loop)
                    m_animation.wrapMode = WrapMode.Loop;
                else
                    m_animation.wrapMode = WrapMode.ClampForever;

                if (!m_triggerForOther)
                    m_animation.Play();
            }
        }
    }

    public override bool TriggerOther(string[] param)
    {
        if (m_triggerForOther)
        {
            if (m_triggerCount <= 0)
            {
                gameObject.SetActive(false);
                return false;
            }
            if (m_animation == null)
            {
                Debug.LogError("CAnimationMechanism TriggerOther m_animation is null");
                gameObject.SetActive(false);
                return false;
            }
            if (!m_animation.isPlaying)
            {
                m_animation.CrossFade(param[1]);
                m_triggerCount--;
            }
        }

        return true;
    }

    protected override bool OnTrigger(Collider other, CBaseMechanism mechanism)
    {
        float timeNow = Time.time;
        if (timeNow - m_lastTriggerTime < m_triggerSpaceTime)
            return false;

        if (m_triggerCount == 0)
        {
            this.enabled = false;
            return false;
        }

        if (m_callback != null)
            m_callback(other, mechanism, m_revertCallback);

        if (GetComponent<ParticleSystem>() != null)
            GetComponent<ParticleSystem>().Play();

        m_triggerCount--;

        return true;
    }
	
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public delegate void MechanismCallback(Collider other, CBaseMechanism mechanism, MechanismRevertCallback callback);
public delegate void MechanismRevertCallback();

public class CBaseMechanism : MonoBehaviour
{

    protected MechanismCallback m_callback;
    public MechanismCallback Callback
    {
        get { return m_callback; }
        set { m_callback = value; }
    }

    protected MechanismRevertCallback m_revertCallback = null;

    protected bool m_start = false;

    [SerializeField]
    protected int m_layers = 1 << 8;
    public int Layers
    {
        get { return m_layers; }
        set { m_layers = value; }
    }

    public static bool m_debug = true;

    protected int m_type;
    public int Type
    {
        get { return m_type; }
    }

    private List<uint> m_scriotID = new List<uint>();
    public List<uint> ScriotID
    {
        get { return m_scriotID; }
        set { m_scriotID = value; }
    }

    [SerializeField]
    protected string[] m_args;

    protected Animation m_animation;

    [SerializeField]
    protected int m_triggerCount = 1;
    public int TriggerCount
    {
        get { return m_triggerCount; }
        set { m_triggerCount = value; }
    }

    [SerializeField]
    protected float m_triggerSpaceTime;
    public float TriggerSpaceTime
    {
        get { return m_triggerSpaceTime; }
        set
        {
            m_triggerSpaceTime = Mathf.Abs(value);
            m_lastTriggerTime = -m_triggerSpaceTime;
        }
    }

    protected float m_lastTriggerTime = -1;

    protected virtual void Start()
    {
        Collider[] colls = GetComponentsInChildren<Collider>();
        int len = colls.Length;
        for (int i = 0; i < len; i++)
        {
            colls[i].isTrigger = true;
        }

        if (transform.childCount > 0)
            m_animation = transform.GetChild(0).GetComponent<Animation>();
        else
            m_animation = transform.GetComponent<Animation>();

        if (m_animation == null)
        {
            Debug.Log("CBaseMechanism Start animation is null : " + this.ToString() + "\t" + gameObject.name);
        }


        if (m_animation != null &&( m_animation.playAutomatically || m_animation.isPlaying))
        {
            m_animation.playAutomatically = false;
            m_animation.Stop();
        }
        if (!m_debug)
            return;

        m_triggerSpaceTime = Mathf.Abs(m_triggerSpaceTime);
        m_lastTriggerTime = -m_triggerSpaceTime;

        Reset();
        m_start = true;
    }

    public virtual void Init(string[] args)
    {
        m_start = true;
        m_args = args;
        Reset();
    }

    protected virtual void Reset()
    {
 
    }

    public virtual bool TriggerOther(string[] param)
    {
        if (m_triggerCount > 0)
            return true;
        else
        {
            gameObject.SetActive(false);
            return false;
        }
    }

    public void Trigger(Collider other,CBaseMechanism mechanism)
    {
        OnTrigger(other,mechanism);
    }

    protected virtual bool OnTrigger(Collider other, CBaseMechanism mechanism)
    {
        float timeNow = Time.time;
        if (timeNow - m_lastTriggerTime < m_triggerSpaceTime)
            return false;

        m_lastTriggerTime = timeNow;

        if (m_triggerCount == 0)
        {
            this.enabled = false;
            return false;
        }

        if (m_callback != null)
            m_callback(other,mechanism, m_revertCallback);

        if (m_animation != null)
        {
            m_animation.Play();
        }

        if (GetComponent<ParticleSystem>() != null)
            GetComponent<ParticleSystem>().Play();

        m_triggerCount--;

        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        layer = 1 << layer;
        if ((layer & m_layers) > 0)
        {
            Trigger(other,this);
            return;
        }
    }
}

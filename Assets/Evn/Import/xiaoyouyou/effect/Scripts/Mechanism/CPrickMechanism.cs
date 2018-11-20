using UnityEngine;
using System.Collections;
using System;

public class CPrickMechanism : CBaseMechanism
{
    public float m_moveSpeed;
    public float m_moveLength;

    private float m_lerpSpeed;
    private Vector3 m_startPosition;
    private Vector3 m_endposition;

    private float m_lerpVal;

    void Awake()
    {
        m_type = 2;
    }

    public override void Init(string[] args)
    {
        m_moveSpeed = Convert.ToSingle(args[0]);
        m_moveLength = Convert.ToSingle(args[1]);

        base.Init(args);
    }

    protected override void Reset()
    {
        m_startPosition = transform.position;
        m_endposition = m_startPosition + transform.forward * m_moveLength;
        m_lerpSpeed = 1 / (m_moveLength / m_moveSpeed);
    }

    protected override bool OnTrigger(Collider other, CBaseMechanism mechanism)
    {
        if (base.OnTrigger(other, mechanism))
            m_start = true;

        return true;
    }

    void Update()
    {
        if (!m_start)
            return;

        transform.position = Vector3.Lerp(m_startPosition, m_endposition, m_lerpVal);

        m_lerpVal += m_lerpSpeed * Time.deltaTime;

        if (m_lerpVal >= 1)
        {
            m_lerpVal = 1;
            m_lerpSpeed *= -1;
        }

        if (m_lerpVal <= 0)
        {
            m_lerpVal = 0;
            m_lerpSpeed *= -1;
        }
    }
}

using UnityEngine;
using System.Collections;
using System;

public class CMoveMechanism : CBaseMechanism
{
    public float m_moveSpeed;
    public float m_moveLength;
    

    private float m_moveLen;
    private float m_rotateSpeed;
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;

    private Vector3 m_moveDirect;

    private Vector3 m_size;

    void Awake()
    {
        m_type = 1;
    }

    public override void Init(string[] args)
    {
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;

        m_moveSpeed = Convert.ToSingle(args[0]);
        m_moveLength = Convert.ToSingle(args[1]);

        base.Init(args);
    }
	
	void Update () {
        if (!m_start)
            return;

        Vector3 dist = transform.position - m_startPosition;
        if (Vector3.SqrMagnitude(dist) > m_moveLen)
        {
            Reset();
        }

        float deltaTime = Time.deltaTime;
        transform.Translate(m_moveDirect * deltaTime, Space.World);

        transform.Rotate(Vector3.right, m_rotateSpeed * deltaTime);
	}

    protected override void Reset()
    {
        transform.position = m_startPosition;
        transform.rotation = m_startRotation;

        m_moveDirect = transform.forward * m_moveSpeed;

        m_size = GetComponent<MeshFilter>().mesh.bounds.size;

        m_size = new Vector3(m_size.x * transform.lossyScale.x, m_size.y * transform.lossyScale.y, m_size.z * transform.lossyScale.z);

        m_rotateSpeed = (360 * m_moveSpeed) / (m_size.y * Mathf.PI);

        m_moveLen = m_moveLength * m_moveLength;
    }
}

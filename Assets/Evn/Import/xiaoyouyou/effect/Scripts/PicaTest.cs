using UnityEngine;
using System.Collections;

public class PicaTest : MonoBehaviour {

    public Transform m_go;
    public ParticleSystem m_ps;

    public float m_ttt;

	void Start () {
        if (m_ps == null)
        {
            m_ps = GetComponent<ParticleSystem>();
        }

        if (m_go == null)
        {
            m_go = m_ps.transform;
        }
	}
	
	void Update () {
        if (m_ps == null || m_go == null)
        {
            this.enabled = false;
            return;
        }

        m_ps.startRotation =Mathf.Deg2Rad * m_go.eulerAngles.y;
        m_ttt = m_go.eulerAngles.y;

	}
}

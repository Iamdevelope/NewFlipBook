using UnityEngine;
using System.Collections;

public class MobileWaterChangeTexture : MonoBehaviour {

    public GameObject m_go;
    public Texture2D[] m_Tex;
    public float m_speed = 0.1f;
    public int m_index = 0;
	// Use this for initialization
	void Start () {
        InvokeRepeating("PlayTexture", 0, m_speed);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Tex == null || m_Tex.Length == 0)
        {
            LoadTexture();
        }
	}

    private void LoadTexture()
    {
        Object[] temp = Resources.LoadAll("Texture");
        m_Tex = new Texture2D[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            m_Tex[i] = temp[i] as Texture2D;
        }
    }

    private void PlayTexture()
    {
        if (m_Tex != null && m_Tex.Length > 0)
        {
            m_go.GetComponent<Renderer>().sharedMaterial.mainTexture = m_Tex[m_index];
            m_index++;
            if (m_index >= m_Tex.Length)
                m_index = 0;
        }
    }

    void OnApplicationQuit()
    {
        if (IsInvoking())
            CancelInvoke();
    }
}

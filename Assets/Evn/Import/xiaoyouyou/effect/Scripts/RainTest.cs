using UnityEngine;
using System.Collections;

public class RainTest : MonoBehaviour {

    public RainManage m_rainManageObject;
    public Transform m_tran;
    public Transform m_cam;
    public GameObject m_groud;

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_rainManageObject.OpenRain(m_tran, m_cam);
            m_groud.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            m_rainManageObject.CloseRain();
            m_groud.SetActive(false);
        }
    }
#else
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 50), "Open"))
        {
            m_rainManageObject.OpenRain(m_tran, m_cam);
            m_groud.SetActive(true);
        }

        if (GUI.Button(new Rect(100, 300, 100, 50), "Close"))
        {
            m_rainManageObject.CloseRain();
            m_groud.SetActive(false);
        }
    }
#endif
}

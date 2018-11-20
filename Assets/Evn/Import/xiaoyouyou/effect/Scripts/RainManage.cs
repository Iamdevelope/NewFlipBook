using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RainManage : MonoBehaviour {

    private enum eRainRunState
    {
        None = 0,
        Wait = 1,
        Load = 2,
        Group = 3,
        Update = 4,
        Release = 5,
    }

    public enum eRainRange
    {
        Small = 2,
        Big = 3,
    }

    public GameObject m_RaimObject;      //雨的模板物体
    public float m_density = 0.35f;              //下雨的密度（多久下一次）
    public float m_speed = 10;                //雨下落的速度

    public eRainRange m_rainRange = eRainRange.Small;

    private float m_hideLength;           //下落多少米后隐藏
    private int m_count = 2;                  //要创建雨的数量
    private float m_accumulativeTotalTime = 0;    //累计时间，到达密度后下一次雨
    private float m_startHeight;         //开始高度（每个场景会不一样）
    private float m_bountWight;          //雨模型网格包围合的宽（X值）
    private int m_delayFrame = 3;        //等待帧数（为了保证相机已经初始化成功）
    private int m_curFrame = 0;

    private Transform m_followTrans;     //跟随的物体变换
    private Transform m_followCam;       //跟随相机
    private bool m_open;                 //是否打开
    public bool Open
    {
        get { return m_open; }
    }

    private List<Transform> m_hideList = new List<Transform>();    //内存池中的雨（不可见）
    private List<Transform> m_showList = new List<Transform>();    //场景中的雨（可见）
    private List<Transform> m_swapList = new List<Transform>();
    private List<GameObject> m_loadList = new List<GameObject>();

    private GameObject m_temp;
    private Transform m_tempTran;
    private eRainRunState m_state = eRainRunState.None;

	void Update () {
        if (m_state == eRainRunState.None)
            return;

        switch (m_state)
        {
            case eRainRunState.Wait: Wait(); break;
            case eRainRunState.Load: LoadRain(); break;
            case eRainRunState.Group: GroupRain(); break;
            case eRainRunState.Update: ForUpdate(); break;
            case eRainRunState.Release: Relsase(); break;
        }
	}

    public void OpenRain(Transform follow,Transform camTran)
    {
        if (follow == null || camTran == null)
            return;

        if (m_RaimObject == null)
            return;

        if (m_open)
            return;

        m_followCam = camTran;
        m_followTrans = follow;
        m_open = true;
        
        m_startHeight = m_followCam.position.y - m_followTrans.position.y;
        m_startHeight = Mathf.Abs(m_startHeight);
        m_hideLength = m_startHeight + m_RaimObject.GetComponent<Renderer>().bounds.size.y * 0.5f;
        m_bountWight = m_RaimObject.GetComponent<Renderer>().bounds.size.x * m_RaimObject.transform.localScale.x;

        SetState(eRainRunState.Wait);
    }

    public void CloseRain()
    {
        if (!m_open)
            return;

        m_open = false;
        m_curFrame = 0;
    }

    public void Relsase()
    {
        int count = m_showList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(m_showList[i]);
        }
        m_showList.Clear();

        count = m_hideList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(m_hideList[i]);
        }
        m_hideList.Clear();
        m_open = false;
        SetState(eRainRunState.None);
        m_temp = null;
        m_tempTran = null;
    }

    private void Wait()
    {
        if (m_curFrame >= m_delayFrame)
        {
            SetState(eRainRunState.Load);
            m_curFrame = 0;
        }
        m_curFrame++;
    }

    private void LoadRain()
    {
        int count = m_loadList.Count + m_hideList.Count * (int)m_rainRange;
        if (count < m_count * (int)m_rainRange)
        {
            m_temp = Object.Instantiate(m_RaimObject) as GameObject;
            m_temp.transform.parent = transform;
            m_loadList.Add(m_temp);
            return;
        }
        //DynamicShader.ReplaceUnSupportShader(gameObject);
        SetState(eRainRunState.Group);
    }

    private void GroupRain()
    {
        if (m_followCam == null)
        {
            CloseRain();
            SetState(eRainRunState.None);
            return;
        }
        int count = m_loadList.Count;
        if (count == 0)
        {
            SetState(eRainRunState.Update);
            return;
        }
        int groupIndex = -1;
        GameObject tempGo = null;
        Vector3 forward = m_followCam.forward;
        forward.y = 0;
        forward.Normalize();
        Quaternion rotation = Quaternion.LookRotation(forward);
        for (int i = 0; i < count; i++)
        {
            if (groupIndex == -1)
            {
                tempGo = new GameObject("RainGroup");
                tempGo.transform.parent = transform;

                tempGo.transform.rotation = rotation;
                m_hideList.Add(tempGo.transform);
            }
            if (m_rainRange == eRainRange.Small)
                m_loadList[i].transform.position = tempGo.transform.position + tempGo.transform.right * m_bountWight * groupIndex * 0.5f;
            else
                m_loadList[i].transform.position = tempGo.transform.position + tempGo.transform.right * m_bountWight * groupIndex;
            m_loadList[i].transform.parent = tempGo.transform;
            m_loadList[i].transform.localRotation = Quaternion.identity;
            groupIndex++;
            if (groupIndex > 1)
            {
                groupIndex = -1;
            }

            if (m_rainRange == eRainRange.Small)
            {
                if (groupIndex == 0)
                {
                    groupIndex = 1;
                }
            }
        }
        m_loadList.Clear();
        SetState(eRainRunState.Update);
    }

    private void ForUpdate()
    {
        if (m_followCam == null || m_followTrans == null)
        {
            CloseRain();
            
            m_followTrans = null;
            m_followCam = null;
            m_temp = null;
            m_tempTran = null;

            for (int i = 0, ch = m_showList.Count; i < ch; i++)
            {
                m_tempTran = m_showList[i];
                for (int c = 0; c < (int)m_rainRange; c++)
                {
                    m_tempTran.GetChild(c).GetComponent<Renderer>().enabled = false;
                }
                m_hideList.Add(m_showList[i]);
                m_showList.RemoveAt(i);
                i--;
                ch -= 1;
            }
            SetState(eRainRunState.None);
            return;
        }

        if (m_open)
        {
            m_accumulativeTotalTime += Time.deltaTime;
            if (m_accumulativeTotalTime >= m_density)
            {
                m_accumulativeTotalTime = 0;
                if (m_hideList.Count > 0)
                {
                    m_tempTran = m_hideList[0];
                    m_hideList.RemoveAt(0);
                    if (m_tempTran != null)
                    {
                        Vector3 followPos = m_followTrans.position;
                        followPos.y += m_startHeight;
                        m_tempTran.position = followPos;
                        for (int c = 0; c < (int)m_rainRange; c++)
                        {
                            m_tempTran.GetChild(c).GetComponent<Renderer>().enabled = true;
                        }
                        m_showList.Add(m_tempTran);
                    }
                }
            }
        }

        int countShow = m_showList.Count;
        if (!m_open && countShow == 0)
        {
            SetState(eRainRunState.None);
            m_followTrans = null;
            m_followCam = null;
            m_temp = null;
            m_tempTran = null;
            return;
        }
        for (int i = 0; i < countShow; i++)
        {
            m_tempTran = m_showList[i];
            m_tempTran.position -= Vector3.up * m_speed * Time.deltaTime;
            if (m_followCam.position.y - m_tempTran.position.y >= m_hideLength)
            {
                for (int c = 0; c < (int)m_rainRange; c++)
                {
                    m_tempTran.GetChild(c).GetComponent<Renderer>().enabled = false;
                }
                m_hideList.Add(m_showList[i]);
                m_showList.RemoveAt(i);
                i--;
                countShow -= 1;
                //m_swapList.Add(m_tempTran);
            }
        }

        //int len = m_swapList.Count;
        //if (len > 0)
        //{
        //    for (int i = 0; i < len; i++)
        //    {
        //        m_showList.Remove(m_swapList[i]);
        //    }

        //    m_hideList.AddRange(m_swapList);
        //    m_swapList.Clear();
        //}
    }

    private void SetState(eRainRunState state)
    {
        m_state = state;
    }

    public void ResetTransform(Transform followGameObject, Transform folllowCamera)
    {
        m_followTrans = followGameObject;
        m_followCam = folllowCamera;
    }
}

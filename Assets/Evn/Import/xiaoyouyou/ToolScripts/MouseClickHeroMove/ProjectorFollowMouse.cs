using UnityEngine;
using System.Collections;

public class ProjectorFollowMouse :MonoBehaviour{

    public GameObject m_go;                       //储存在内存当中的projector对象
    private Projector m_project;                   //projector实例
    private GameObject m_projectorObject;          //正在使用的projector对象
    private Transform m_projectorTrans;            //正在使用的projector对象变换
    private Vector3 m_position;                    //初始位置
    private Quaternion m_rotation;                 //初始旋转

    //一些属性值
    private float m_nearClipPlane;                 
    private float m_farClipPlane;
    private float m_fieldOfView;
    private float m_aspectRotio;
    private bool m_orthographic;
    private float m_orthographicSize;


    private float m_projectorHight;

    private KeyCode m_key;
    public KeyCode Key
    {
        get { return m_key; }
        set { m_key = value; }
    }
    private bool m_isLoad;

    private Ray m_ray;
    private RaycastHit m_rayHit;
    private float m_rayDistance;
    public Camera m_camrea;


    void Start()
    {
        Init();
    }

    void Update()
    {
        ForUpdate();
    }

    private void Init()
    {

        Key = KeyCode.R;
        m_nearClipPlane = 0.1f;
        m_farClipPlane = 10;
        m_fieldOfView = 30;
        m_aspectRotio = 1;
        m_orthographic = true;
        m_orthographicSize = 1;
        m_projectorHight = 1;
        m_isLoad = false;
        m_position = Vector3.zero;
        m_rotation = Quaternion.Euler(new Vector3(90,0,0));


        m_rayDistance = 1000;


    }

    private void ForUpdate()
    {
        if (IsLoadProject())
        {
            ProjectorMove();
        }
    }

    private bool IsLoadProject()
    {
        if (Input.GetKeyDown(m_key))
        {
            LoadProject();
        }

        if (Input.GetKeyUp(m_key))
        {
            UnloadProjector();
        }
        return m_isLoad;
    }

    private void LoadProject()
    {
        if (!m_isLoad)
        {
            m_projectorObject = m_go;
            m_projectorTrans = m_projectorObject.transform;
            m_isLoad = true;
        }
    }

    private void UnloadProjector()
    {
        if (m_isLoad)
        {
            m_projectorTrans.position = m_position;
            m_projectorObject = null;
            m_projectorTrans = null;
            m_isLoad = false;
        }
    }

    private void ProjectorMove()
    {
        if (m_isLoad)
        {
            Vector3 point = GetRayPoint();

            if (point != Vector3.zero)
            {
                point += Vector3.up * m_projectorHight;
                m_projectorTrans.position = point;
            }

        }
    }

    private Vector3 GetRayPoint()
    {
        if (m_camrea == null)
            m_camrea = Camera.main;

        m_ray = m_camrea.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(m_ray.origin, m_ray.direction * m_rayDistance);

        if (Physics.Raycast(m_ray, out m_rayHit, m_rayDistance))
        {
            Vector3 upNom = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(upNom, m_rayHit.normal)) > 0.2f)
            {
                return m_rayHit.point;
            }

            return Vector3.zero;
        }

        return Vector3.zero;
    }

}

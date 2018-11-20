using UnityEngine;
using System.Collections;

public class RenderTexTest : MonoBehaviour {


	private GameObject m_camObj;
	private Camera m_cma;
	private Material m_mat;
	private RenderTexture m_RT;
	private Renderer[] m_renders;
	void Start () {
		CreateCameraAndSetTexture();
	}

	void Update () {

	}

	private void CreateCameraAndSetTexture()
	{
		m_camObj = new GameObject("RenderTexCam",typeof(Camera));

		m_camObj.transform.position = Camera.main.gameObject.transform.position;
		m_camObj.transform.rotation = Camera.main.gameObject.transform.rotation;

		m_cma = m_camObj.GetComponent<Camera>();
		m_cma.fieldOfView = Camera.main.fieldOfView;
		m_cma.clearFlags = CameraClearFlags.Depth;
		m_cma.depth = 1;
		m_cma.cullingMask = (1 << 0 | 2 << 0 | 3 << 0);

		m_RT = new RenderTexture(Screen.width,Screen.height,24);
		m_RT.wrapMode = TextureWrapMode.Clamp;
		m_RT.filterMode = FilterMode.Bilinear;
		m_RT.isPowerOfTwo = false;
		m_cma.targetTexture = m_RT;
		m_renders = gameObject.GetComponentsInChildren<Renderer>();

		foreach(Renderer val in m_renders)
		{
			m_mat = val.material;
			if(m_mat.shader.name == "MyMobile/HeatDistortAndDissolve1")
				m_mat.SetTexture("_RenderTex",m_RT);
		}
	}
}

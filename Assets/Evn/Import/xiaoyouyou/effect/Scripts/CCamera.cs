using UnityEngine;
using System.Collections;

public class CCamera {

	private GameObject m_camObj;
	private static CCamera m_instance;

	public CCamera()
	{
		m_camObj = Camera.main.gameObject;
	}

	public static CCamera GetInst()
	{
		if(m_instance == null)
			m_instance = new CCamera();
		return m_instance;
	}

	public GameObject GetCameraObj()
	{
		return m_camObj;
	}
}

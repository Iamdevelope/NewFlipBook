using UnityEngine;
using System.Collections;

public class ShowValues : MonoBehaviour {

	// Use this for initialization
    public GameObject m_go;
    private SkinnedMeshRenderer m_smr;
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (m_smr == null && m_go != null)
        {
            m_smr = m_go.GetComponentInChildren<SkinnedMeshRenderer>();
        }
        if (m_smr != null)
        {
            if (m_smr.material.shader.name == "Custom/XRayAndBackLightSufaces")
                WindowContral(0);
        }
	}


    private void WindowContral(int id)
    {
        GUILayout.Label("    Light(1-2) = " + GetValues("_Light"));
        GUILayout.Label("    RimLineRange(0.5-10) = " + GetValues("_RimPower"));
        GUILayout.Label("    RimLinePower(1-20) = " + GetValues("_LightRim"));
        GUILayout.Label("    XRayLineRange(0.5-10) = " + GetValues("_BlendVal"));
        GUILayout.Label("    sXRayLinePower(1-20) = " + GetValues("_BlendPower"));
        
    }

    private string GetValues(string name)
    {
        float val = m_smr.material.GetFloat(name);
        return val.ToString("F2");
    }
}

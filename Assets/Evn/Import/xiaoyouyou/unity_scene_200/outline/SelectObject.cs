using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectObject : MonoBehaviour {
	
	private Color mMainColor = new Color();

	// Use this for initialization
	void Start () {
		SkinnedMeshRenderer[] renderList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach(SkinnedMeshRenderer smr in renderList)
		{
			foreach(Material mat in smr.materials)
			{
				mMainColor	= mat.color;
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseEnter()
	{
        SkinnedMeshRenderer[] renderList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		string log = string.Format("OnMouseEnter Mesh Render Count is {0}",renderList.Length);
		print(log);
		foreach(SkinnedMeshRenderer smr in renderList)
		{
			foreach(Material mat in smr.materials)
			{
				mat.color	=	new Color(1.0f,1.0f,1.0f,1.0f);
			}
		}
    }
	
	void OnMouseExit() 
	{
		SkinnedMeshRenderer[] renderList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		string log = string.Format("OnMouseExit Mesh Render Count is {0}",renderList.Length);
		print(log);
		foreach(SkinnedMeshRenderer smr in renderList)
		{
			foreach(Material mat in smr.materials)
			{
				mat.color	=	mMainColor;
			}
		}
	}
	
	
}

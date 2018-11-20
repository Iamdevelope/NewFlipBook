using UnityEngine;
using UnityEditor;
using System.Collections;

public class RenderCubemapWizard : ScriptableWizard
{
	public Transform renderFromPosition;
	public Cubemap cubemap;




	void OnWizardCreate()
	{

		if ((renderFromPosition != null) && (cubemap != null)){
		// create temporary camera for rendering
		GameObject go = new GameObject("CubemapCamera");
		go.AddComponent<Camera>();
		// place it on the object
		go.transform.position = renderFromPosition.position;
		go.transform.rotation = Quaternion.identity;
		// render into cubemap
		go.GetComponent<Camera>().RenderToCubemap(cubemap);

		// destroy temporary camera
		DestroyImmediate(go);
		}
	}

	[MenuItem("Cubemap Maker/Create CubeMap")]
	static void RenderCubemap()
	{
		ScriptableWizard.DisplayWizard<RenderCubemapWizard>(
			"Render cubemap", "Render!");
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class APOLLOUVTillingEditor : MonoBehaviour {

	[HeaderAttribute ("GLOBAL UV TILLER")]
	public bool AllFollow = false;
	public float GlobalX = 1;
	public float GlobalY = 1;

	[HeaderAttribute (".")]

	[HeaderAttribute ("MAIN TEXTURE")]
	public bool MTFollowGlobalUV = false;

	public float MainTextureX = 1;
	public float MainTextureY = 1;

	[HeaderAttribute ("SPECULARITY MAP")]
	public bool SMFollowGlobalUV = false;
	public float SpeTextureX = 1;
	public float SpeTextureY = 1;

	[HeaderAttribute ("METALIC MAP")]
	public bool MMFollowGlobalUV = false;
	public float MetTextureX = 1;
	public float MetTextureY = 1;


	[HeaderAttribute ("AO MAP")]
	public bool AOMFollowGlobalUV = false;
	public float AOTextureX = 1;
	public float AOTextureY = 1;

	[HeaderAttribute ("BUMP MAP")]
	public bool BMFollowGlobalUV = false;
	public float BTextureX = 1;
	public float BTextureY = 1;

	[HeaderAttribute ("EMISSION MAP")]
	public bool EMFollowGlobalUV = false;
	public float EmTextureX = 1;
	public float EmTextureY = 1;


	[HeaderAttribute ("PARRALLAX MAP")]
	public bool PMFollowGlobalUV = false;
	public float ParTextureX = 1;
	public float ParTextureY = 1;

	[HeaderAttribute (".")]

	public Renderer ObjRenderer;

	void Start() {
		ObjRenderer = gameObject.GetComponent<Renderer>();
	}

	void Update() {

		//MainTexture
		float scaleX = MainTextureX;
		float scaleY = MainTextureY;


		//Specular Texture
		float scaleX2 = SpeTextureX;
		float scaleY2 = SpeTextureY;

		//Metal Texture
		float scaleX3 = MetTextureX; 
		float scaleY3 = MetTextureY;

		//AO Texture
		float scaleX4 = AOTextureX; 
		float scaleY4 = AOTextureY;

		//Bump Texture
		float scaleX5 = BTextureX;
		float scaleY5 = BTextureY;

		//Emission Texture
		float scaleX6 = EmTextureX;
		float scaleY6 = EmTextureY;

		//Parrallax Texture
		float scaleX7 = ParTextureX;
		float scaleY7 = ParTextureY;





		ObjRenderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY)); 
		ObjRenderer.sharedMaterial.SetTextureScale("_SpeTex", new Vector2(scaleX2, scaleY2)); 
		ObjRenderer.sharedMaterial.SetTextureScale("_MetTex", new Vector2(scaleX3, scaleY3)); 
		ObjRenderer.sharedMaterial.SetTextureScale("_AOTex", new Vector2(scaleX4, scaleY4)); 
		ObjRenderer.sharedMaterial.SetTextureScale("_BumpMap", new Vector2(scaleX5, scaleY5)); 
		ObjRenderer.sharedMaterial.SetTextureScale("_EmTex", new Vector2(scaleX6, scaleY6));
		ObjRenderer.sharedMaterial.SetTextureScale("_ParallaxMap", new Vector2(scaleX7, scaleY7));


		if (MTFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_MainTex", new Vector2 (GlobalX, GlobalY)); 
		}

		if (SMFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_SpeTex", new Vector2 (GlobalX, GlobalY)); 
		}

		if (MMFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_MetTex", new Vector2 (GlobalX, GlobalY)); 
		}

		if (AOMFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_AOTex", new Vector2 (GlobalX, GlobalY)); 
		}

		if (BMFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_BumpMap", new Vector2 (GlobalX, GlobalY)); 
		}

		if (EMFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_EmTex", new Vector2 (GlobalX, GlobalY)); 
		}
		if (PMFollowGlobalUV == true) {
			ObjRenderer.sharedMaterial.SetTextureScale ("_ParallaxMap", new Vector2 (GlobalX, GlobalY)); 
		}
	


		//Make all follow
		if (AllFollow  == true){

			ObjRenderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(GlobalX, GlobalY)); 
			ObjRenderer.sharedMaterial.SetTextureScale("_SpeTex", new Vector2(GlobalX, GlobalY)); 
			ObjRenderer.sharedMaterial.SetTextureScale("_MetTex", new Vector2(GlobalX, GlobalY)); 
			ObjRenderer.sharedMaterial.SetTextureScale("_AOTex", new Vector2(GlobalX, GlobalY)); 
			ObjRenderer.sharedMaterial.SetTextureScale("_BumpMap", new Vector2(GlobalX, GlobalY)); 
			ObjRenderer.sharedMaterial.SetTextureScale("_EmTex", new Vector2(GlobalX, GlobalY));
			ObjRenderer.sharedMaterial.SetTextureScale("_ParallaxMap", new Vector2(GlobalX, GlobalY));		
		}

	}


}

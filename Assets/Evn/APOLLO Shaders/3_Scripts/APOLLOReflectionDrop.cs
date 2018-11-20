using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu ("APOLLO/ReflectionDrop")]

[ExecuteInEditMode]


public class APOLLOReflectionDrop : MonoBehaviour {





	public enum QualityMode2 
	{
		CS_1 = 1,
		CS_32 = 32,
		CS_64 = 64,
		CS_128 = 128,
		CS_256 = 256,          
		CS_512 = 512,
		CS_1024 = 1024,
	};


	public   QualityMode2  CubemapQuality = QualityMode2.CS_512;


	[Header ("Mobile Settings")]
	public bool Mobile = false;


	[Header ("ClipPlane Settings")]
	public bool ViewClipPlaneDistance = true;
	public float ClipPlaneDistance  = 50;



	[Header ("Generate Cubemap Settings")]

	public bool LateUpdateCubmap = false;
	public bool UpdateOnTriggerEnter = false;
	public bool UpdateOnTriggerExit = false;
	public bool UpdateOnTriggerStay = false;
	public string TriggerTag ;

	[Header ("Cubemap Render Settings")]
	public bool oneFacePerFrame = false;
	public bool SharedMaterial = false;


	[Header ("Need Help?")]
	public bool Yes = false;


	private Camera cam;
	private int AntSet;
	private RenderTexture rtex;



	public void Start() {
	
		StartCoroutine (BakeAfter());
	}

	IEnumerator BakeAfter() {
		yield return new WaitForSeconds(0.1F);
		UpdateCubemap( 63 );    

	}




	public void BakeCubeMaps(){

		//yield WaitForSeconds (0.1);
		UpdateCubemap( 63 );   
	}


	
	// Update is called once per frame
	public void Update (){

		//if the game is for mobile don't allow the following settings

		if (Mobile == true) {

			oneFacePerFrame = true;
		}
	 

		//Don't waste your rendering time!
		if (UpdateOnTriggerStay == true){
			UpdateOnTriggerEnter = false;
			UpdateOnTriggerExit = false;
		}
		if (UpdateOnTriggerEnter == true ){
			oneFacePerFrame = false;
			UpdateOnTriggerStay = false;
		}

		if (UpdateOnTriggerExit == true ){
			oneFacePerFrame = false;
			UpdateOnTriggerStay = false;
		}

		if (UpdateOnTriggerExit == true ){
			oneFacePerFrame = false;
			UpdateOnTriggerStay = false;
		}


	}









	//Update Reflection Drop On Trigger Enter
	public void OnTriggerEnter (Collider other){
		if (UpdateOnTriggerEnter == true){
			if(other.gameObject.tag == TriggerTag){
				UpdateCubemap( 63 );

			}
		}
	}

	//Update Reflection Drop On Trigger Exit
	public void OnTriggerExit (Collider other){
		if (UpdateOnTriggerExit == true){
			if(other.gameObject.tag == TriggerTag){ 
				UpdateCubemap( 63 );

			}
		}
	}

	//Update Reflection Drop On Trigger Stay
	public void OnTriggerStay (Collider other){
		if (UpdateOnTriggerStay == true){

			if(other.gameObject.tag == TriggerTag){ 
				UpdateCubemap( 63 );

			}
		}
	}



	//RealTime Reflection Drop
	public void LateUpdate(){
		if (LateUpdateCubmap) {


			UpdateOnTriggerExit = false;
			UpdateOnTriggerEnter = false;
			UpdateOnTriggerStay = false;


			if (oneFacePerFrame) {
				var faceToRender = Time.frameCount % 6;
				var faceMask = 1 << faceToRender;
				UpdateCubemap (faceMask);
			} else {
				UpdateCubemap( 63 );
			}
		}
	}

	//Generate CubeMap
	public void GenerateCubeMap () {
		
			UpdateCubemap (63); // all six faces
		}




	public void UpdateCubemap (int faceMask) {
		AntSet =   QualitySettings.antiAliasing ;

		QualitySettings.antiAliasing = 0;



		if (!cam) {
			GameObject go = new GameObject ("CubemapCamera",  typeof(Camera));
			go.hideFlags = HideFlags.HideAndDontSave;
			go.transform.position = transform.position;
			go.transform.rotation = Quaternion.identity;
			cam = go.GetComponent<Camera>();
			cam.farClipPlane = ClipPlaneDistance; // Set the rendering distance
			cam.enabled = false;


		
		}

		if (!rtex) {



			rtex = new RenderTexture ((int)CubemapQuality, (int)CubemapQuality,16); 
			rtex.dimension = UnityEngine.Rendering.TextureDimension.Cube;
			rtex.useMipMap = true;
			rtex.hideFlags = HideFlags.HideAndDontSave;


			//Activate Material Instances
			if(SharedMaterial ==true) {

				if(Application.isPlaying==true){
					GetComponent<Renderer>().material.SetTexture ("_Cube", rtex);
				}

				if(Application.isPlaying==false){
					GetComponent<Renderer>().sharedMaterial.SetTexture ("_Cube", rtex);
				}
			}

			if(SharedMaterial ==false) {
				GetComponent<Renderer>().sharedMaterial.SetTexture ("_Cube", rtex);
			}

		}

		cam.transform.position = transform.position;
		cam.RenderToCubemap (rtex, faceMask);

		QualitySettings.antiAliasing = AntSet;

		DestroyImmediate (cam);

	}

	public void OnDisable () {
		DestroyImmediate (cam);
		DestroyImmediate (rtex);
	}

	public void OnDrawGizmosSelected () {

		if(ViewClipPlaneDistance == true){
			// Draw a yellow cube at the transforms position
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube (transform.position, new Vector3 (ClipPlaneDistance,ClipPlaneDistance,ClipPlaneDistance));
		}
	}










}

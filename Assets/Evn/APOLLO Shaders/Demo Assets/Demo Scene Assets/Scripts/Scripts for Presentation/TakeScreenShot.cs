using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class APOLLOTakeScreenShot : MonoBehaviour {


	public GameObject FSL;

	void LateUpdate () {

		if (Input.GetKeyDown ("space")){

			FSL.SetActive(true);

			Wait();

			TakeScreenshot();

			AudioSource	audio = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
			audio.Play();
		}
	}



	void TakeScreenshot (){

		StartCoroutine (WaitFrame ());
		// We should only read the screen buffer after rendering is complete
		StartCoroutine (WaitForSeconds ());

		// Create a texture the size of the screen, RGB24 format
		int  width = Screen.width ;
		int height = Screen.height;

		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);
		// Read screen contents into the texture
		tex.ReadPixels (new Rect(0, 0, width, height), 0, 0);
		tex.Apply ();

		// Encode texture into PNG
		var bytes = tex.EncodeToPNG();
		// Destroy (tex);

		// For testing purposes, also write to a file in the project folder
		File.WriteAllBytes("Assets/APOLLO Shaders/4_APOLLO Renderings/My Rendering.png", bytes);

	}

	void Wait(){

		StartCoroutine (WaitForSeconds2 ());
		FSL.SetActive(false);
	}


	IEnumerator WaitFrame() {
		yield return new WaitForEndOfFrame();
	}

	IEnumerator WaitForSeconds() {
		yield return new WaitForSeconds (0.02f);
	}

	IEnumerator WaitForSeconds2() {
		yield return new WaitForSeconds (0.01f);
	}

}

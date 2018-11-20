using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




[CustomEditor (typeof(APOLLOReflectionDrop)), CanEditMultipleObjects]


public class ReflectionDropEditor : Editor {

	private bool UpdateMyCubmap = true;



	public override void OnInspectorGUI() 
	{ 
		APOLLOReflectionDrop myScript = target as APOLLOReflectionDrop;

	

		if (myScript.Yes == true){

			EditorGUILayout.HelpBox (" Sure! Feel free to contact me though my official Facebook page (https://www.facebook.com/rispat.momit/) :D ", MessageType.Info);
		}

		DrawDefaultInspector();

	}

	void Update(){ 


		APOLLOReflectionDrop myScript = target as APOLLOReflectionDrop;

		if(EditorApplication.isPlaying ==false) {

			if (UpdateMyCubmap == true) {

				if (Lightmapping.isRunning == false){
					myScript.GenerateCubeMap();
					UpdateMyCubmap = false;
				}
			}
			if (Lightmapping.isRunning == true){
				UpdateMyCubmap = true;
			}
		}





	}
}

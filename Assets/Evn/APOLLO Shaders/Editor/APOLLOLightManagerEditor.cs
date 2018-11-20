using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor (typeof(APOLLOLightManager)), CanEditMultipleObjects]





public class LightManagerEditor : Editor {

	public override void OnInspectorGUI() 
	{ 

		APOLLOLightManager myScript2   = target as APOLLOLightManager; 


		EditorGUILayout.HelpBox ("Light Manager is an essential tool of APOLLO shaders that let you have full control over your scene lighting.", MessageType.Info);
		GUILayout.Space(5);
		EditorGUILayout.HelpBox ("Be sure you have only one LightManager script in your scene.", MessageType.Info);
		GUILayout.Space(5);
		//GUILayout.Box (" ");

		if (myScript2.INeedHelpWithThis == true){ 


			EditorGUILayout.HelpBox ("Add a Global reflection for all of your scene objects (This will not affect the Ambient Intencity of your objects)", MessageType.Info); 
			EditorGUILayout.HelpBox ("Add a main Light to the Sun property (In case you want your Ambient Intecsity to be controled by your Directional Light Intencity)", MessageType.Info); 
			EditorGUILayout.HelpBox ("Use Lighting Prefab if you can't figure out a good lighting for your scene ", MessageType.Info); 
			EditorGUILayout.HelpBox ("Use Ambient Contrast for a more contrasty Light Mapping (It may create negative values when used with Post processing Effects)", MessageType.Info); 
			EditorGUILayout.HelpBox ("Increase Light Add to make the lighting more thick", MessageType.Info); 
			EditorGUILayout.HelpBox ("Use Ambient Intesity to control the Ambient Light of your scene", MessageType.Info); 

			EditorGUILayout.HelpBox ("Need more help? Don't worry, You can contact me though my official Facebook page (https://www.facebook.com/rispat.momit/) :D ", MessageType.Warning); 
		}


if (myScript2.AmbientContrast > 1){ 

			EditorGUILayout.HelpBox ("Increasing the Ambient Contrast over this point it MAY create negative texture results when combined with Color Corection Image Effects", MessageType.Warning); 
}


		DrawDefaultInspector();

	}
}

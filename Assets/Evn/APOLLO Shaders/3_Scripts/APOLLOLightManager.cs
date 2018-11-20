using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[AddComponentMenu ("APOLLO/LightManager")]

public class APOLLOLightManager : MonoBehaviour {


	[Header (". ")]
	public Cubemap GlobalSceneReflection;
	public Light Sun;

	[Header (". ")]

	public bool LightingPrefab = false;

	public bool SunManageAmbientisLight = false;

	[Header (". ")]
	public bool CalculateAmbientContrast = false;


	[Range(0F, 5F)]
	public float AmbientContrast = 0.5F;


	[Header (". ")]
	[Range(0F, 0.5F)]
	public float LightAdd = 0.05F;


	[Header (". ")]
	[Range(0F, 4F)]
	public float AmbientIntensity = 1F;

	[Header (". ")]
	public bool INeedHelpWithThis = false;


	
	// Update is called once per frame
	void Update () {

		if (LightingPrefab == true){

			AmbientContrast = 0.5F;
			LightAdd = 0.05F;
			AmbientIntensity = 1F;

		}


		if (Sun == null) {
			
			SunManageAmbientisLight = false;
		}
			else{
			if (SunManageAmbientisLight == true) {
			
				AmbientIntensity = Sun.intensity;

			}
		}


	if (CalculateAmbientContrast == false) {
			Shader.SetGlobalFloat("_Con", 0F);
	}

		if (CalculateAmbientContrast == true) {
				Shader.SetGlobalFloat("_Con", AmbientContrast);

		}

		Shader.SetGlobalTexture("_Cube", GlobalSceneReflection);


		RenderSettings.ambientIntensity = AmbientIntensity;
		Shader.SetGlobalFloat("_LightAdd", LightAdd);


	}

}
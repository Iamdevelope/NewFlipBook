using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour {

    private Material skyMaterial;
    private float num;
    private void Start()
    {
        skyMaterial = RenderSettings.skybox;
        StartCoroutine(SkyboxSelfRotation());
    }

    private IEnumerator SkyboxSelfRotation()
    {
        yield return new WaitForEndOfFrame();
        num = skyMaterial.GetFloat("_Rotation");
        skyMaterial.SetFloat("_Rotation", num + 0.05f);
        StartCoroutine(SkyboxSelfRotation());
    }
}

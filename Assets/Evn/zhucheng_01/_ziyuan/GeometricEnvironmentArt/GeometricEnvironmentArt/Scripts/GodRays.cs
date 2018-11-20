using UnityEngine;
using System.Collections;

/// <summary>
/// Animating God rays.
/// Place this script on a quad with the GodRays material. 
/// Handles:
/// 	Rotates the quad to face the view (but keeping the tilt angle intact)
/// 	Fades out as the camera approaches
/// 	Animates the god-rays over time.
/// </summary>
public class GodRays : MonoBehaviour 
{
	[Range(0.0f, 10.0f)]
	public float animSpeed = 3.0f;				// speed the god-rays animate at.

	public float fadeDistanceStart = 10.0f;		// distance from the camera at which it will start to fade out
	public float fadeDistanceEnd = 5.0f;		// distance from the camera at which it is fully transparent

	private Material mat;
	private int fadeId;
	private int yOffsetId;
	private float yOffset = 0.0f;
	
	void Start()
	{
		mat = GetComponent<Renderer>().material;

		fadeId = Shader.PropertyToID("_Fade");
		yOffsetId = Shader.PropertyToID("_YOffset");
	}

	void Update () 
	{
		Transform camXfm = Camera.main.transform;
		Vector3 camPos = camXfm.position;
		Vector3 mPos = transform.position;
		Vector3 up = transform.up;

		// find the closest point to the camera on the line along the godrays (that is our local up vector)
		Vector3 closestPoint = mPos + up * Vector3.Dot(camPos - mPos, up);
		Vector3 delta = closestPoint - camPos;

		// rotate to face the camera (but keeping our original up vector)
		transform.rotation = Quaternion.LookRotation(delta, transform.up);

		// fade out when the camera gets close
		float dist = delta.magnitude;
		float alpha = Mathf.Clamp01((dist - fadeDistanceEnd) / (fadeDistanceStart - fadeDistanceEnd));
		mat.SetFloat(fadeId, 1.0f - alpha);

		// scroll the texture to animate the rays
		yOffset += 0.01f * Time.deltaTime * animSpeed;
		yOffset = Mathf.Repeat(yOffset, 1.0f);
		mat.SetFloat(yOffsetId, yOffset);
	}
}

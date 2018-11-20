using UnityEngine;
using System.Collections;

/// <summary>
/// Really simple class to control the demo cameras. Not intended for real-world usage.
/// </summary>
public class DemoCamera : MonoBehaviour 
{
	[Tooltip("Local space motion direction")]
	public Vector3 motionDir = Vector3.forward;
	[Tooltip("Speed the camera should move at in units per second")]
	public float speed = 1.0f;
	[Tooltip("Time this camera should be active before moving to the next")]
	public float time = 5.0f;
	[Tooltip("Camera to activate after this one")]
	public DemoCamera nextCamera;

	private Vector3 startPos;
	private float timer;

	void Start()
	{
		timer = 0.0f;
		startPos = transform.position;
	}

	void Update () 
	{
		timer += Time.deltaTime;
		transform.Translate(motionDir * speed * Time.deltaTime, Space.Self);

		if(timer > time && nextCamera != null)
		{
			gameObject.SetActive(false);
			nextCamera.gameObject.SetActive(true);

			timer = 0.0f;
			transform.position = startPos;
		}
	}
}

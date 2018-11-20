
using UnityEngine;
using System.Collections;

/// <summary>
/// Really simple script to rotate an object at a constant speed
/// </summary>
public class Rotator : MonoBehaviour 
{
	public float rpm = 10.0f;				// revolutions per minute
	public Vector3 axis = Vector3.right;	// axis to rotate around

	void Update () 
	{
		float degreesPerSecond = (rpm * 360.0f / 60.0f);
		float angle = degreesPerSecond * Time.deltaTime;
		transform.Rotate(axis, angle);
	}
}

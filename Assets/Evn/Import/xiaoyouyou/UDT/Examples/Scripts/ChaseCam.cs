using UnityEngine;
using System.Collections;

public class ChaseCam : MonoBehaviour
{
	public Transform Target;
	public float Distance = 10.0f;
	public float Height = 10.0f;
	
	// Use this for initialization
	void Start ()
	{
		if(!Target) {
			Target = GameObject.FindGameObjectWithTag("Player").transform;	
		}
	}
	
	private void LateUpdate() {
		if(!Target) {
			return;
		}
		
		transform.position = Target.position;
		transform.rotation = Target.rotation;
		
		var pos = transform.position;
		
		pos.z -= Distance;
		pos.y += Height;
		
		transform.position = pos;
		
		transform.LookAt(Target);
	}
}


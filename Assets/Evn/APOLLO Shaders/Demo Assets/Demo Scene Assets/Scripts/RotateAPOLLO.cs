using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAPOLLO : MonoBehaviour {

	public float Speed =2;

	
	// Update is called once per frame
	void Update () {



			transform.Rotate(Vector3.left * Time.deltaTime*Speed, Space.World);

	}
}

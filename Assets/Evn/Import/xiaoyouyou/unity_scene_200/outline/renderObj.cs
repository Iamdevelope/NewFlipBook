using UnityEngine;
using System.Collections;

public class renderObj : MonoBehaviour {
	
	public RenderTexture RTT;

	// Use this for initialization
	void Start () {
		RTT.format	= RenderTextureFormat.Depth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

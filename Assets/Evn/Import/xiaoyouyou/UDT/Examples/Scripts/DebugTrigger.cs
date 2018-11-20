using UnityEngine;
using System.Collections;

public class DebugTrigger : MonoBehaviour {
	
	public string MatchTag = "Player";
	
	private float _frequency = 1.0f;
	private float _lastTick = 0.0f;

	private void OnTriggerEnter(Collider other) {
		Debug.Log("Trigger zone entered by: " + other.name);	
	}
	
	private void OnTriggerStay(Collider other) {
		
		if(Time.time - _lastTick > _frequency) {
			_lastTick = Time.time;
			Debug.Log("Trigger zone stay triggered by: " + other.name);
		}
	}
	
	private void OnTriggerExit(Collider other) {
		Debug.Log("Trigger zone exited by: " + other.name);	
	}
}

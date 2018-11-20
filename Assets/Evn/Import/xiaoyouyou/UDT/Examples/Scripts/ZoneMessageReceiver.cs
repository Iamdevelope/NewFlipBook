using UnityEngine;
using System.Collections;

public class ZoneMessageReceiver : MonoBehaviour {
	/// <summary>
	/// SendMessage receiver OnZoneEnter
	/// </summary>
	/// <param name='other'>
	/// The other collider that caused the trigger event.
	/// </param>
	private void OnZoneEnter(Collider other) {
		Debug.Log("Enter Message Received: Triggered by " + other.gameObject.name);
	}
	
	/// <summary>
	/// SendMessage receiver OnZoneStay
	/// </summary>
	/// <param name='other'>
	/// The other collider that caused the trigger event.
	/// </param>
	private void OnZoneStay(Collider other) {
		Debug.Log("Stay Message Received: Triggered by " + other.gameObject.name);
	}
	
	/// <summary>
	/// SendMessage receiver OnZoneExit
	/// </summary>
	/// <param name='other'>
	/// The other collider that caused the trigger event.
	/// </param>
	private void OnZoneExit(Collider other) {
		Debug.Log("Exit Message Received: Triggered by " + other.gameObject.name);
	}
}
using UnityEngine;
using Nss.Udt.Referee;

/// <summary>
/// Super cube component
/// 
/// This component is referee'd by SuperCubeReferee.
/// The base class will force you to implement OnEnable and OnDisable.
/// </summary>
public class SuperCube : RefereeBehaviour {
	
	/// <summary>
	/// Unity3D OnEnable event
	/// </summary>
	protected override void OnEnable () {
		SuperCubeReferee.Instance.Add(this);	// track this component
	}
	
	/// <summary>
	/// Unity3D OnDisable event
	/// </summary>
	protected override void OnDisable () {
		SuperCubeReferee.Instance.Remove(this);	// stop tracking this component
	}
}

///// <summary>
///// Super cube vanilla component
///// 
///// This component is referee'd by SuperCubeReferee.
///// It does not force you to implement OnEnable and OnDisable
///// and will break if they are not present
///// </summary>
//public class SuperCube : MonoBehaviour {
//	
//	/// <summary>
//	/// Unity3D OnEnable event
//	/// </summary>
//	public void OnEnable() {
//		SuperCubeReferee.Instance.Add(this);	// track this component
//	}
//	
//	/// <summary>
//	/// Unity3D OnDisable event
//	/// </summary>
//	public void OnDisable() {
//		SuperCubeReferee.Instance.Remove(this);	// stop tracking this component
//	}
//}
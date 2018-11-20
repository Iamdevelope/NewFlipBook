using UnityEngine;
using System.Collections;

namespace Nss.Udt.Pooling.Examples {
	public class PoolGui : MonoBehaviour {
		
		private void OnGUI() {
			if(GUILayout.Button("Spawn super cube", GUILayout.Width(200f), GUILayout.Height(100f))) {
				var g = PoolController.Instance.GetNext("super-cube", true);
				g.transform.position = Vector3.zero;
			}
		}
	}
}
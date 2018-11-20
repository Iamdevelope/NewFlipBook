using UnityEngine;

namespace Nss.Udt.Pooling.Examples {
	public class DieOverTime : MonoBehaviour {
		
		public float delay = 3f;
		private bool preloaded = false;
	
		private void OnEnable() {
			if(!preloaded) {
				preloaded = true;
				return;
			}
			
			// Do normal enable work...
			PoolController.Instance.Destroy(gameObject, delay);
		}
		
		private void OnInstantiated() {
			Debug.Log("instantiated!");
			PoolController.Instance.Destroy(gameObject, delay);
		}
		
		private void OnDestroyed() {
			Debug.Log("removed!");
		}
	}
}
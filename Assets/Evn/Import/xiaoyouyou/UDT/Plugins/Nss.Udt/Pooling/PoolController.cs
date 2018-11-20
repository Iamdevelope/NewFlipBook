using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Nss.Udt.Pooling {
	public class PoolController : MonoBehaviour {
		public static PoolController Instance;			// Singleton access
		public const string ROOT_NAME = "__Pools__";	// Controller hierarchy object name
		
		private List<Pool> pools;						// List of all pool components
		private Hashtable poolTable;					// Lookup pool name to pool component
		
		#region Unity3D Events
		/// <summary>
		/// Unity3D OnEnable event.
		/// </summary>
		private void OnEnable() {
			Initialize();
		}

		/// <summary>
		/// Unity3D OnDisable event.
		/// </summary>
		private void OnDisable() {
			Instance = null;
			DestroyAll();
		}
		#endregion
		
		/// <summary>
		/// Gets the next available preloaded GameObject in the pool.
		/// </summary>
		/// <returns>
		/// Preloaded GameObject, activated if specified.
		/// 
		/// If the object is not found, or limiting is enabled and reached,
		/// this will return NULL.
		/// </returns>
		/// <param name='poolName'>Pool name that contains cached GameObjects.</param>
		/// <param name='activate'>Activates the returned GameObject if <c>true</c></param>
		public GameObject GetNext(string poolName, bool activate) {
			poolName = string.Format("pool-{0}", poolName);
			
			var pool = poolTable[poolName] as Pool;
			
			if(pool == null) {
				Debug.LogError(string.Format("POOL: Pool doesn't exist. [poolName: '{0}']", poolName));
				
				return null;
			}
			
			return pool.GetNext(activate);
		}
		
		/// <summary>
		/// Destroys all pools and cached GameObjects.
		/// </summary>
		public void DestroyAll() {
			for(int i=0; i < pools.Count; i++) {
				pools[i].DestroyAll();
			}
			
			pools.Clear();
			poolTable.Clear();
		}
		
		/// <summary>
		/// Destroys a specific pool and its cached GameObjects.
		/// </summary>
		/// <param name='poolName'>Pool name.</param>
		public void DestroyPool(string poolName) {
			var pool = poolTable[poolName] as Pool;
			
			if(pool == null) {
				Debug.LogError(string.Format("POOL: Pool doesn't exist. [poolName: '{0}']", poolName));
				return;
			}
			
			pool.DestroyAll();
		}
		
		/// <summary>
		/// Despawn the pooledItem after delay in seconds.
		/// This will start a coroutine.
		/// </summary>
		/// <param name='pooledItem'>Pooled GameObject to destroy.</param>
		/// <param name='delay'>Delay.</param>
		public void Destroy(GameObject pooledItem, float delay) {
			StartCoroutine(Despawn(pooledItem, delay));
		}
		
		/// <summary>
		/// Destroy the pooledItem.
		/// </summary>
		/// <param name='pooledItem'>Pooled GameObject to destroy.</param>
		public void Destroy(GameObject pooledItem) {
			var poolItem = pooledItem.GetComponent<PooledItem>();
			
			if(poolItem == null) {
				Debug.LogError(string.Format("POOL: PoolItem doesn't exist on GameObject. [GameObject: '{0}']", pooledItem));
				
				return;
			}
			
			var pool = poolTable[poolItem.sourcePool] as Pool;
			
			if(pool == null) {
				Debug.LogError(string.Format("POOL: Pool doesn't exist. [poolName: '{0}']", poolItem.sourcePool));
				
				return;
			}
			
			pool.Destroy(pooledItem);
		}
		
		#region Private Methods
		/// <summary>
		/// Initialize pool controller and all pool components.
		/// </summary>
		private void Initialize() {
			if(Instance != null) {
				Debug.LogError("POOL: There can only be one PoolController, detected multiple instances.");
				return;
			}
			
			if(pools != null && poolTable != null) {
				// rehydrating, editor recompile during playback
				Instance = this;
				return;
			}
			
			pools = new List<Pool>();
			poolTable = new Hashtable();
			var removals = new List<Pool>();
			
			pools.AddRange(GetComponentsInChildren<Pool>(true));
			
			for(int i=0; i < pools.Count; i++) {
				if(pools[i].prefab == null) {
					removals.Add(pools[i]);
					continue;
				}
				
				pools[i].name = string.Format("pool-{0}", pools[i].prefab.name);
				poolTable.Add(pools[i].name, pools[i]);

				pools[i].Initialize();
			}
			
			for(int i=0; i < removals.Count; i++) {
				pools.Remove(removals[i]);
			}
			
			removals.Clear();
			Instance = this;
		}

		/// <summary>
		/// Despawn the pooledItem after delay in seconds.
		/// </summary>
		private IEnumerator Despawn(GameObject pooledItem, float delay) {
			
			yield return new WaitForSeconds(delay);
			Destroy(pooledItem);
		}
		#endregion
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace Nss.Udt.Pooling {
	public class Pool : MonoBehaviour {
	
		public GameObject prefab;
		
		public int size = 1;
		public bool limit;
		public bool suppressLimitErrors = false;
		public int limitSize;
		public bool hideInHierarchy = true;
		public bool shrinkBack = true;
		
		private Transform poolRoot;
		private int currentIndex;
		private int initialSize;
		private List<GameObject> pooledObjects;
		
		public void Initialize() {
			initialSize = size;
			
			poolRoot = gameObject.transform;
			name = string.Format("pool-{0}", prefab.name);
			
			pooledObjects = new List<GameObject>();
			
			for(int i=0; i < size; i++) {
				var g = AddToPool();
				
				if(hideInHierarchy) {
					g.hideFlags = HideFlags.HideInHierarchy;
				}
			}
			
			currentIndex = 0;
		}
		
		public GameObject GetNext(bool activate) {
			var next = GetNextAvailable();
			
			if(next == null) {
				next = IncreasePool();
			}
			
			if(next == null && !suppressLimitErrors) {
				Debug.LogError("POOL: Unable to increase pool size.  All instances used and limit reached.");
				return null;
			}
			
			// If we're here, then we're good to go
			next.SetActive(activate);
			next.SendMessage("OnInstantiated", SendMessageOptions.DontRequireReceiver);
			
			return next;
		}
		
		public void Destroy(GameObject pooledItem) {
			pooledItem.SendMessage("OnDestroyed", SendMessageOptions.DontRequireReceiver);
			pooledItem.SetActive(false);
		}
		
		public void DestroyAll() {
			if(pooledObjects != null) {
				for(int i=0; i < pooledObjects.Count; i++) {
					GameObject.Destroy(pooledObjects[i]);
				}
				
				pooledObjects.Clear();
			}
		}
		
		private void Update() {
			if(shrinkBack) {
				Shrink();
			}
		}
		
		private void Shrink() {
			int delta = size - initialSize;
			
			if(delta > 0) {
				for(int i=0; i < size - 1; i++) {
					if(pooledObjects[i].activeInHierarchy) continue;
					
					GameObject.Destroy(pooledObjects[i]);
					pooledObjects.RemoveAt(i);
					size--;
					
					break;
				}
			}
		}
		
		private GameObject GetNextAvailable() {
			if(currentIndex >= size) currentIndex = 0;
			
			for(int i=currentIndex; i < size; i++) {
				if(!pooledObjects[i].activeInHierarchy) {
					currentIndex++;
					return pooledObjects[i];
				}
			}
			
			return null;
		}
		
		private GameObject IncreasePool() {
			if(limit && size >= limitSize) {
				return null;
			}
			
			var pooledItem = AddToPool();
			
			size++;
			
			return pooledItem;
		}

		private GameObject AddToPool() {
			var pooledItem = GameObject.Instantiate(prefab) as GameObject;
			pooledItem.SetActive(false);
			pooledItem.transform.parent = poolRoot;
			
			var item = pooledItem.GetComponent<PooledItem>();
			
			if(item == null) {
				item = pooledItem.AddComponent<PooledItem>();
			}
			
			item.sourcePool = name;
			pooledObjects.Add(pooledItem);
			
			return pooledItem;
		}
	}
}
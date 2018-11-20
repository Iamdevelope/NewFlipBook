using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCommon
{
    /// <summary>
    /// 可以重置
    /// </summary>
    public interface IResetable
    {
        void OnReset();
    }

    /// <summary>
    /// 游戏对象池
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, List<GameObject>> cache;

        private new void Awake()
        {
            base.Awake();

            cache = new Dictionary<string, List<GameObject>>();
        }
         
        /// <summary>
        /// 通过对象池创建对象
        /// </summary>
        /// <param name="key">对象的类别</param>
        /// <param name="prefab">对象的预制件</param>
        /// <param name="position">使用的位置</param>
        /// <param name="rotate">使用的旋转</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 position, Quaternion rotate)
        {
            GameObject go = FindUsableObject(key);
            if (go == null)
            {
                go = AddObject(key, prefab); 
            }
            UseObject(position, rotate, go);
            return go;
        }

        private void UseObject(Vector3 position, Quaternion rotate, GameObject go)
        {
            go.transform.position = position;
            go.transform.rotation = rotate;
            go.SetActive(true);
            //遍历执行 所有需要被重置的脚本
            foreach (var item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            } 
        }

        private GameObject AddObject(string key, GameObject prefab)
        {
            //创建对象
            GameObject go = Instantiate(prefab);
            //加入池中
            if (!cache.ContainsKey(key))
                cache.Add(key, new List<GameObject>());
            cache[key].Add(go);
            return go;
        }

        private GameObject FindUsableObject(string key)
        {
            return cache.ContainsKey(key) ? cache[key].Find(g => g.activeInHierarchy == false) : null;
        }
        
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go">需要被回收的对象</param>
        /// <param name="delay">延迟时间</param>
        public void CollectObject(GameObject go, float delay = 0)
        { 
            StartCoroutine(DelayCollectObject(go, delay));
        }
         
        private IEnumerator DelayCollectObject(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }
        
        public void Clear(string key)
        {
            for (int i = 0; i < cache[key].Count; i++)
            {
                Destroy(cache[key][i]);
            }
            cache.Remove(key);
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public void ClearAll()
        {
            //将所有键存入List中
            List<string> keyList =  new List<string>(cache.Keys);
            //遍历List移除字典记录
            foreach (var key in keyList)
            {
                Clear(key);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCommon{
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T">子类的类型</typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        //按需加载
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        instance = new GameObject("Singleton of " + typeof(T).ToString()).AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        protected void Awake()
        {
            if (instance == null)
                instance = this as T;
        }
    }
}
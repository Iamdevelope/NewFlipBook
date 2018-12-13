using PJW.MVC.Core;
using UnityEngine;

namespace PJW.MVC
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseMonobehiviour : MonoBehaviour
    {
        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name,object data=null)
        {
            Facade.Instance.SendNotification(name, data);
        }
        /// <summary>
        /// 找到子物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public T Find<T>(string s)
        {
            if (transform.Find(s) == null)
            {
                Debug.LogError(this + " 子对象 " + s + " 未找到 ");
                return default(T);
            }
            return transform.Find(s).GetComponent<T>();
        }
    }
}

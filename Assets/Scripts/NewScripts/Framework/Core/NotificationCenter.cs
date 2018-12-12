

using PJW.MVC.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.MVC
{
    /// <summary>
    /// 消息处理中心：移除，注册获取消息
    /// </summary>
    public class NotificationCenter
    {
        private Dictionary<string, List<IObserver>> allObserver;
        private static NotificationCenter instance;
        public static NotificationCenter Instance
        {
            get
            {
                if (instance == null) instance = new NotificationCenter();
                return instance;
            }
        }
        private NotificationCenter()
        {
            allObserver = new Dictionary<string, List<IObserver>>();
        }
        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <param name="observerName"></param>
        /// <param name="observer"></param>
        public void AddObserver(string observerName,IObserver observer)
        {
            if (!allObserver.ContainsKey(observerName))
                allObserver[observerName] = new List<IObserver>();
            allObserver[observerName].Add(observer);
        }
        /// <summary>
        /// 移除观察者
        /// </summary>
        /// <param name="observerName"></param>
        /// <param name="observer"></param>
        public void RemoveObserver(string observerName,IObserver observer)
        {
            if (!allObserver.ContainsKey(observerName)) return;
            if (!allObserver[observerName].Contains(observer)) return;
            allObserver[observerName].Remove(observer);
            if (allObserver[observerName].Count == 0)
                allObserver.Remove(observerName);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name,object data = null)
        {
            if (!allObserver.ContainsKey(name)) return;
            List<IObserver> list = allObserver[name];
            foreach (IObserver item in list)
            {
                item.HandleNotification(new Patterns.Notification(name, data));
            }
        }
        /// <summary>
        /// 查看消息的监听对象
        /// </summary>
        public void View()
        {
            string s = "----------------------------Start View----------------------\n";
            // 找到所有消息
            foreach (string name in allObserver.Keys)
            {
                s += name + " :[ ";
                List<IObserver> list = allObserver[name];
                for (int i = 0; i < list.Count; i++)
                {
                    s += list[i];
                    if (i != list.Count - 1) s += " , ";
                }
                s += " ]\n";
            }
            s += "----------------------------End View----------------------\n\n\n";
            Debug.Log(s);
        }
    }
}
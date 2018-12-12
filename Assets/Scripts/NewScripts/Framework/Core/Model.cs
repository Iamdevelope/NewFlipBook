

using PJW.MVC.Interface;
using System.Collections.Generic;

namespace PJW.MVC.Core
{
    /// <summary>
    /// 管理Proxy
    /// </summary>
    public class Model : IModel
    {
        private Dictionary<string, IProxy> allProxy;
        private static Model instance;
        public static Model Instance
        {
            get
            {
                if (instance == null) instance = new Model();
                return instance;
            }
        }
        private Model()
        {
            allProxy = new Dictionary<string, IProxy>();
        }
        /// <summary>
        /// 获取Proxy
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns></returns>
        public IProxy GetProxy(string proxyName)
        {
            return allProxy.ContainsKey(proxyName) ? allProxy[proxyName] : null;
        }
        /// <summary>
        /// 注册Proxy
        /// </summary>
        /// <param name="proxy"></param>
        public void RegisterProxy(IProxy proxy)
        {
            allProxy[proxy.ProxyName] = proxy;
        }
        /// <summary>
        /// 移除Proxy
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns></returns>
        public IProxy RemoveProxy(string proxyName)
        {
            IProxy proxy= allProxy.ContainsKey(proxyName) ? allProxy[proxyName] : null;
            if (proxy != null) allProxy.Remove(proxyName);
            return proxy;
        }
    }
}
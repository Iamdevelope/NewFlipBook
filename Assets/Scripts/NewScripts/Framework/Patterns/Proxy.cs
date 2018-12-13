

using PJW.MVC.Core;
using PJW.MVC.Interface;

namespace PJW.MVC.Patterns
{
    /// <summary>
    /// 处理数据
    /// </summary>
    public class Proxy : IProxy
    {
        public const string NAME = "Proxy";
        public Proxy()
        {
            ProxyName = NAME;
        }
        public string ProxyName { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name, object data = null)
        {
            Facade.Instance.SendNotification(name, data);
        }
    }
}
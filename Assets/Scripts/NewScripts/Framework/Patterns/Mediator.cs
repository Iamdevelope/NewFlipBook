

using PJW.MVC.Core;
using PJW.MVC.Interface;

namespace PJW.MVC.Patterns
{

    public class Mediator : IMediator
    {
        public const string NAME = "Mediator";
        public Mediator()
        {
            MediatorName = NAME;
        }
        public string MediatorName
        {
            get;set;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="notification"></param>
        public virtual void HandleNotification(Notification notification)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <returns></returns>
        public virtual string[] NotificationList()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name, object data)
        {
            Facade.Instance.SendNotification(name, data);
        }
    }
}
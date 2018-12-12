
using PJW.MVC.Patterns;

namespace PJW.MVC.Interface
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="notification"></param>
        void HandleNotification(Notification notification);
    }
}
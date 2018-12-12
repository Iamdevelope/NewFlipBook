
namespace PJW.MVC.Interface
{
    /// <summary>
    /// UI和数据之间的桥梁，监听和处理消息
    /// </summary>
    public interface IMediator:IObserver,INotifier
    {
        /// <summary>
        /// 名称
        /// </summary>
        string MediatorName { get; set; }
        /// <summary>
        /// 监听消息的数组
        /// </summary>
        /// <returns></returns>
        string[] NotificationList();
    }
}
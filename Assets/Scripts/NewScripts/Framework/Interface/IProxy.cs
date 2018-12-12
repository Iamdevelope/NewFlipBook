

namespace PJW.MVC.Interface
{
    /// <summary>
    /// 处理数据接口
    /// </summary>
    public interface IProxy:INotifier
    {
        /// <summary>
        /// proxy的名称
        /// </summary>
        string ProxyName { get; set; }
    }
}
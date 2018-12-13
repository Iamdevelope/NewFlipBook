using PJW.MVC.Interface;

namespace PJW.MVC.Core
{
    /// <summary>
    /// 实现所有的IView和IProxy
    /// </summary>
    public class Facade : IFacade
    {
        #region 单例
        protected static Facade instance;
        public static Facade Instance { get
            {
                if (instance == null) instance = new Facade();
                return instance;
            } }
        #endregion
        /// <summary>
        /// 获取Mediator
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public IMediator GetMediator(string mediatorName)
        {
            return View.Instance.GetMediator(mediatorName);
        }
        /// <summary>
        /// 注册Mediator
        /// </summary>
        /// <param name="mediator"></param>
        public void RegisterMediator(IMediator mediator)
        {
            View.Instance.RegisterMediator(mediator);
        }
        /// <summary>
        /// 移除Mediator
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public IMediator RemoveMediator(string mediatorName)
        {
            return View.Instance.RemoveMediator(mediatorName);
        }
        /// <summary>
        /// 获取Proxy
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns></returns>
        public IProxy GetProxy(string proxyName)
        {
            return Model.Instance.GetProxy(proxyName);
        }
        /// <summary>
        /// 注册Proxy
        /// </summary>
        /// <param name="proxy"></param>
        public void RegisterProxy(IProxy proxy)
        {
            Model.Instance.RegisterProxy(proxy);
        }
        /// <summary>
        /// 移除Proxy
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns></returns>
        public IProxy RemoveProxy(string proxyName)
        {
            return Model.Instance.RemoveProxy(proxyName);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name, object data = null)
        {
            NotificationCenter.Instance.SendNotification(name, data);
        }
    }
}
using PJW.MVC.Interface;
using System.Collections.Generic;

namespace PJW.MVC.Core
{
    /// <summary>
    /// 管理Mediator
    /// </summary>
    public class View : IView
    {
        private Dictionary<string, IMediator> allMediator;
        protected static View instance;
        public static View Instance
        {
            get
            {
                if (instance == null) instance = new View();
                return instance;
            }
        }
        private View()
        {
            allMediator = new Dictionary<string, IMediator>();
        }
        /// <summary>
        /// 获取Mediator
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public IMediator GetMediator(string mediatorName)
        {
            return allMediator.ContainsKey(mediatorName) ? allMediator[mediatorName] : null;
        }
        /// <summary>
        /// 注册Mediator
        /// </summary>
        /// <param name="mediator"></param>
        public void RegisterMediator(IMediator mediator)
        {
            allMediator[mediator.MediatorName] = mediator;
            string[] notifications = mediator.NotificationList();
            for (int i = 0; i < notifications.Length; i++)
            {
                NotificationCenter.Instance.AddObserver(notifications[i], mediator);
            }
        }
        /// <summary>
        /// 移除Mediator
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public IMediator RemoveMediator(string mediatorName)
        {
            IMediator mediator = allMediator.ContainsKey(mediatorName) ? allMediator[mediatorName] : null;
            if (mediator != null)
            {
                allMediator.Remove(mediatorName);
                string[] notifications = mediator.NotificationList();
                for (int i = 0; i < notifications.Length; i++)
                {
                    NotificationCenter.Instance.RemoveObserver(notifications[i], mediator);
                }
            }
            return mediator;
        }
    }
}
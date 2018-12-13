using PJW.MVC.Core;
using PJW.MVC.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.MVC
{
    /// <summary>
    /// 入口
    /// </summary>
    public class ApplicationFacade : Facade
    {
        private static ApplicationFacade _instance;
        public new static ApplicationFacade Instance
        {
            get
            {
                if (_instance == null) _instance = new ApplicationFacade();
                return _instance;
            }
        }
        public void StartUP()
        {
            Debug.Log("MVC 框架开启了   ");
            RegisterProxy(new UserProxy());
            RegisterProxy(new ResourcesProxy());

            RegisterMediator(new UserMediator());
            RegisterMediator(new ResourcesMediator());

            NotificationCenter.Instance.View();
            //先进行网络判断
            SendNotification(NotificationArray.CHECK + NotificationArray.NET);
        }
        
    }
}

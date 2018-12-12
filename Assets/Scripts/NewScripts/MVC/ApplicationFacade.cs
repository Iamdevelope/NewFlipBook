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
        public void StartUP()
        {
            Debug.Log("MVC 框架开启了   ");
            RegisterProxy(new UserProxy());

            RegisterMediator(new UserMediator());

            NotificationCenter.Instance.View();
        }
        private void Test()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("没有联网，请打开网络");
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                Debug.Log("当前为数据流量，下载数据较大，请注意流量是否足够，再确认下载！");
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Debug.Log("开始下载");
            }
        }
    }
}

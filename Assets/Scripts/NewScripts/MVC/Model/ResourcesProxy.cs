
using PJW.Datas;
using PJW.MVC.Base;
using UnityEngine;

namespace PJW.MVC.Model
{
    /// <summary>
    /// 资源是否更新处理
    /// </summary>
    public class ResourcesProxy : BaseProxy
    {
        public new const string NAME = "ResourcesProxy";
        public ResourcesProxy()
        {
            ProxyName = NAME;
            MessageData = new MessageData();
        }
        /// <summary>
        /// 检查网络是否满足更新条件
        /// </summary>
        public void CheckNet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                MessageData.Color = Color.red;
                MessageData.Message = " 没有网络，请连接网络后再重新打开软件。";
                SendNotification(NotificationArray.UPDATE + NotificationArray.FAILURE, MessageData);
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                MessageData.Color = Color.red;
                MessageData.Message = " 当前为4G网络，资源较大，会消耗大量流量，确认继续更新? ";
                SendNotification(NotificationArray.UPDATE + NotificationArray.CHECK, MessageData);
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                SendNotification(NotificationArray.CHECK + NotificationArray.UPDATE);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="rd">资源</param>
        /// <param name="isUpdate">是否可以更新</param>
        public void Update(ResourcesData rd)
        {
            SendNotification(NotificationArray.UPDATE + NotificationArray.SUCCESS);
        }
        /// <summary>
        /// 检查需要更新的资源
        /// </summary>
        public void CheckIsUpdate()
        {


            SendNotification(NotificationArray.START + NotificationArray.UPDATE);
        }
    }
}

using PJW.Book;
using PJW.Book.UI;
using PJW.Datas;
using PJW.MVC.Patterns;
using UnityEngine;

namespace PJW.MVC
{
    /// <summary>
    /// 用于处理资源更新
    /// </summary>
    public class ResourcesMediator : BaseMediator
    {
        public new const string NAME = "ResourcesMediator";
        public ResourcesMediator()
        {
            MediatorName = NAME;
        }
        public override string[] NotificationList()
        {
            return new string[]
            {
                NotificationArray.CHECK + NotificationArray.NET,
                NotificationArray.CHECK + NotificationArray.UPDATE,
                NotificationArray.UPDATE + NotificationArray.CHECK,
                NotificationArray.START + NotificationArray.UPDATE,
                NotificationArray.UPDATE + NotificationArray.SUCCESS,
                NotificationArray.UPDATE + NotificationArray.FAILURE
            };
        }
        public override void HandleNotification(Notification notification)
        {
            switch (notification.name)
            {
                case NotificationArray.CHECK + NotificationArray.NET:
                    Debug.Log("正在检查网络类型");
                    //检查网络是否满足下载要求
                    ResourcesProxy.CheckNet();
                    break;
                case NotificationArray.CHECK + NotificationArray.UPDATE:
                    Debug.Log("判断是否有需要更新资源");
                    //判断需要更新的资源
                    ResourcesProxy.CheckIsUpdate();
                    break;
                case NotificationArray.START + NotificationArray.UPDATE:
                    Debug.Log("开始更新");
                    //开始更新
                    ResourcesProxy.Update(notification.data as ResourcesData);
                    break;
                case NotificationArray.UPDATE + NotificationArray.SUCCESS:
                    Debug.Log("更新完成");
                    GameCore.Instance.OpenNextUIPanel(GameCore.FindObjectOfType<LoginPanel>().gameObject);
                    break;
                case NotificationArray.UPDATE + NotificationArray.CHECK:
                    //提示是否需要进行更新
                    Debug.Log(notification.data as MessageData);
                    GameCore.Instance.SendMessageToMessagePanel(notification.data as MessageData, true);
                    break;
                case NotificationArray.UPDATE + NotificationArray.FAILURE:
                    Debug.Log(notification.data as MessageData);
                    //更新失败，将失败原因显示
                    GameCore.Instance.SendMessageToMessagePanel(notification.data as MessageData,true);
                    break;
            }
        }
        public override string ToString()
        {
            return NAME;
        }
    }
}
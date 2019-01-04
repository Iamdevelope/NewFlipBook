
using PJW.MVC.Patterns;

namespace PJW.MVC
{
    /// <summary>
    /// 用于处理摄像机寻路处理
    /// </summary>
    public class CameraFindPathMediator : BaseMediator
    {
        public new const string NAME = "CameraFindPathMediator";
        
        public CameraFindPathMediator()
        {
            MediatorName = NAME;
        }
        public override string[] NotificationList()
        {
            return new string[]
            {
                NotificationArray.JIANKANG + NotificationArray.ARR + NotificationArray.JINGLINGWU,
                NotificationArray.KEXUE + NotificationArray.ARR + NotificationArray.KEJICHENG,
                NotificationArray.YISHU + NotificationArray.ARR + NotificationArray.HAITAN,
                NotificationArray.YUYAN + NotificationArray.ARR + NotificationArray.MOGUBAO,
                NotificationArray.SHEHUI + NotificationArray.ARR + NotificationArray.CUNZHUANG
            };
        }
        public override void HandleNotification(Notification notification)
        {
            switch (notification.name)
            {
                case NotificationArray.JIANKANG + NotificationArray.ARR + NotificationArray.JINGLINGWU:
                    break;
                default:
                    break;
            }
        }
        public override string ToString()
        {
            return NAME;
        }
    }
}
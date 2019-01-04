

using PJW.MVC.Patterns;

namespace PJW.MVC
{
    /// <summary>
    /// 管理所有书的所属科目类型
    /// </summary>
    public class ClassMediator : BaseMediator
    {
        public new const string NAME = "ClassMediator";
        public ClassMediator()
        {
            MediatorName = NAME;
        }
        public override string[] NotificationList()
        {
            return new string[]
            {
                NotificationArray.SHEHUI,
                NotificationArray.KEXUE,
                NotificationArray.YISHU,
                NotificationArray.YUYAN,
                NotificationArray.JIANKANG
            };
        }
        public override void HandleNotification(Notification notification)
        {

        }
    }
}
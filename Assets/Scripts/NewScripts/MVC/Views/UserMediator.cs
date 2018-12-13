

using PJW.Book;
using PJW.Book.UI;
using PJW.Datas;
using PJW.MVC.Patterns;

namespace PJW.MVC
{
    /// <summary>
    /// 用于处理用户系统的逻辑，包括UI，登录
    /// </summary>
    public class UserMediator : BaseMediator
    {
        public new const string NAME = "UserMediator";

        public UserMediator()
        {
            this.MediatorName = NAME;
        }
        public override string[] NotificationList()
        {
            return new string[]
            {
                NotificationArray.LOGIN,
                NotificationArray.SINAWEIBO + NotificationArray.LOGIN,
                NotificationArray.QQ + NotificationArray.LOGIN,
                NotificationArray.WECHAT + NotificationArray.LOGIN,
                NotificationArray.LOGOUT,
                NotificationArray.LOGIN + NotificationArray.SUCCESS,
                NotificationArray.SINAWEIBO + NotificationArray.LOGIN + NotificationArray.SUCCESS,
                NotificationArray.SINAWEIBO + NotificationArray.LOGIN + NotificationArray.FAILURE,
                NotificationArray.QQ + NotificationArray.LOGIN + NotificationArray.SUCCESS,
                NotificationArray.QQ + NotificationArray.LOGIN + NotificationArray.FAILURE,
                NotificationArray.WECHAT + NotificationArray.LOGIN + NotificationArray.SUCCESS,
                NotificationArray.WECHAT + NotificationArray.LOGIN + NotificationArray.FAILURE,
                NotificationArray.LOGIN + NotificationArray.FAILURE,
                NotificationArray.SHOW + NotificationArray.REGISTER,
                NotificationArray.SHOW + NotificationArray.LOGIN,
                NotificationArray.REGISTER,
                NotificationArray.HIDE + NotificationArray.REGISTER,
                NotificationArray.REGISTER + NotificationArray.SUCCESS,
                NotificationArray.REGISTER + NotificationArray.FAILURE
            };
        }
        public override void HandleNotification(Notification notification)
        {
            switch (notification.name)
            {
                case NotificationArray.SHOW + NotificationArray.LOGIN:
                    GameCore.Instance.OpenNextUIPanel(GameCore.FindObjectOfType<LoginPanel>().gameObject);
                    break;
                case NotificationArray.LOGIN:
                    UserProxy.Login(notification.data as UserData);
                    break;
                case NotificationArray.LOGIN + NotificationArray.SUCCESS:
                    GameCore.Instance.CloseCurrentUIPanel();
                    break;
                case NotificationArray.LOGIN + NotificationArray.FAILURE:
                    GameCore.Instance.SendMessageToMessagePanel(notification.data as MessageData);
                    break;
                case NotificationArray.SHOW + NotificationArray.REGISTER:
                    GameCore.Instance.OpenNextUIPanel(GameCore.FindObjectOfType<RegisterPanel>().gameObject);
                    break;
                case NotificationArray.REGISTER:
                    UserProxy.Register(notification.data as UserData);
                    break;
                case NotificationArray.REGISTER + NotificationArray.SUCCESS:
                    GameCore.Instance.SendMessageToMessagePanel(notification.data as MessageData);
                    GameCore.Instance.OpenNextUIPanel(GameCore.FindObjectOfType<LoginPanel>().gameObject);
                    break;
                case NotificationArray.REGISTER + NotificationArray.FAILURE:
                    GameCore.Instance.SendMessageToMessagePanel(notification.data as MessageData);
                    break;
                case NotificationArray.LOGOUT:
                    GameCore.Instance.OpenNextUIPanel(GameCore.FindObjectOfType<LoginPanel>().gameObject);
                    break;
            }
        }
        public override string ToString()
        {
            return NAME;
        }
    }
}

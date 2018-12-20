using UnityEngine;
using UnityEngine.UI;
using PJW.Datas;

namespace PJW.Book.UI
{
    /// <summary>
    /// 登录界面
    /// </summary>
    public class LoginPanel : BasePanel
    {
        private Button exitBtn;
        private Button loginBtn;
        private Button registerBtn;
        private Button QQBtn;
        private Button weixinBtn;
        private Button weiboBtn;
        private InputField userName;
        private InputField passWord;
        private LoadingPanel loadingPanel;
        private string fileName;
        public override void Init()
        {
            MessageData = new MessageData();
            userName = transform.Find("UserName").GetComponentInChildren<InputField>();
            passWord = transform.Find("PassWord").GetComponentInChildren<InputField>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
            loginBtn = transform.Find("LoginBtn").GetComponent<Button>();
            registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
            QQBtn = transform.Find("QQBtn").GetComponent<Button>();
            weixinBtn = transform.Find("WeiXinBtn").GetComponent<Button>();
            weiboBtn = transform.Find("WeiBoBtn").GetComponent<Button>();
            loadingPanel = FindObjectOfType<LoadingPanel>();

            exitBtn.onClick.AddListener(ExitButtonHandle);
            loginBtn.onClick.AddListener(LoginButtonHandle);
            EnterClickedEvent += LoginButtonHandle;
            registerBtn.onClick.RemoveAllListeners();
            registerBtn.onClick.AddListener(RegisterButtonHandle);
            QQBtn.onClick.AddListener(QQButtonHandle);
            weixinBtn.onClick.AddListener(WeiXinButtonHandle);
            weiboBtn.onClick.AddListener(WeiBoButtonHandle);
        }
        /// <summary>
        /// 注册
        /// </summary>
        private void RegisterButtonHandle()
        {
            base.PlayClickSound();
            SendNotification(NotificationArray.SHOW + NotificationArray.REGISTER, "");
        }
        /// <summary>
        /// 登录
        /// </summary>
        private void LoginButtonHandle()
        {
            base.PlayClickSound();
            string username = userName.text;
            string password = passWord.text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                MessageData.Message = "用户名/密码不能为空！";
            else if(!StringHelper.IsSafeSqlString(username)||
                StringHelper.CheckBadWord(username)||
                !StringHelper.IsSafeSqlString(password)||
                StringHelper.CheckBadWord(password))
            {
                MessageData.Message = "不允许出现非法字符！";
            }
            if (!string.IsNullOrEmpty(MessageData.Message))
            {
                SendNotification(NotificationArray.LOGIN + NotificationArray.FAILURE, MessageData);
                return;
            }
            SendNotification(NotificationArray.LOGIN, new UserData() { Username = username, Password = password });
        }
        /// <summary>
        /// 退出
        /// </summary>
        private void ExitButtonHandle()
        {
            PlayClickSound();
            Application.Quit();
        }
        /// <summary>
        /// QQ登录
        /// </summary>
        private void QQButtonHandle()
        {
            PlayClickSound();
            SendNotification(NotificationArray.QQ + NotificationArray.LOGIN);
        }
        /// <summary>
        /// 微信登录
        /// </summary>
        private void WeiXinButtonHandle()
        {
            PlayClickSound();
            SendNotification(NotificationArray.WECHAT + NotificationArray.LOGIN);
        }
        /// <summary>
        /// 微博登录
        /// </summary>
        private void WeiBoButtonHandle()
        {
            PlayClickSound();
            SendNotification(NotificationArray.SINAWEIBO + NotificationArray.LOGIN);
        }
        public override void Reset(Vector3 scale, float t,string msg="")
        {
            if (scale == Vector3.one)
                EnterClickedEvent += LoginButtonHandle;
            else
                EnterClickedEvent -= LoginButtonHandle;
            base.Reset(scale, t, msg);
        }
    }
}
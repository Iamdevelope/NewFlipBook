
using UnityEngine;
using UnityEngine.UI;
using PJW.Datas;

namespace PJW.Book.UI
{
    /// <summary>
    /// 注册面板
    /// </summary>
    public class RegisterPanel : BasePanel
    {
        private Button exitBtn;
        private InputField userName;
        private InputField passWord;
        private InputField rePassWord;
        private Button registerBtn;
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            MessageData = new MessageData();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
            registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
            userName = transform.Find("UserName").GetComponentInChildren<InputField>();
            passWord = transform.Find("PassWord").GetComponentInChildren<InputField>();
            rePassWord = transform.Find("RePassWord").GetComponentInChildren<InputField>();

            registerBtn.onClick.AddListener(RegisterButtonHandle);
            exitBtn.onClick.AddListener(ExitRegisterPanelButtonHandle);
        }
        /// <summary>
        /// 关闭事件
        /// </summary>
        private void ExitRegisterPanelButtonHandle()
        {
            base.PlayClickSound();
            SendNotification(NotificationArray.SHOW + NotificationArray.LOGIN,"");
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterButtonHandle()
        {
            base.PlayClickSound();
            string username = userName.text;
            string password = passWord.text;
            string repassword = rePassWord.text;
            string msg = "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repassword))
                msg = "用户名或密码不能为空";
            if (!password.Equals(repassword))
                msg = "两次输入的密码不相同，请重新输入！";

            if (string.IsNullOrEmpty(msg))
                SendNotification(NotificationArray.REGISTER, new UserData() { Username = username, Password = password });
            else
            {
                MessageData.Message = msg;
                SendNotification(NotificationArray.REGISTER + NotificationArray.FAILURE, MessageData);
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="t"></param>
        public override void Reset(Vector3 scale, float t,string msg="")
        {
            userName.text = "";
            passWord.text = "";
            rePassWord.text = "";
            if (scale == Vector3.one)
                EnterClickedEvent += RegisterButtonHandle;
            else
                EnterClickedEvent -= RegisterButtonHandle;
            base.Reset(scale, t, msg);
        }
    }
}

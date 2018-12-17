
using System;
using PJW.Datas;
using PJW.MVC.Base;
using UnityEngine;

namespace PJW.MVC.Model
{
    /// <summary>
    /// 用户数据处理
    /// </summary>
    public class UserProxy : BaseProxy
    {
        public new const string NAME = "UserProxy";
        public UserProxy()
        {
            ProxyName = NAME;
            MessageData = new MessageData();
        }
        public void Login(UserData ud)
        {
            OpenDB();
            reader = db.SelectWhere("user",
                new string[] { "Username" },
                new string[] { "Username", "Password" },
                new string[] { "=", "=" },
                new string[] { ud.Username, ud.Password }
                );
            //如果该条数据存在，则登录成功
            if (reader.HasRows)
            {
                Debug.Log(ud);
                SendNotification(NotificationArray.LOGIN + NotificationArray.SUCCESS, ud);
            }
            else
            {
                MessageData.Message = " 请检查用户名或密码是否正确！";
                SendNotification(NotificationArray.LOGIN + NotificationArray.FAILURE, MessageData);
            }
            CloseDB();
        }
        public void Register(UserData ud)
        {
            OpenDB();
            reader = db.Select("user", "Username", ud.Username);
            //如果数据库中存在相同用户名，则注册失败
            if (reader.HasRows)
            {
                MessageData.Message = " 注册失败，该用户已经存在！";
                SendNotification(NotificationArray.REGISTER + NotificationArray.FAILURE, MessageData);
            }
            else
            {
                MessageData.Message = " 注册成功。";
                db.InsertInto("user", new string[] { ud.Username, ud.Password });
                SendNotification(NotificationArray.REGISTER + NotificationArray.SUCCESS, MessageData);
            }
            CloseDB();
        }
        public void SinaWeiboLogin()
        {

        }

        internal void WechatLogin()
        {

        }

        internal void QQLogin()
        {

        }
    }
}

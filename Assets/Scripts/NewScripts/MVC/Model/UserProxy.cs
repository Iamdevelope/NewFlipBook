
using System;
using System.Collections;
using System.IO;
using cn.sharesdk.unity3d;
using LitJson;
using PJW.Book;
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
        private string fileName;

        public UserProxy()
        {
            ProxyName = NAME;
            MessageData = new MessageData();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="ud"></param>
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
                SendNotification(NotificationArray.LOGIN + NotificationArray.SUCCESS, ud);
            }
            else
            {
                MessageData.Message = " 请检查用户名或密码是否正确！";
                SendNotification(NotificationArray.LOGIN + NotificationArray.FAILURE, MessageData);
            }
            CloseDB();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="ud"></param>
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
        /// <summary>
        /// 新浪微博第三方登录
        /// </summary>
        public void SinaWeiboLogin()
        {
            fileName = "/sina.json";
            if (File.Exists(Application.persistentDataPath + fileName))
            {
                return;
            }
            AddAuthHandler(PlatformType.SinaWeibo);
        }
        /// <summary>
        /// 微信第三方登录
        /// </summary>
        public void WechatLogin()
        {
            fileName = "/wechat.json";
            if (File.Exists(Application.persistentDataPath + fileName))
            {
                return;
            }
            AddAuthHandler(PlatformType.WeChat);
        }
        /// <summary>
        /// QQ第三方登录
        /// </summary>
        public void QQLogin()
        {
            fileName = "/qq.json";
            if (File.Exists(Application.persistentDataPath + fileName))
            {
                return;
            }
            AddAuthHandler(PlatformType.QQ);
        }
        /// <summary>
        /// 添加授权事件
        /// </summary>
        /// <param name="platform"></param>
        private void AddAuthHandler(PlatformType platform)
        {
            if (GameCore.Instance.ssdk.authHandler == null)
                GameCore.Instance.ssdk.authHandler = AuthHandler;
            GameCore.Instance.ssdk.Authorize(platform);
        }
        /// <summary>
        /// 授权回调
        /// </summary>
        /// <param name="reqID"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        private void AuthHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
        {
            //如果授权成功
            if (state == ResponseState.Success)
            {
                JsonData userData = JsonMapper.ToObject(JsonMapper.ToJson(data));
                SaveUserInfo(JsonMapper.ToJson(data));
                string icon = userData["icon"].ToString();
                GameCore.Instance.StartCoroutine(DownUserIcon(icon));
                SendNotification(NotificationArray.LOGIN + NotificationArray.SUCCESS);
                Debug.Log("授权成功");
            }
            else if (state == ResponseState.Fail)
            {
                MessageData.Message = "授权失败！";
                SendNotification(NotificationArray.LOGIN + NotificationArray.FAILURE, MessageData);
                Debug.Log("授权失败！");
            }
        }
        /// <summary>
        /// 将用户的头像下载
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        private IEnumerator DownUserIcon(string icon)
        {
            if (File.Exists(Application.persistentDataPath + "/icon.jpg"))
                File.Delete(Application.persistentDataPath + "/icon.jpg");
            WWW www = new WWW(icon);
            yield return www;
            FileStream stream = File.Create(Application.persistentDataPath + "/icon.jpg");
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height);
            www.LoadImageIntoTexture(texture);
            byte[] bytes = texture.EncodeToJPG();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            stream.Dispose();
        }
        /// <summary>
        /// 将得到的用户信息保存
        /// </summary>
        /// <param name="jsonFile"></param>
        private void SaveUserInfo(string jsonFile)
        {
            if (File.Exists(Application.persistentDataPath + "/" + fileName))
                File.Delete(Application.persistentDataPath + "/" + fileName);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonFile);
        }
    }
}

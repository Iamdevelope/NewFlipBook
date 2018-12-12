using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
using cn.sharesdk.unity3d;
using System;
using System.IO;
using LitJson;

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
            userName = transform.Find("UserName").GetComponentInChildren<InputField>();
            passWord = transform.Find("PassWord").GetComponentInChildren<InputField>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
            loginBtn = transform.Find("LoginBtn").GetComponent<Button>();
            registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
            QQBtn = transform.Find("QQBtn").GetComponent<Button>();
            //weixinBtn = transform.Find("WeiXinBtn").GetComponent<Button>();
            //weiboBtn = transform.Find("WeiBoBtn").GetComponent<Button>();
            loadingPanel = FindObjectOfType<LoadingPanel>();

            exitBtn.onClick.AddListener(ExitButtonHandle);
            loginBtn.onClick.AddListener(LoginButtonHandle);
            EnterClickedEvent += LoginButtonHandle;
            registerBtn.onClick.RemoveAllListeners();
            registerBtn.onClick.AddListener(RegisterButtonHandle);
            QQBtn.onClick.AddListener(QQButtonHandle);
            //weixinBtn.onClick.AddListener(WeiXinButtonHandle);
            //weiboBtn.onClick.AddListener(WeiBoButtonHandle);
        }
        /// <summary>
        /// 注册
        /// </summary>
        private void RegisterButtonHandle()
        {
            base.PlayClickSound();
            GameCore.Instance.OpenNextUIPanel(FindObjectOfType<RegisterPanel>().gameObject);
        }
        /// <summary>
        /// 登录
        /// </summary>
        private void LoginButtonHandle()
        {
            OpenDB();
            base.PlayClickSound();
            string username = userName.text;
            string password = passWord.text;
            string msg = "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                msg = "用户名/密码不能为空！";
            if (!string.IsNullOrEmpty(msg))
            {
                GameCore.Instance.SendMessageToMessagePanel(msg);
                CloseDB();
                return;
            }
            reader = db.SelectWhere("user",
                new string[] { "Username" },
                new string[] { "Username", "Password" },
                new string[] { "=", "=" },
                new string[] { username, password }
                );
            if (reader.HasRows)
            {
                PlayerPrefs.SetString("username", username);
                msg = "登录成功！";
                //GameCore.Instance.isSuccessLogin = true;
                GameCore.Instance.OpenNextUIPanel(FindObjectOfType<CharacterSelectPanel>().gameObject);
                Reset(Vector3.zero, 0.6f);
            }
            else
                msg = "用户名或密码不正确,请重新输入!";

            CloseDB();
            if (!string.IsNullOrEmpty(msg))
            {
                GameCore.Instance.SendMessageToMessagePanel(msg);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        private void ExitButtonHandle()
        {
            GameCore.Instance.PlaySoundBySoundName();
            Application.Quit();
        }
        /// <summary>
        /// QQ登录
        /// </summary>
        private void QQButtonHandle()
        {
            fileName = "/qq.json";
            if (File.Exists(Application.persistentDataPath + fileName))
            {
                return;
            }
            GameCore.Instance.ssdk.authHandler = AuthHandler;
            GameCore.Instance.ssdk.Authorize(PlatformType.QQ);
        }

        private void AuthHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
        {
            //如果授权成功
            if (state == ResponseState.Success)
            {
                JsonData userData = JsonMapper.ToObject(JsonMapper.ToJson(data));
                SaveUserInfo(JsonMapper.ToJson(data));
                string icon = userData["icon"].ToString();
                StartCoroutine(DownUserIcon(icon));
                //text.text += "\n userid : " + userData["userID"] + "\n username : " + userData["nickname"] + "\n icon : " + userData["icon"];
                //text.text += "\n微信授权成功！！！";
                userName.text = userData["nickname"].ToString();
                Debug.Log("授权成功");
            }
            else if (state == ResponseState.Fail)
            {
                Debug.Log("授权失败！");
            }
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        private void WeiXinButtonHandle()
        {
            fileName = "/wechat.json";
            if (File.Exists(Application.persistentDataPath + fileName))
            {
                return;
            }
            GameCore.Instance.ssdk.authHandler = AuthHandler;
            GameCore.Instance.ssdk.Authorize(PlatformType.WeChat);
        }
        /// <summary>
        /// 微博登录
        /// </summary>
        private void WeiBoButtonHandle()
        {

        }
        
        private IEnumerator DownUserIcon(string icon)
        {
            Debug.Log("开启协程进行资源下载");
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

        private void SaveUserInfo(string jsonFile)
        {
            if (File.Exists(Application.persistentDataPath + "/" + fileName))
                File.Delete(Application.persistentDataPath + "/" + fileName);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonFile);
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
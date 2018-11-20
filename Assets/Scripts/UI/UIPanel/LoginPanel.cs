using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

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
        public override void Init()
        {
            userName = transform.Find("UserName").GetComponentInChildren<InputField>();
            passWord = transform.Find("PassWord").GetComponentInChildren<InputField>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
            loginBtn = transform.Find("LoginBtn").GetComponent<Button>();
            registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
            //QQBtn = transform.Find("QQBtn").GetComponent<Button>();
            //weixinBtn = transform.Find("WeiXinBtn").GetComponent<Button>();
            //weiboBtn = transform.Find("WeiBoBtn").GetComponent<Button>();
            loadingPanel = FindObjectOfType<LoadingPanel>();

            exitBtn.onClick.AddListener(ExitButtonHandle);
            loginBtn.onClick.AddListener(LoginButtonHandle);
            registerBtn.onClick.RemoveAllListeners();
            registerBtn.onClick.AddListener(RegisterButtonHandle);
            //QQBtn.onClick.AddListener(QQButtonHandle);
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
                GameCore.Instance.isSuccessLogin = true;
                Reset(Vector3.zero, 0.6f);
            }
            else
                msg = "用户名或密码不正确,请重新输入!";

            CloseDB();
            if (!string.IsNullOrEmpty(msg))
            {
                GameCore.Instance.SendMessageToMessagePanel(msg);
                return;
            }
            //进行数据库数据对比
            //登录成功切换场景
            //StartCoroutine(ChangeScene());
            
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

        }
        /// <summary>
        /// 微信登录
        /// </summary>
        private void WeiXinButtonHandle()
        {

        }
        /// <summary>
        /// 微博登录
        /// </summary>
        private void WeiBoButtonHandle()
        {

        }
        private IEnumerator ChangeScene()
        {
            if (GameCore.Instance.asset != null)
            {
                GameCore.Instance.asset.Unload(true);
                GameCore.Instance.asset = null;
            }
            SceneManager.LoadSceneAsync("Bookstore");
            //PlayerPrefs.SetString("AssetBundle", "dinosaurchangefoam.dinosaurchangefoam");
            yield return StartCoroutine(WaitLoadingNextScene("Bookstore"));
        }
        private IEnumerator WaitLoadingNextScene(string v)
        {
            if (SceneManager.GetActiveScene().name != v)
            {
                GameCore.Instance.OpenLoadingPanel(Vector3.one);
                yield return null;
            }
            else
                GameCore.Instance.OpenLoadingPanel(Vector3.zero);
        }
        public override void Reset(Vector3 scale, float t,string msg="")
        {
            transform.DOScale(scale, t);
        }
    }
}
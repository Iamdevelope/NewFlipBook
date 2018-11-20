using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
            GameCore.Instance.OpenNextUIPanel(FindObjectOfType<LoginPanel>().gameObject);
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterButtonHandle()
        {
            OpenDB();
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
            {
                reader = db.Select("user", "Username", GetStr(username));
                if (reader.HasRows)
                {
                    msg = "该用户名已经注册过了，不可再注册。";
                }
                else
                {
                    db.InsertInto("user",
                        new string[] { GetStr(username), GetStr(password) }
                        );
                    msg = "注册成功！";
                }
                GameCore.Instance.SendMessageToMessagePanel(msg);
                CloseDB();
                return;
            }
            else
            {
                GameCore.Instance.SendMessageToMessagePanel(msg);
                CloseDB();
                return;
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
            transform.DOScale(scale, t);
        }
    }
}

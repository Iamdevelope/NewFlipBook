using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace PJW.Book.UI
{
    /// <summary>
    /// 消息面板
    /// </summary>
    public class MessagePanel : BasePanel
    {
        private Text message;
        private Button exitBtn;
        private Button enterBtn;
        public override void Init()
        {
            message = GetComponentInChildren<Text>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
            enterBtn = transform.Find("Enter").GetComponent<Button>();

            exitBtn.onClick.AddListener(CloseButtonHandle);
        }
        /// <summary>
        /// 关闭面板
        /// </summary>
        private void CloseButtonHandle()
        {
            Reset(Vector3.zero, 0.3f);
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(string msg)
        {
            message.text = msg;
            Reset(Vector3.one,0.3f);
            StartCoroutine(CloseSelfWaitTime());
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="t"></param>
        public override void Reset(Vector3 scale, float t,string msg="")
        {
            transform.DOScale(scale, t);
        }
        /// <summary>
        /// 关闭自身
        /// </summary>
        /// <returns></returns>
        private IEnumerator CloseSelfWaitTime()
        {
            yield return new WaitForSeconds(2);
            CloseButtonHandle();
        }
    }
}

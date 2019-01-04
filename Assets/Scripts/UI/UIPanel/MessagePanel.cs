using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        private string msgType;
        public override void Init()
        {
            message = GetComponentInChildren<Text>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
            enterBtn = transform.Find("Enter").GetComponent<Button>();

            exitBtn.onClick.AddListener(CloseButtonHandle);
            enterBtn.onClick.AddListener(EnterHandle);
        }

        private void EnterHandle()
        {
            if (msgType != null)
                SendNotification(msgType);
            CloseButtonHandle();
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        private void CloseButtonHandle()
        {
            base.PlayClickSound();
            Reset(Vector3.zero, 0.3f);
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg">需要显示的信息</param>
        /// <param name="showEnterBtn">是否需要显示确认按钮</param>
        public void ShowMessage(MessageData msg, bool showEnterBtn)
        {
            message.text = msg.Message;
            message.color = msg.Color;
            msgType = msg.Type;
            Reset(Vector3.one, 0.3f);
            if (!showEnterBtn)
            {
                if(enterBtn.gameObject.activeSelf)
                    enterBtn.gameObject.SetActive(false);
                StartCoroutine(CloseSelfWaitTime());
            }
            else
                enterBtn.gameObject.SetActive(true);
        }
        /// <summary>
        /// 关闭自身
        /// </summary>
        /// <returns></returns>
        private IEnumerator CloseSelfWaitTime()
        {
            yield return new WaitForSeconds(2f);
            CloseButtonHandle();
        }
    }
}
using UnityEngine;
using DG.Tweening;
using System;
using PJW.MVC;

namespace PJW.Book.UI
{
    public class BasePanel : BaseMonobehiviour
    {
        public delegate void ButtonClickedEvent(string name);
        public event ButtonClickedEvent ClassTypeButtonHandle;
        public event ButtonClickedEvent ClassButtonHandle;
        public event Action EnterClickedEvent;
        public MessageData MessageData;
        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (EnterClickedEvent != null)
                    EnterClickedEvent();
            }
        }
        public virtual void Init() { }
        /// <summary>
        /// 显示和隐藏
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public virtual void Reset(Vector3 scale,float t,string msg="")
        {
            transform.DOScale(scale, t);
        }
        public void PlayClickSound()
        {
            GameCore.Instance.PlaySoundBySoundName();
        }
        public virtual void StartAnim() { }
        public virtual void OverAnim() { }
    }
}
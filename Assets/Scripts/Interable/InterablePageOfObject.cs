using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PJW.Book
{
    /// <summary>
    /// 书本中可交互的物体
    /// </summary>
    public class InterablePageOfObject : InterableObject
    {
        public bool notMove;
        public bool isFirstDown;
        /// <summary>
        /// 鼠标移入物体时触发的事件
        /// </summary>
        public override void OnMouseUp()
        {
            GameCore.Instance.PlaySoundBySoundName(SoundManager.CLICK_02);
            if (!notMove)
            {

                if (!isFirstDown)
                {
                    GetComponentInParent<Canvas>().overrideSorting = true;
                    GetComponentInParent<Canvas>().sortingOrder = 1;
                    isFirstDown = !isFirstDown;
                    transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f);
                }
                else
                {
                    GetComponentInParent<Canvas>().overrideSorting = false;
                    GetComponentInParent<Canvas>().sortingOrder = 0;
                    isFirstDown = !isFirstDown;
                    transform.DOScale(Vector3.one, 1f);
                }
            }
        }
    }
}
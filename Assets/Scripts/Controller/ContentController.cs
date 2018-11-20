using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PJW.Book
{
    /// <summary>
    /// 恐龙介绍界面
    /// </summary>
    public class ContentController : MonoBehaviour
    {

        public void ExitClickEvent()
        {
            GameCore.Instance.PlaySoundBySoundName();
            transform.DOScale(0, 0.3f);
        }
    }
}

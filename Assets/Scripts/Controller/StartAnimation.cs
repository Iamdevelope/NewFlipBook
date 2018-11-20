using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;
using MyCommon;

namespace PJW.Book
{
    /// <summary>
    /// 控制刚进入场景时的动画显示
    /// </summary>
    public class StartAnimation : MonoSingleton<StartAnimation>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            ViewController.Instance._image.DOFade(0, 4f).OnComplete(() =>
            {
                float f = ViewController.Instance.StartInSceneAnim();
                StartCoroutine(CloseCameraStartAnim(f));
            });
        }

        private IEnumerator CloseCameraStartAnim(float time)
        {
            yield return new WaitForSeconds(time);
            GetComponent<Animator>().SetBool("start", false);
        }
    }
}
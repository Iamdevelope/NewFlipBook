using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using MyCommon;
using UnityEngine.EventSystems;

namespace PJW.Book
{
    /// <summary>
    /// 视角控制
    /// </summary>
    public class ViewController : MonoSingleton<ViewController>
    {
        [HideInInspector]
        public bool is3DView;
        [HideInInspector]	
        public Animator anim;
        private int index = 0;
        public bool isExit;
        public Image _image;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
        /// <summary>
        /// 开始时进入书本的动画
        /// </summary>
        public float StartInSceneAnim()
        {
            anim.SetBool("start", true);
            return GetCurrentPlayAnim("start");
        }
        /// <summary>
        /// 翻书结束时，需要进行播放结束动画
        /// </summary>
        public void OverAnim()
        {
            _image.DOFade(1f, 3f);
            anim.SetBool("over", true);
            foreach (var item in GetComponentsInChildren<ICanReset>())
                item.Reset();
            StartCoroutine(CloseOverAnim());
        }
        IEnumerator CloseOverAnim()
        {
            yield return new WaitForFixedUpdate();
            anim.SetBool("over", false);
        }
        /// <summary>
        /// 切换到3D视角
        /// </summary>
        public void Change3DView(float time,Action callBack)
        {
            //在进行视野切换时，禁止再次翻页
            FlipBook.Instance.isFlip = true;
            index++;
            InterableObject.Instance.isTrigger = false;
            if (anim.GetBool("playCommon"))
            {
                anim.SetBool("playCommon", false);
                StartCoroutine(CanFlip(time,callBack));
                return;
            }
            anim.SetBool("play3DView", true);
            StartCoroutine(CanFlip(time,callBack));
        }
        /// <summary>
        /// 由3D视角切换到正常视角
        /// </summary>
        public void ChangeCommonView()
        {
            if (!anim.GetBool("play3DView"))
            {
                StartCoroutine(ChangeSeeState(1));
                return;
            }
            InterableObject.Instance.isTrigger = false;
            float time;
            anim.SetBool("playCommon", true);
            time = GetCurrentPlayAnim("playCommon");
            StartCoroutine(ChangeSeeState(time));
        }
        /// <summary>
        /// 改变视角状态
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator ChangeSeeState(float time)
        {
            yield return new WaitForSeconds(time);
            InterableObject.Instance.isTrigger = true;
            FlipBook.Instance.isFlip = false;
        }
        private IEnumerator CanFlip(float time,Action callBack)
        {
            yield return new WaitForSeconds(time);
            InterableObject.Instance.isTrigger = true;
            FlipBook.Instance.isFlip = false;
            if (callBack != null)
                callBack();
        }
        /// <summary>
        /// 通过动画的名字得到当前动画的播放时长
        /// </summary>
        /// <param name="animName"></param>
        /// <returns></returns>
        private float GetCurrentPlayAnim(string animName)
        {
            AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(0);
            float time = 4;
            for (int i = 0; i < clipInfos.Length; i++)
            {
                if (clipInfos[i].clip.name.Equals(animName))
                    time = clipInfos[i].clip.length;
            }
            return time;
        }
        /// <summary>
        /// 判断如何加载场景
        /// </summary>
        public void LoadScene()
        {
            if (isExit)
                SceneManager.LoadScene("Bookstore");
            else
                SceneManager.LoadScene("Book");
        }
        /// <summary>
        /// 当入场动画结束，才可以进行翻页操作
        /// </summary>
        public void CanClickBook()
        {
            GameObject.Find("Book").GetComponentInChildren<StartPageFlip>().canFlip = true;
            //GameObject.Find("BookDummy").GetComponentInChildren<BookDummyStartPageFlip>().canFlip = true;
        }
    }
}
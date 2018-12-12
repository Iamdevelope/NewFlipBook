using DG.Tweening;
using PJW.HotUpdate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PJW.Book.UI
{
    /// <summary>
    /// 下载界面
    /// </summary>
    public class DownLoadPanel : BasePanel
    {
        [HideInInspector]
        public Slider slider;
        public bool isDown;
        private WWW progressWWW;
        private Text progressText;
        private int index;
        private int count;
        private string text;
        public override void Init()
        {
            LoadAssetByAssetBundle loadAssetByAssetBundle = FindObjectOfType<LoadAssetByAssetBundle>();
            loadAssetByAssetBundle.slider = transform.GetComponentInChildren<Slider>();
            loadAssetByAssetBundle.progressText = transform.GetComponentInChildren<Text>();
            slider = GetComponentInChildren<Slider>();
            progressText = GetComponentInChildren<Text>();
        }

        public override void Update()
        {
            if (isDown)
            {
                if (progressWWW != null && !progressWWW.isDone && progressText != null)
                {
                    progressText.text = string.Format("下载进度:{0:F}% \n当前正在下载第{1}个资源，共{2}个资源。", progressWWW.progress * 100, index, count);
                    slider.value = progressWWW.progress;
                }
            }
            else
            {
                progressText.text = text;
                slider.value = 1;
                Reset(Vector3.zero, 0);
                isDown = true;
            }
        }
        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="www">下载使用的WWW</param>
        /// <param name="index">当前正在下载第几个资源</param>
        /// <param name="count">共有几个资源需要下载</param>
        public void StartDownLoad(WWW www,int index,int count)
        {
            Debug.Log(" start down assets ");
            progressText.text = "";
            slider.value = 0;
            isDown = true;
            progressWWW = www;
            this.index = index;
            this.count = count;
        }
        public void StartDownLoad(WWW www)
        {
            slider.value = www.progress;
            //下载完成
            if (1 == www.progress)
            {
                progressText.text = string.Format("下载进度:{0:F}%", 100);
                slider.value = 1;
            }
            else
            {
                progressText.text = string.Format("下载进度:{0:F}% ", www.progress * 100.0);
            }
        }
        
        public void DownOver()
        {
            Debug.Log(" down over ");
            isDown = false;
            //progressWWW.Dispose();
            text = string.Format("下载进度:{0:F}% \n当前正在下载第{1}个资源，共{2}个资源。", 100, count, count);
            slider.value = 1;
            //Reset(Vector3.zero, 0.6f);
            index = 0;
            count = 0;
        }
    }
}
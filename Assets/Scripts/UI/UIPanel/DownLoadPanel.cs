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
        private Slider slider;
        public override void Init()
        {
            LoadAssetByAssetBundle loadAssetByAssetBundle = FindObjectOfType<LoadAssetByAssetBundle>();
            loadAssetByAssetBundle.slider = transform.GetComponentInChildren<Slider>();
            loadAssetByAssetBundle.progressText = transform.GetComponentInChildren<Text>();
            slider = GetComponentInChildren<Slider>();
        }
        public override void Reset(Vector3 scale, float t,string msg="")
        {
            base.Reset(scale, t, msg);
            //transform.DOScale(scale, t);
            //transform.DOLocalMove(scale, t);
        }
        public IEnumerator DownOver()
        {
            yield return new WaitForSeconds(1);
            Reset(Vector3.zero, 0);
        }
    }
}
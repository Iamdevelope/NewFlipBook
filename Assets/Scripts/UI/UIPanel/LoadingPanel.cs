using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book.UI
{
    /// <summary>
    /// 等待页面
    /// </summary>
    public class LoadingPanel : BasePanel
    {
        private RectTransform child;
        public override void Init()
        {
            child = transform.GetChild(0).GetComponent<RectTransform>();
            StartCoroutine(ObjectRorate());
            child.Rotate(Vector3.forward, -Time.deltaTime * 100);
        }

        private IEnumerator ObjectRorate()
        {
            yield return new WaitForEndOfFrame();
            child.Rotate(Vector3.forward, -Time.deltaTime * 100);
            StartCoroutine(ObjectRorate());
        }
        public override void Reset(Vector3 scale, float t,string msg="")
        {
            //transform.DOScale(scale, t);
            base.Reset(scale, t, msg);
        }
    }
}

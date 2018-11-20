using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace PJW.Book.UI
{
    /// <summary>
    /// 标题面板
    /// </summary>
    public class TitlePanel : BasePanel
    {
        private RectTransform children;
        private Image classTypeSprite;
        private float startPos;
        public override void Init()
        {
            children = transform.Find("BG").GetComponent<RectTransform>();
            classTypeSprite = children.Find("ClassTypeName").GetComponent<Image>();
            startPos = children.localPosition.y;
        }
        /// <summary>
        /// 根据当前课程类型名字，将标题的精灵改成对应的
        /// </summary>
        /// <param name="titleName">课程类型名字</param>
        public void ChangeTitleSprite(string titleName)
        {
            if (classTypeSprite == null) return;
            string path = "UISprites/Bookstore/Title/biaoti_" + titleName;
            Sprite sprite = Resources.Load<Sprite>(path);
            classTypeSprite.sprite = sprite;
            StartAnim();
        }
        public override void StartAnim()
        {
            children.DOLocalMoveY(-100, 2f);
            //children.TransformPingPongMove(-190, 3f);
        }
        public override void OverAnim()
        {
            children.DOLocalMoveY(startPos, 0.6f);
        }
    }
}
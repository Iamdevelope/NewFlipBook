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
        private Dictionary<string, Sprite> titleMap;
        public override void Init()
        {
            titleMap = new Dictionary<string, Sprite>();
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
            if(!titleMap.ContainsKey(titleName))
                titleMap[titleName]= UnityEngine.Resources.Load<Sprite>("UISprites/Bookstore/Title/biaoti_" + titleName);

            classTypeSprite.sprite = titleMap[titleName];
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
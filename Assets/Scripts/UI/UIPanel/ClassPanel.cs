﻿using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace PJW.Book.UI
{
    /// <summary>
    /// 课程面板
    /// </summary>
    public class ClassPanel : BasePanel
    {
        private RectTransform children;
        private RectTransform shehui;
        private RectTransform yuyan;
        private RectTransform jiankang;
        private RectTransform yishu;
        private RectTransform kexue;

        private string currentName = "";
        private RectTransform currentClickedButton;
        private Vector3 buttonClickedScale = new Vector3(1.5f, 1.5f, 1.5f);
        private float startPos;
        public override void Init()
        {
            children = transform.Find("BG").GetComponent<RectTransform>();

            shehui = children.Find(PJW.Book.StartAnim.SHEHUI_CUNZHUANG.Split('_')[0]).GetComponent<RectTransform>();
            yuyan = children.Find(PJW.Book.StartAnim.YUYAN_MOGUBAO.Split('_')[0]).GetComponent<RectTransform>();
            jiankang = children.Find(PJW.Book.StartAnim.JIANKANG_JINGLINGWU.Split('_')[0]).GetComponent<RectTransform>();
            yishu = children.Find(PJW.Book.StartAnim.YISHU_HAITAN.Split('_')[0]).GetComponent<RectTransform>();
            kexue = children.Find(PJW.Book.StartAnim.KEXUE_KEJICHENG.Split('_')[0]).GetComponent<RectTransform>();
            startPos = children.localPosition.x;
            //注册事件
            shehui.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(shehui.gameObject.name));
            yuyan.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(yuyan.gameObject.name));
            jiankang.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(jiankang.gameObject.name));
            yishu.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(yishu.gameObject.name));
            kexue.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(kexue.gameObject.name));
        }
        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="name"></param>
        private void ButtonClickedByName(string name)
        {
            if (currentClickedButton != null)
            {
                if (currentClickedButton.name.Equals(name)) return;
                currentClickedButton.DOScale(Vector3.one, 0.6f);
            }
            switch (name)
            {
                case "shehui":
                    ClassButton(PJW.Book.StartAnim.SHEHUI_CUNZHUANG.Split('_')[0]);
                    currentClickedButton = shehui;
                    break;
                case "yuyan":
                    ClassButton(PJW.Book.StartAnim.YUYAN_MOGUBAO.Split('_')[0]);
                    currentClickedButton = yuyan;
                    break;
                case "jiankang":
                    ClassButton(PJW.Book.StartAnim.JIANKANG_JINGLINGWU.Split('_')[0]);
                    currentClickedButton = jiankang;
                    break;
                case "yishu":
                    SpeciaButton(PJW.Book.StartAnim.YISHU_HAITAN.Split('_')[0]);
                    currentClickedButton = yishu;
                    break;
                case "kexue":
                    SpeciaButton(PJW.Book.StartAnim.KEXUE_KEJICHENG.Split('_')[0]);
                    currentClickedButton = kexue;
                    break;
                default:
                    break;
            }
            if (currentClickedButton != null)
                currentClickedButton.DOScale(buttonClickedScale, 0.6f);
        }

        /// <summary>
        /// 当按科学和艺术时需要进行不同的操作,先打开选择UI
        /// </summary>
        /// <param name="name"></param>
        public void SpeciaButton(string name)
        {
            Debug.Log(" TODO " + name);
        }

        /// <summary>
        /// 书籍类型按钮点击事件
        /// </summary>
        /// <param name="name"></param>
        public void ClassButton(string name)
        {
            if (name.Equals(currentName)) return;

            currentName = name;
            FindObjectOfType<Scene2Back>().CameraStartOrOverAnimation(false);

            //FindObjectOfType<LoadAllBookXML>().GenerateAllBook(name);
            FindObjectOfType<TitlePanel>().OverAnim();
            FindObjectOfType<ClassTypePanel>().OverAnim();
            FindObjectOfType<ClassTypePanel>().ChangeClassTypsSprites(name);
            OverAnim(name);
        }

        public override void OverAnim(string nextName)
        {
            StartCoroutine(NextClass(nextName));
        }

        private IEnumerator NextClass(string nextName)
        {
            yield return new WaitForSeconds(1f);
            ButtonClickedByName(nextName);
            FindObjectOfType<Scene2Back>().CameraStartOrOverAnimation(true);
            
            FindObjectOfType<TitlePanel>().ChangeTitleSprite(nextName);
        }
    }
}
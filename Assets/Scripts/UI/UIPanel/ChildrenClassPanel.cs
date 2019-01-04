using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PJW.Book.UI
{
    /// <summary>
    /// 科目类别下级目录
    /// </summary>
    public class ChildrenClassPanel : BasePanel
    {
        private Button btn1;
        private Button btn2;
        private Image btn1Image;
        private Image btn2Image;
        private string currentName;
        private Scene2Back Scene2Back;
        private TitlePanel TitlePanel;
        private ClassTypePanel ClassTypePanel;
        public override void Init()
        {
            TitlePanel = FindObjectOfType<TitlePanel>();
            ClassTypePanel = FindObjectOfType<ClassTypePanel>();
            Scene2Back = FindObjectOfType<Scene2Back>();
            btn1 = transform.Find("btn1").GetComponent<Button>();
            btn2 = transform.Find("btn2").GetComponent<Button>();
            btn1Image = btn1.gameObject.GetComponent<Image>();
            btn2Image = btn2.gameObject.GetComponent<Image>();
            Reset(Vector3.zero,0f);
        }
        public override void Reset(Vector3 scale, float t, string msg = "")
        {
            if (!string.IsNullOrEmpty(msg))
            {
                GameCore.Instance.NewGenerateBookstore.isCanClickBook = false;
                switch (msg)
                {
                    case "kexue":
                        btn1.onClick.AddListener(() => ButtonHandle("shuxue"));
                        btn1Image.sprite = Resources.Load<Sprite>("UISprites/Bookstore/Class/lingyu_icon_shuxue_01");
                        btn2.onClick.AddListener(() => ButtonHandle("tanjiu"));
                        break;
                    case "yishu":
                        btn1.onClick.AddListener(() => ButtonHandle("meishu"));
                        //btn1Image.sprite = Resources.Load<Sprite>("UISprites/Bookstore/Class/lingyu_icon_shuxue_01");
                        btn2.onClick.AddListener(() => ButtonHandle("yinyue"));
                        break;
                }
            }
            else
            {
                GameCore.Instance.NewGenerateBookstore.isCanClickBook = true;
                btn1.onClick.RemoveAllListeners();
                btn2.onClick.RemoveAllListeners();
            }
            transform.DOScale(scale, t);
        }

        private void ChangeClassTypsSprites(string name)
        {
            if (name.Equals(currentName)) return;
            Debug.Log(name);
            currentName = name;
            string temp = name.Split('/')[0];
            if (Scene2Back.canAnim)
            {
                TitlePanel.OverAnim();
                ClassTypePanel.OverAnim();
                ClassTypePanel.ChangeClassTypsSprites(name);
                Scene2Back.CameraStartOrOverAnimation(false);
                StartCoroutine(NextClass(temp));
            }
        }

        private IEnumerator NextClass(string nextName)
        {
            yield return new WaitForSeconds(1f);

            Scene2Back.CameraStartOrOverAnimation(true);

            TitlePanel.ChangeTitleSprite(nextName);
        }

        private void ButtonHandle(string v)
        {
            Reset(Vector3.zero,0.3f);
            switch (v)
            {
                case "shuxue":
                    ChangeClassTypsSprites("kexue/shuxue");
                    break;
                case "tanjiu":
                    ChangeClassTypsSprites("kexue/tanjiu");
                    break;
                case "meishu":
                    ChangeClassTypsSprites("yishu/meishu");
                    break;
                case "yinyue":
                    ChangeClassTypsSprites("yishu/yinyue");
                    break;
            }
        }
    }
}
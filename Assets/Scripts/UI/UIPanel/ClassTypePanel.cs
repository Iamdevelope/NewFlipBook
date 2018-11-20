using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PJW.Book.UI
{
    /// <summary>
    /// 课程科目面板
    /// </summary>
    public class ClassTypePanel : BasePanel
    {
        private RectTransform children;
        private float startPos;
        private RectTransform classL;
        private RectTransform classM;
        private RectTransform classS;
        private RectTransform currentClickedButton;
        private Vector3 buttonClickedScale = new Vector3(1.5f, 1.5f, 1.5f);
        private string bookName;
        private bool isButton;
        public override void Init()
        {
            children = transform.Find("BG").GetComponent<RectTransform>();
            classL = children.Find("L").GetComponent<RectTransform>();
            classM = children.Find("M").GetComponent<RectTransform>();
            classS = children.Find("S").GetComponent<RectTransform>();
            startPos = children.localPosition.x;

            classL.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(classL.gameObject.name));
            classM.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(classM.gameObject.name));
            classS.GetComponent<Button>().onClick.AddListener(() => ButtonClickedByName(classS.gameObject.name));
        }

        private void ButtonClickedByName(string name)
        {
            if (currentClickedButton != null)
            {
                //if (currentClickedButton.name.Equals(name)) return;
                isButton = true;
                currentClickedButton.DOScale(Vector3.one, 0.3f);
            }
            switch (name)
            {
                case "L":
                    ClassTypeButton("L");
                    currentClickedButton = classL;
                    break;
                case "M":
                    ClassTypeButton("M");
                    currentClickedButton = classM;
                    break;
                case "S":
                    ClassTypeButton("S");
                    currentClickedButton = classS;
                    break;
                default:
                    break;
            }
            if (currentClickedButton != null)
                currentClickedButton.DOScale(buttonClickedScale, 0.6f);
        }

        /// <summary>
        /// 班级类型按钮点击事件
        /// </summary>
        /// <param name="name"></param>
        private void ClassTypeButton(string name)
        {
            FindObjectOfType<LoadAllBookXML>().GenerateAllBook(bookName, name);
            if (isButton)
            {
                FindObjectOfType<Scene2Back>().CameraStartOrOverAnimation(false);
                StartCoroutine(CameraInScene());
            }
        }

        private IEnumerator CameraInScene()
        {
            yield return new WaitForSeconds(1f);
            FindObjectOfType<Scene2Back>().CameraStartOrOverAnimation(true);
        }

        /// <summary>
        /// 根据书类型，更换书的精灵
        /// </summary>
        /// <param name="name">书类型名字</param>
        public void ChangeClassTypsSprites(string name)
        {

            isButton = false;
            bookName = name;
            Sprite[] temps = Resources.LoadAll<Sprite>("UISprites/Bookstore/ClassType/" + name + "/");
            for (int i = 0; i < temps.Length; i++)
            {
                string n = temps[i].name.Split('_')[2];
                switch (n)
                {
                    case "L":
                        classL.GetComponent<Image>().sprite = temps[i];
                        break;
                    case "M":
                        classM.GetComponent<Image>().sprite = temps[i];
                        break;
                    case "S":
                        classS.GetComponent<Image>().sprite = temps[i];
                        break;
                }
            }
            StartAnim();
        }

        public override void StartAnim()
        {
            //默认班级为小班
            ButtonClickedByName("S");
            
        }
        public override void OverAnim()
        {
            children.DOLocalMoveX(startPos, 1f).OnComplete(() => children.DOLocalMoveX(300, 1f));
        }
    }
}
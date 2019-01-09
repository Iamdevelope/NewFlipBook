
using System.Collections;
using System.Collections.Generic;
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
        private const string L = "L";
        private const string M = "M";
        private const string S = "S";

        private Dictionary<string, Sprite[]> nameSprite;
        private RectTransform children;
        private float startPos;
        private RectTransform classL;
        private RectTransform classM;
        private RectTransform classS;
        private RectTransform currentClickedButton;
        private Vector3 buttonClickedScale = new Vector3(1.5f, 1.5f, 1.5f);
        private string bookName;
        private bool isButton;
        public Material material;
        private Scene2Back Scene2Back;
        private LoadAllBookXML LoadAllBookXML;
        public override void Init()
        {
            LoadAllBookXML = FindObjectOfType<LoadAllBookXML>();
            Scene2Back = FindObjectOfType<Scene2Back>();
            nameSprite = new Dictionary<string, Sprite[]>();
            children = transform.Find("BG").GetComponent<RectTransform>();
            classL = children.Find(L).GetComponent<RectTransform>();
            classM = children.Find(M).GetComponent<RectTransform>();
            classS = children.Find(S).GetComponent<RectTransform>();
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
                currentClickedButton.GetComponent<Image>().material = null;
                currentClickedButton.DOScale(Vector3.one, 0.3f);
            }
            switch (name)
            {
                case L:
                    currentClickedButton = classL;
                    break;
                case M:
                    currentClickedButton = classM;
                    break;
                case S:
                    currentClickedButton = classS;
                    break;
                default:
                    break;
            }
            if (currentClickedButton != null)
            {
                ClassTypeButton(name);
                currentClickedButton.GetComponent<Image>().material = material;
                currentClickedButton.DOScale(buttonClickedScale, 0.6f);
            }
        }

        /// <summary>
        /// 班级类型按钮点击事件
        /// </summary>
        /// <param name="name"></param>
        private void ClassTypeButton(string name)
        {
            LoadAllBookXML.GenerateAllBook(bookName, name);
            if (isButton && Scene2Back.canAnim)
            {
                Scene2Back.CameraStartOrOverAnimation(false);
                StartCoroutine(CameraInScene());
            }
        }

        private IEnumerator CameraInScene()
        {
            yield return new WaitForSeconds(1f);
            Scene2Back.CameraStartOrOverAnimation(true);
        }

        /// <summary>
        /// 根据书类型，更换书的精灵
        /// </summary>
        /// <param name="name">书类型名字</param>
        public void ChangeClassTypsSprites(string name)
        {

            isButton = false;
            bookName = name;
            if(!nameSprite.ContainsKey(name))
                nameSprite[name]= UnityEngine.Resources.LoadAll<Sprite>("UISprites/Bookstore/ClassType/" + name + "/");
            for (int i = 0; i < nameSprite[name].Length; i++)
            {
                string n = nameSprite[name][i].name.Split('_')[2];
                switch (n)
                {
                    case L:
                        classL.GetComponent<Image>().sprite = nameSprite[name][i];
                        break;
                    case M:
                        classM.GetComponent<Image>().sprite = nameSprite[name][i];
                        break;
                    case S:
                        classS.GetComponent<Image>().sprite = nameSprite[name][i];
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
            children.DOLocalMoveX(startPos, 1f).OnComplete(() => children.DOMoveX(360, 1f));
        }
    }
}
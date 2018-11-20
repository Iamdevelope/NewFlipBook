using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using System.Xml; 
using System.IO;
using System;
using MyCommon;
using UnityEngine.SceneManagement;

namespace PJW.Book
{

    public class TouchInfo
    {
        public Vector2 touchPosition;
        public bool swipeComplete;
        public float timeSwipeStarted;
    }

    public class BookEdit : MonoBehaviour,ICanReset
    {

        /// <summary>
        /// 书本中页码对应该页的可交互物体
        /// </summary>
        public Dictionary<int, List<GameObject>> bookMap = new Dictionary<int, List<GameObject>>();
        [HideInInspector]
        /// <summary>
        /// 已经显示的物体
        /// </summary>
        public List<GameObject> showObject = new List<GameObject>();
        /// <summary>
        /// 存放左边的页面
        /// </summary>
        public Stack<GameObject> leftPages = new Stack<GameObject>();
        /// <summary>
        /// 存放右边的页面
        /// </summary>
        public Stack<GameObject> rightPages = new Stack<GameObject>();
        [Tooltip("右边页面的预制件")]
        /// <summary>
        /// 右边页面的预制件
        /// </summary>
        public GameObject rightPagePrefab;
        [Tooltip("左边页面的预制件")]
        /// <summary>
        /// 左边页面的预制件
        /// </summary>
        public GameObject leftPagePrefab;
        [Tooltip("右边页面的父物体")]
        public Transform rightPageParent;
        [Tooltip("左边页面的父物体")]
        public Transform leftPageParent;
        private int leftPos = 0;
        private int rightPos = 0;
        /// <summary>
        /// 书是否读完
        /// </summary>
        [HideInInspector]
        public bool bookIsClose;

        //Do not remove or change pages
        public GameObject[] pages;
        //number of book pages. Either set manually or will load dynamically depending on xml parameter selection (see bellow).Must be even. 
        public int pagesnumber = 20;
        //The array that the book pages are loaded
        //Can be loaded manually from editor or dynamicaly from Web or Resources folder . Must be even. 
        public Texture2D[] pagestextures;
        //Speed of page turning
        public float timepagesturn = 0.2f;
        //The current page of the book at runtime
        public int currentpage = 0;
        //Disables gui button during page turn
        public bool nextbutton = false;
        //Disables back button if current page =0
        public bool backbutton = false;
        //If set to true the pages are loaded from XML file Local or web 
        public bool xml = false;
        //Set Localxml to true if you want to read xml file from resources foldes. The name of the xml file must be images1.xml
        public bool Localxml = false;
        //Set to XMLURL the url of the XML if LocalXml = false to read from the Web. Cross Domain policy needs to be valid. 
        public string XMLURL;
        //The Xml File Name of the local XML
        public string localXMLFile;
        public List<Texture2D> img_list = new List<Texture2D>();
        //Determines if the size of the book will be dynamic
        public bool EnableBookSize = false;
        //chapters
        public string[] chapters;
        public int[] chapterPages;
        private bool chapterson = false;
        public bool ChaptersFromXML = false;
        //Saves and retrieves the current page of the user
        public bool EnableCurrentPage = false;
        public bool EnableBookstore = false;
        //ios enable booleans
        public bool portraitenable = false;
        private string booktitle;
        private bool downloadiconbool;
        public static BookEdit Instance;
        
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            booktitle = PlayerPrefs.GetString("BookName");

            if (EnableBookSize)
            {
                GameObject.Find("Book").transform.localScale = new Vector3(float.Parse(PlayerPrefs.GetString("BookX")), 1f, float.Parse(PlayerPrefs.GetString("BookY")));
                GameObject.FindWithTag("portrait").transform.localScale = new Vector3(float.Parse(PlayerPrefs.GetString("BookX")), 1f, float.Parse(PlayerPrefs.GetString("BookY")));
            }
        }
        void Start()
        {
            //
            StartCoroutine(LoadBookPages());
            

            if (xml == false)
            {
                GameObject.FindWithTag("back1").GetComponent<Renderer>().material.mainTexture = pagestextures[currentpage];
                GameObject.FindWithTag("portrait2").GetComponent<Renderer>().material.mainTexture = pagestextures[currentpage];
            }
            GameObject.FindWithTag("in2").GetComponent<Renderer>().enabled = false;
            for (int i = 0; i <= 22; i++)
            {
                pages[i].GetComponent<Renderer>().enabled = false;
            }
        }

        void LateUpdate()
        {
            if (portraitenable == true)
            {
                if (currentpage != 0)
                {
                    GameObject.FindWithTag("in2").GetComponent<Renderer>().enabled = true;
                }
            }

            if (currentpage != 0)
            {
                //GameObject.FindWithTag("portrait2").GetComponent<Renderer>().material.mainTexture = pagestextures[currentpage - 1];
            }
            else
            {
                if (currentpage > 0)
                {
                    GameObject.FindWithTag("portrait2").GetComponent<Renderer>().material.mainTexture = pagestextures[currentpage];
                }
            }
            //if it is the first page of the book disable the back button
            if (currentpage == 0)
            {
                backbutton = false;
            }
            else
            {
                backbutton = true;
            }
            
        }
        private IEnumerator LoadBookPages()
        {
            downloadiconbool = true;
            if (xml == true)
            {
                Array.Clear(pagestextures, 0, pagestextures.Length);
                Array.Resize(ref pagestextures, pagestextures.Length - pagesnumber);
                pagesnumber = 0;
                string URLString3 = "";

                int i = 0;
                if (ChaptersFromXML == true)
                {
                    Array.Clear(chapters, 0, chapters.Length);
                    Array.Clear(chapterPages, 0, chapterPages.Length);
                }
                if (Localxml)
                {
                    if (EnableBookstore)
                    {
                        URLString3 = "file://" + Application.dataPath + "/Resources/Files/" + PlayerPrefs.GetString("BookXML");
                    }
                    else
                    {
                        URLString3 = "file://" + Application.dataPath + "/Resources/Files/" + localXMLFile;
                    }
                    //need to bypass xml reading just do it manually for ios devices
                    //loop thru the list
                    Array.Resize(ref pagestextures, 1);
                    

                    if (currentpage == 0)
                    {
                        GameObject.FindWithTag("back1").GetComponent<Renderer>().material.mainTexture = pagestextures[0];
                    }
                }
                else
                {
                    //Xml file from URL 
                    if (EnableBookstore)
                    {
                        URLString3 = PlayerPrefs.GetString("BookXML");
                    }
                    else
                    {
                        URLString3 = "" + XMLURL + "";
                    }
                }

                WWW ww3 = new WWW(URLString3);
                yield return ww3;
                string XmlString = ww3.text;
                XmlDocument XmlData = new XmlDocument();
                XmlData.LoadXml(XmlString);
                XmlElement root3 = XmlData.DocumentElement;

                //resizes array of images to the XML page Nodes
                Array.Resize(ref pagestextures, pagestextures.Length + root3.ChildNodes.Count);

                GameObject parent = new GameObject("parent");
                foreach (XmlNode thisnode in root3.ChildNodes)
                {
                    WWW w = null;
                    if (Localxml)
                    {
                        //Loads Images from resources folder
                        w = new WWW("file://" + Application.dataPath + "/Resources/Files/" + thisnode.Attributes["name"].Value);
                        //
                        if (thisnode.ChildNodes.Count > 0)
                        {
                            bookMap[int.Parse(thisnode.Attributes["count"].Value)] = new List<GameObject>();
                            foreach (XmlNode xmlNode in thisnode.ChildNodes)
                            {
                                string temp = xmlNode.Attributes["name"].Value;
                                GameObject go = Resources.Load<GameObject>("Prefabs/Books/" + root3.Attributes["bookName"].Value + "/" + temp);
                                GameObject tempGo = Instantiate(go);
                                tempGo.name = temp;
                                tempGo.transform.parent = parent.transform;
                                //tempGo.GetComponent<InterableObject>().MoveLeave();
                                tempGo.SetActive(false);
                                bookMap[int.Parse(thisnode.Attributes["count"].Value)].Add(tempGo);
                            }
                        }
                    }
                    else
                    {
                        //Loads Images from the Web URL stated in the Xml name attribute
                        w = new WWW(thisnode.Attributes["name"].Value);
                    }
                    yield return w;
                    Texture2D tex = new Texture2D(4,4);
                    w.LoadImageIntoTexture(tex);
                    pagestextures[i] = tex;
                    i++;
                    pagesnumber++;
                    if (currentpage == 0)
                    {
                        GameObject.FindWithTag("back1").GetComponent<Renderer>().material.mainTexture = pagestextures[0];
                        GameObject.FindWithTag("portrait2").GetComponent<Renderer>().material.mainTexture = pagestextures[0];
                    }
                }
                downloadiconbool = false;
            }

            //UIManager.Instance.ShowAllPage(pagesnumber - 2);
            ResourcesAllPage();

            if (EnableCurrentPage) { FindAndGoCurrentPagePlayerPrefs(); }
        }
        /// <summary>
        /// Previous page Generator
        /// </summary>
        /// <returns></returns>
        private IEnumerator leftpage()
        {

            if (showObject.Count > 0)
            {
                //
                if (!ViewController.Instance.is3DView)
                    ViewController.Instance.ChangeCommonView();
                for (int i = 0; i < showObject.Count; i++)
                {
                    //showObject[i].GetComponent<InterableObject>().MoveLeave();
                    showObject[i].SetActive(false);
                }
            }

            ResourcesLeftPage();

            yield return new WaitForSeconds(0);

            for (int i = 0; i <= 22; i++)
            {
                yield return new WaitForSeconds(timepagesturn);
                if (currentpage + 2 >= pagesnumber)
                {
                    GameObject.FindWithTag("back1").GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    GameObject.FindWithTag("back1").GetComponent<Renderer>().enabled = true;
                    GameObject.FindWithTag("back1").GetComponent<Renderer>().material.mainTexture = pagestextures[currentpage + 2];
                }

                pages[i].GetComponent<Renderer>().materials[0].mainTexture = pagestextures[currentpage];
                Debug.Log("这是当前的页面数：" + currentpage);
                pages[i].GetComponent<Renderer>().materials[1].mainTexture = pagestextures[currentpage+1];
                pages[i].GetComponent<Renderer>().enabled = true;
                if (i >= 1)
                {
                    pages[i - 1].GetComponent<Renderer>().enabled = false;
                }
            }
            //clickIndex = 0;
            InterableObject.Instance.isTrigger = true;
            currentpage += 2;
            //if (currentpage > pagesnumber - 2)
            //    UIManager.Instance.UpdatePageNum(0);
            //else
            //    UIManager.Instance.UpdatePageNum(currentpage);

            //
            ShowHideObject();
            //saves the cuurent page
            if (EnableCurrentPage) SaveCurrentPagePlayerPrefs(currentpage);
        }
        /// <summary>
        /// Next page Generator
        /// </summary>
        /// <returns></returns>
        private IEnumerator rightpage()
        {
            if (showObject.Count > 0)
            {
                //
                if (!ViewController.Instance.is3DView)
                    ViewController.Instance.ChangeCommonView();
                for (int i = 0; i < showObject.Count; i++)
                {
                    //showObject[i].GetComponent<InterableObject>().MoveLeave();
                    showObject[i].SetActive(false);
                }
            }

            ResourcesRightPage();

            yield return new WaitForSeconds(0);
            for (int i = 22; i >= 0; i--)
            {
                yield return new WaitForSeconds(timepagesturn);
                if (currentpage - 2 <= 0)
                {
                    GameObject.FindWithTag("in2").GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    GameObject.FindWithTag("in2").GetComponent<Renderer>().enabled = true;
                    if (currentpage == pagesnumber)
                    {
                        GameObject.FindWithTag("in2").GetComponent<Renderer>().materials[1].mainTexture = pagestextures[currentpage - 3];
                    }
                    else
                    {
                        GameObject.FindWithTag("in2").GetComponent<Renderer>().materials[1].mainTexture = pagestextures[currentpage - 3];
                    }
                }
                if (currentpage > 1)
                {
                    pages[i].GetComponent<Renderer>().materials[0].mainTexture = pagestextures[currentpage - 2];
                    pages[i].GetComponent<Renderer>().materials[1].mainTexture = pagestextures[currentpage - 1];
                }
                else
                {
                    pages[i].GetComponent<Renderer>().materials[0].mainTexture = pagestextures[currentpage];
                }
                
                pages[i].GetComponent<Renderer>().enabled = true;
                if (i < 22)
                {
                    pages[i + 1].GetComponent<Renderer>().enabled = false;
                }
            }
            //clickIndex = 0;
            InterableObject.Instance.isTrigger = true;
            if (currentpage > 1)
            {
                currentpage -= 2;
            }
            //if (currentpage >= 0)
            //    UIManager.Instance.UpdatePageNum(currentpage);

            //
            ShowHideObject();
            if (EnableCurrentPage) SaveCurrentPagePlayerPrefs(currentpage);
        }
        private void SaveCurrentPagePlayerPrefs(int currentbookpage)
        {
            if (EnableBookstore)
            {
                PlayerPrefs.SetInt(PlayerPrefs.GetString("BookName") + "currentpage", currentbookpage);
            }
            else
            {
                PlayerPrefs.SetInt("currentpage", currentbookpage);
            }
        }
        private void FindAndGoCurrentPagePlayerPrefs()
        {
            if (EnableBookstore)
            {
                currentpage = PlayerPrefs.GetInt(PlayerPrefs.GetString("BookName") + "currentpage");
            }
            else
            {
                currentpage = PlayerPrefs.GetInt("currentpage");
            }
            if (currentpage > 0)
            {
                currentpage = currentpage - 2;
                chapterson = false;
                StartCoroutine(leftpage());
                StartCoroutine(buttontime());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResourcesAllPage()
        {
            for (int i = 0; i < pagesnumber / 2 - 1; i++)
            {
                rightPos++;
                GameObject go = GameObjectPool.Instance.CreateObject("right", rightPagePrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
                go.transform.parent = rightPageParent;
                go.transform.localPosition = new Vector3(0, 0, -0.01f * i);
                rightPages.Push(go);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResourcesRightPage(bool isSkip=false)
        {
            if (leftPages.Count <= 0) return;
            Debug.Log("the page in right num " + rightPages.Count);
            GameObject go = leftPages.Pop();
            //
            GameObjectPool.Instance.CollectObject(go);
            GameObject temp = GameObjectPool.Instance.CreateObject("right", rightPagePrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
            if (!isSkip)
                temp.SetActive(false);
            StartCoroutine(HidePage(temp));
            temp.transform.parent = rightPageParent;
            temp.transform.localPosition = new Vector3(0, 0, -0.01f * rightPos);
            rightPages.Push(temp);
            rightPos++;
            leftPos--;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResourcesLeftPage(bool isSkip=false)
        {
            if (rightPages.Count <= 0) return;
            Debug.Log("the page in left num " + leftPages.Count);
            leftPos++;
            rightPos--;
            GameObject go = rightPages.Pop();
            //
            GameObjectPool.Instance.CollectObject(go);
            GameObject temp = GameObjectPool.Instance.CreateObject("left", leftPagePrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
            if (!isSkip)
                temp.SetActive(false);
            StartCoroutine(HidePage(temp));
            temp.transform.parent = leftPageParent;
            temp.transform.localPosition = new Vector3(0, -0.01f * leftPos, 0);
            leftPages.Push(temp);
        }
        private IEnumerator HidePage(GameObject go)
        {
            yield return new WaitForSeconds(1f);
            go.SetActive(true);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ShowHideObject()
        {
            float time = 0;
            //
            showObject.Clear();
            if (bookMap.ContainsKey(currentpage))
            {
                //
                if (ViewController.Instance.is3DView)
                    ViewController.Instance.Change3DView(time,() =>
                    {
                        for (int i = 0; i < bookMap[currentpage].Count; i++)
                        {
                            //bookMap[currentpage][i].GetComponent<InterableObject>().Move();
                            bookMap[currentpage][i].SetActive(true);
                            showObject.Add(bookMap[currentpage][i]);
                        }
                    });
            }
            if (bookMap.ContainsKey(currentpage - 1))
            {
                if (ViewController.Instance.is3DView)
                    ViewController.Instance.Change3DView(time,() =>
                    {
                        for (int i = 0; i < bookMap[currentpage - 1].Count; i++)
                        {
                            //bookMap[currentpage - 1][i].GetComponent<InterableObject>().Move();
                            bookMap[currentpage - 1][i].SetActive(true);
                            showObject.Add(bookMap[currentpage - 1][i]);
                        }
                    });
            }
        }
        public void RightFlipPage()
        {
            //
            if (currentpage <= 0) return;
            CanFlipPage(-3,-2,rightpage());
        }
        public void LeftFlipPage()
        {
            //
            if (currentpage >= pagesnumber) return;
            CanFlipPage(1, 2, leftpage());
        }
        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="coroutine"></param>
        private void CanFlipPage(int num1,int num2,IEnumerator coroutine)
        {
            if (!InterableObject.Instance.isTrigger) return;
            // if (currentpage == 0)
            //     ViewController.Instance.anim.SetBool("startRead", true);
            if (currentpage == pagesnumber - 2)
                ViewController.Instance.OverAnim();
            
            if (IsChangeView(num1, num2))
            {
                Debug.Log("this is should right flip page");
                ViewController.Instance.is3DView = true;
            }
            else
            {
                Debug.Log("this should not right flip page");
                ViewController.Instance.is3DView = false;
            }
            //clickIndex++;
            InterableObject.Instance.isTrigger = false;
            StartCoroutine(coroutine);
            StartCoroutine(buttontime());
        }
        /// <summary>
        /// 判断是否需要改变视野
        /// </summary>
        /// <returns></returns>
        private bool IsChangeView(int x,int y)
        {
            return bookMap.ContainsKey(currentpage + x) || bookMap.ContainsKey(currentpage + y);
        }
        /// <summary>
        /// 跳转页面
        /// </summary>
        public void SkipCurrentPage(int page)
        {
            if (page.CompareTo(currentpage) >= 1)
            {
                for (int i = 0; i < (page - currentpage - 2) / 2; i++)
                {
                    ResourcesLeftPage(true);
                }
                currentpage = page - 2;
                StartCoroutine(leftpage());
            }
            else
            {
                for (int i = 0; i < (currentpage - page + 2) / 2;i++)
                {
                    ResourcesRightPage(true);
                }
                currentpage = page + 2;
                StartCoroutine(rightpage());
            }
        }
        public void SkipCurrentPageSync(int page)
        {
            if (page % 2 == 1) page += 1;
            //如果输入的页码不符合，不做处理
            if (page < 0 || page > pagesnumber) return;
            //如果输入的页码和当前页码相同，不做处理
            if (page.CompareTo(currentpage) == 0) return;
            SkipCurrentPage(page);
            if (page.CompareTo(currentpage) >= 1)
                StartCoroutine(NotName(page));
            else
                StartCoroutine(NotName(page));
        }
        private IEnumerator NotName(int page)
        {
            yield return new WaitForSeconds(0.55f);
            SkipCurrentPageSync(page);
        }
        IEnumerator buttontime()
        {
            nextbutton = false;
            yield return new WaitForSeconds(1.5f);
            nextbutton = true;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            SkipCurrentPage(pagesnumber);
            bookIsClose = true;
        }
    }
}
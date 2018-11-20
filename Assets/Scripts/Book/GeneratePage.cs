using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MyCommon;
using System.Xml;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.Video;
using DG.Tweening;
using System.Text.RegularExpressions;

namespace PJW.Book{
    /// <summary>
    /// 生成书本的页面
    /// </summary>
    public class GeneratePage : MonoBehaviour {

        public Dictionary<int,List<GameObject>> bookMap=new Dictionary<int, List<GameObject>>();
        public Dictionary<int, GameObject> spriteMap = new Dictionary<int, GameObject>();
        //书中的所有按钮的精灵
        private Dictionary<int, List<GameObject>> uiBtnSpriteMap = new Dictionary<int, List<GameObject>>();
        //书中的所有视频文件
        private Dictionary<int, List<VideoClip>> bookVideoClip = new Dictionary<int, List<VideoClip>>();
        //书中所有的音频文件
        public List<AudioClip> bookSoundClip = new List<AudioClip>();
        public List<GameObject> showObject = new List<GameObject>();
        public List<GameObject> uiBtnSprites = new List<GameObject>();
        public List<GameObject> spriteObject = new List<GameObject>();
        //[Tooltip("默认本地文件XML文件")]
        //public string localXMLFile;
        [Tooltip("书中每页的图片内容")]
        public Texture[] pagestextures;
        [Tooltip("书的页面数")]
        public int pagesnumber;
        [Tooltip("当前页码")]
        public int currentpage = 1;
        [Tooltip("右边页的预制件")]
        public GameObject rightPagePrefab;
        [Tooltip("左边页的预制件")]
        public GameObject leftPagePrefab;
        [Tooltip("右边页面的父物体")]
        public Transform rightPageParent;
        [Tooltip("左边页面的父物体")]
        public Transform leftPageParent;
        [Tooltip("封面页面")]
        public GameObject firstPage;
        [Tooltip("尾页")]
        public GameObject endPage;
        [Tooltip("2d精灵物体的父物体")]
        public GameObject SpriteParent;
        [HideInInspector]
        public GameObject[] rightPages;
        [HideInInspector]
        public GameObject[] leftPages;
        [HideInInspector]
        public bool isRightFlipPage;
        [HideInInspector]
        public bool isLeftFlipPage;
        [HideInInspector]
        public AudioSource audioSource;
        public VideoPlayer player;
        [HideInInspector]
        public SoundManager soundManager;
        public bool notGenerateSoundManager;

        private void Awake()
        {
            rightPages = new GameObject[2];
            leftPages = new GameObject[2];
            GameCore.Instance.effectPositionZ = 11;
            GameCore.Instance.GeneratePage = this;
        }

        private void Start()
        {
            if (player != null)
            {
                player.Stop();
                player.GetComponent<RectTransform>().DOScale(0, 0);
            }
            if (notGenerateSoundManager)
            {
                GameCore.Instance.GeneratePage.player.SetTargetAudioSource(0, GameCore.Instance.SoundManager.source);
            }
        }
        /// <summary>
        /// 通过Resources加载资源
        /// </summary>
        /// <param name="localfile"></param>
        /// <returns></returns>
        public void LoadBook(string localfile)
        {
            if (!notGenerateSoundManager)
            {
                audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                player.SetTargetAudioSource(0, audioSource);
                soundManager = audioSource.gameObject.AddComponent<SoundManager>();
                soundManager.Init();
            }
            
            #region Resources加载
            Array.Clear(pagestextures, 0, pagestextures.Length);
            Array.Resize(ref pagestextures, pagestextures.Length - pagesnumber);
            pagesnumber = 0;
            XDocument document = XDocument.Load(localfile);
            XElement root = document.Root;
            XElement ele = root.Element("Pages");
            IEnumerable<XElement> xElements = ele.Elements();
            Array.Resize(ref pagestextures, pagestextures.Length + ele.Elements().Count());
            GameObject parent = new GameObject("parent");
            string bookName = ele.Attribute("bookName").Value.Split('.')[0];
            foreach (XElement item in xElements)
            {
                //记录下该本书上所有的可交互对象
                if (int.Parse(item.Attribute("count").Value) > 0)
                {
                    bookMap[int.Parse(item.Attribute("count").Value)] = new List<GameObject>();
                    foreach (var xmlNode in item.Elements())
                    {
                        if (xmlNode.Attribute("objectName") != null)
                        {
                            string temp = xmlNode.Attribute("objectName").Value;
                            temp = temp.Split('.')[0];

                            //GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Books/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Object/" + temp);
                            GameObject go = Resources.Load<GameObject>("Books/" + bookName + "/Object/" + temp);
                            GameObject tempGo = Instantiate(go);
                            if (tempGo.GetComponent<InterableObject>())
                                tempGo.GetComponent<InterableObject>().GenerateEvent();
                            tempGo.name = temp;
                            tempGo.transform.parent = parent.transform;
                            tempGo.SetActive(false);
                            bookMap[int.Parse(item.Attribute("count").Value)].Add(tempGo);
                        }
                    }
                }
                //记录下该本书上的所有精灵对象
                if (int.Parse(item.Attribute("spriteCount").Value) > 0)
                {
                    foreach (var xmlNode in item.Elements())
                    {
                        //if (string.IsNullOrEmpty(xmlNode.Value) continue;
                        if (xmlNode.Attribute("spriteName") != null)
                        {
                            string temp = xmlNode.Attribute("spriteName").Value;
                            temp = temp.Split('.')[0];
                            //GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Books/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Sprites/" + temp);
                            GameObject go = Resources.Load<GameObject>("Books/" + bookName + "/Sprites/" + temp);
                            GameObject tempGo = Instantiate(go, SpriteParent.transform);
                            if (tempGo.GetComponent<InterableObject>())
                                tempGo.GetComponent<InterableObject>().GenerateEvent(bookName);
                            tempGo.name = temp;
                            tempGo.SetActive(false);
                            spriteMap[int.Parse(item.Attribute("spriteCount").Value)] = tempGo;
                        }
                    }
                }
                //记录下该本书中的所有声音
                if (!string.IsNullOrEmpty(item.Attribute("sound").Value))
                {
                    AudioClip clip = Resources.Load<AudioClip>("Books/" + bookName + "/Sound/" + item.Attribute("sound").Value);
                    clip.name = item.Attribute("sound").Value;
                    bookSoundClip.Add(clip);
                }
                //记录下该本书中的所有视频文件
                if (int.Parse(item.Attribute("videoCount").Value) > 0)
                {
                    bookVideoClip[int.Parse(item.Attribute("videoCount").Value)] = new List<VideoClip>();
                    foreach (var xmlNode in item.Elements())
                    {
                        if (xmlNode.Attribute("videoName") != null)
                        {
                            string temp = xmlNode.Attribute("videoName").Value;
                            temp = temp.Split('.')[0];

                            VideoClip videoClip = Resources.Load<VideoClip>("Books/" + bookName + "/Video/" + temp);
                            bookVideoClip[int.Parse(item.Attribute("videoCount").Value)].Add(videoClip);
                        }
                    }
                }
                //记录下该本书中的所有的按钮的精灵对象
                if (int.Parse(item.Attribute("uiButtonSpriteCount").Value) > 0)
                {
                    uiBtnSpriteMap[int.Parse(item.Attribute("uiButtonSpriteCount").Value)] = new List<GameObject>();
                    foreach (var xmlNode in item.Elements())
                    {
                        if (xmlNode.Attribute("uiButtonSpriteName") != null)
                        {
                            string temp = xmlNode.Attribute("uiButtonSpriteName").Value;
                            temp = temp.Split('.')[0];

                            //GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Books/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Object/" + temp);
                            GameObject go = Resources.Load<GameObject>("Books/" + bookName + "/UIButtonSprite/" + temp);
                            GameObject tempGo = Instantiate(go);
                            tempGo.name = temp;
                            if (tempGo.GetComponent<InterableObject>())
                                tempGo.GetComponent<InterableObject>().GenerateEvent(player, bookVideoClip[int.Parse(item.Attribute("uiButtonSpriteCount").Value)]);
                            tempGo.transform.parent = parent.transform;
                            tempGo.SetActive(false);
                            uiBtnSpriteMap[int.Parse(item.Attribute("uiButtonSpriteCount").Value)].Add(tempGo);
                        }
                    }
                }
                Texture t = Resources.Load<Texture>("Books/" + bookName + "/Textures/" + item.Attribute("pageName").Value);
                pagestextures[pagesnumber] = t;
                pagesnumber++;
            }
            #endregion

            #region PC端
            //                string URLString3 = "";
            //#if UNITY_ANDROID
            //                if (loadByStore)
            //                {
            //                    URLString3 = "jar:file:///" + Application.persistentDataPath + "/Books/XMLContent/" + PlayerPrefs.GetString("BookXML");
            //                }
            //                else
            //                {
            //                    URLString3 = "jar:file:///" + Application.persistentDataPath + "/Books/XMLContent/" + localXMLFile;
            //                }
            //                //localfile = "jar:file:///" + localfile;
            //#else
            //                if (loadByStore)
            //                {
            //                    URLString3 = Application.dataPath + "/Books/XMLContent/" + PlayerPrefs.GetString("BookXML");
            //                }
            //                else
            //                {
            //                    URLString3 = Application.dataPath + "/Books/XMLContent/" + localXMLFile;
            //                }
            //#endif
            //                WWW www = new WWW(URLString3);
            //                yield return www;
            //                string XmlString = www.text;
            //                XmlDocument XmlData = new XmlDocument();
            //                XmlData.LoadXml(XmlString);
            //                XmlElement root3 = XmlData.DocumentElement;
            //                Array.Resize(ref pagestextures, pagestextures.Length + root3.ChildNodes.Count);
            //                foreach (XmlNode thisnode in root3.ChildNodes)
            //                {

            //                    WWW w = null;
            //#if UNITY_ANDROID
            //                    w = new WWW("jar:file://" + Application.dataPath + "/Resources/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Textures/" + thisnode.Attributes["name"].Value);
            //#else
            //                    w = new WWW("file://" + Application.dataPath + "/Resources/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Textures/" + thisnode.Attributes["pageName"].Value);
            //#endif
            //                    Debug.Log("file://" + Application.dataPath + "/Resources/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Textures/" + thisnode.Attributes["pageName"].Value);
            //                    //如果当前页面有3D物体需要进行生成
            //                    if (int.Parse(thisnode.Attributes["count"].Value) > 0)
            //                    {
            //                        GameObject parent = new GameObject("parent");
            //                        bookMap[int.Parse(thisnode.Attributes["count"].Value)] = new List<GameObject>();
            //                        foreach (XmlNode xmlNode in thisnode.ChildNodes)
            //                        {
            //                            string temp = xmlNode.Attributes["objectName"].Value;
            //                            //GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Books/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Object/" + temp);
            //                            GameObject go = Resources.Load<GameObject>("Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Object/" + temp);
            //                            GameObject tempGo = Instantiate(go);
            //                            tempGo.name = temp;
            //                            tempGo.transform.parent = parent.transform;
            //                            tempGo.SetActive(false);
            //                            bookMap[int.Parse(thisnode.Attributes["count"].Value)].Add(tempGo);
            //                        }
            //                    }
            //                    if (int.Parse(thisnode.Attributes["spriteCount"].Value) > 0)
            //                    {
            //                        Debug.Log(int.Parse(thisnode.Attributes["spriteCount"].Value) + " the book have sprites of count ");
            //                        spriteMap[int.Parse(thisnode.Attributes["spriteCount"].Value)] = new List<GameObject>();
            //                        foreach (XmlNode xmlNode in thisnode.ChildNodes)
            //                        {
            //                            string temp = xmlNode.Attributes["spriteName"].Value;
            //                            //GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Books/Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Sprites/" + temp);
            //                            GameObject go = Resources.Load<GameObject>("Prefabs/Books/" + root3.Attributes["bookName"].Value + "/Sprites/" + temp);
            //                            GameObject tempGo = Instantiate(go, SpriteParent.transform);
            //                            tempGo.name = temp;
            //                            tempGo.SetActive(false);
            //                            spriteMap[int.Parse(thisnode.Attributes["spriteCount"].Value)].Add(tempGo);
            //                        }
            //                    }
            //                    yield return w;
            //                    Texture2D tex = new Texture2D(4, 4);
            //                    w.LoadImageIntoTexture(tex);
            //                    pagestextures[pagesnumber] = tex;
            //                    pagesnumber++;
            //                }
            #endregion

            CanvasController.Instance.ShowAllPage(pagesnumber - 2);
            GameCore.Instance.OpenLoadingPanel(Vector3.zero);
            ResourcesAllPages();
            //等资源加载结束了，再进行视野进行切换
            StartAnimation.Instance.Init();

        }
        /// <summary>
        /// 通过AssetBundle加载资源
        /// </summary>
        /// <param name="assetBundleFile">assetbundle文件名</param>
        public void LoadBookByAssetBundle(string assetBundleFile)
        {
            if (!notGenerateSoundManager)
            {
                audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                player.SetTargetAudioSource(0, audioSource);
                soundManager = audioSource.gameObject.AddComponent<SoundManager>();
                soundManager.Init();
            }
            //在进行资源加载之前将所有AssetBundle资源卸载，以防止资源重复加载出错
            AssetBundle.UnloadAllAssetBundles(true);
            GameCore.Instance.asset = AssetBundle.LoadFromFile(assetBundleFile);
            string[] names = GameCore.Instance.asset.GetAllAssetNames();
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = names[i].Split('/')[6];
            }
            pagesnumber = 0;
            //将文件按数字大小排序
            names = names.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Split('.')[1].Equals("jpg"))
                {
                    Array.Resize(ref pagestextures, pagestextures.Length + 1);
                    Texture t = GameCore.Instance.asset.LoadAsset<Texture>( names[i]);
                    pagestextures[pagesnumber] = t;
                    pagesnumber++;
                }
                else if (names[i].Split('.')[1].Equals("mp3"))
                {
                    AudioClip clip = GameCore.Instance.asset.LoadAsset<AudioClip>( names[i]);
                    clip.name = names[i].Split('.')[0];
                    bookSoundClip.Add(clip);
                }
                else if (names[i].Split('.')[1].Equals("mp4"))
                {
                    VideoClip videoClip = GameCore.Instance.asset.LoadAsset<VideoClip>( names[i]);
                    if (!bookVideoClip.ContainsKey(int.Parse(names[i].Split('_')[0])))
                        bookVideoClip[int.Parse(names[i].Split('_')[0])] = new List<VideoClip>();
                    bookVideoClip[int.Parse(names[i].Split('_')[0])].Add(videoClip);
                }
                else if (names[i].Split('.')[1].Equals("prefab"))
                {
                    if (names[i].Split('.')[0].Split('_')[1].Equals("bg"))
                    {
                        GameObject go = GameCore.Instance.asset.LoadAsset<GameObject>(names[i]);
                        GameObject tempGo = Instantiate(go, SpriteParent.transform);
                        if (tempGo.GetComponent<InterableObject>())
                            tempGo.GetComponent<InterableObject>().GenerateEvent(GameCore.Instance.asset.name.Split('.')[0]);
                        tempGo.name = names[i].Split('.')[0];
                        tempGo.SetActive(false);
                        spriteMap[int.Parse(names[i].Split('_')[0])] = tempGo;
                    }
                    else if (names[i].Split('.')[0].Split('_')[1].Equals("3d"))
                    {
                        if (!bookMap.ContainsKey(int.Parse(names[i].Split('_')[0])))
                            bookMap[int.Parse(names[i].Split('_')[0])] = new List<GameObject>();
                        GameObject go = GameCore.Instance.asset.LoadAsset<GameObject>(names[i]);
                        GameObject tempGo = Instantiate(go, SpriteParent.transform);
                        if (tempGo.GetComponent<InterableObject>())
                            tempGo.GetComponent<InterableObject>().GenerateEvent();
                        tempGo.name = names[i];
                        tempGo.SetActive(false);
                        Debug.Log("3DObject of name " + names[i].Split('_')[0]);
                        bookMap[int.Parse(names[i].Split('_')[0])].Add(tempGo);
                    }
                    else
                    {
                        GameObject go = GameCore.Instance.asset.LoadAsset<GameObject>(names[i]);
                        GameObject tempGo = Instantiate(go, SpriteParent.transform);
                        if (tempGo.GetComponent<InterableObject>())
                            tempGo.GetComponent<InterableObject>().GenerateEvent(player, GameCore.Instance.asset.LoadAsset<VideoClip>(names[i].Split('.')[0] + ".mp4"));
                        tempGo.name = names[i].Split('.')[0];
                        tempGo.SetActive(false);
                        if (!uiBtnSpriteMap.ContainsKey(int.Parse(names[i].Split('_')[0])))
                            uiBtnSpriteMap[int.Parse(names[i].Split('_')[0])] = new List<GameObject>();
                        uiBtnSpriteMap[int.Parse(names[i].Split('_')[0])].Add(tempGo);
                    }
                }
                CanvasController.Instance.ShowAllPage(pagesnumber);
            }
            GameCore.Instance.OpenLoadingPanel(Vector3.zero);
            ResourcesAllPages();
            //等资源加载结束了，再进行视野进行切换
            StartAnimation.Instance.Init();
        }
        /// <summary>
        /// 生成所有的页面，每本书生成两份
        /// </summary>
        private void ResourcesAllPages()
        {
            foreach(var item in firstPage.GetComponentsInChildren<MeshRenderer>()){
                if(item.name=="see")
                    item.material.SetTexture("_MainTex",pagestextures[0]);
                if(item.name=="back")
                    item.material.SetTexture("_MainTex",pagestextures[1]);
            }
            foreach(var item in endPage.GetComponentsInChildren<MeshRenderer>()){
                if(item.name=="see")
                    item.material.SetTexture("_MainTex",pagestextures[pagesnumber-2]);
                if(item.name=="back")
                    item.material.SetTexture("_MainTex",pagestextures[pagesnumber-1]);
            }
            GameObject rightGo, leftGo;
            //生成左右两边的页面
            for (int i = 2; i < 4; i++)
            {
                rightGo = GameObjectPool.Instance.CreateObject("right", rightPagePrefab, Vector3.zero, Quaternion.identity);
                rightGo.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[i - 1]);
                rightGo.GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[i]);
                rightGo.transform.parent = rightPageParent;
                rightGo.name = "rightPage" + i;
                rightGo.transform.localPosition = new Vector3(1.377f, 0.02f - 0.01f * i, 0);
                rightPages[i-2] = rightGo;

                leftGo = GameObjectPool.Instance.CreateObject("left", leftPagePrefab, Vector3.zero, Quaternion.Euler(180, 180, 0));
                leftGo.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[i - 1]);
                leftGo.GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[i]);
                leftGo.transform.parent = leftPageParent;
                leftGo.name = "leftPage" + i;
                leftGo.transform.localPosition = new Vector3(-1.377f, 0.02f - 0.01f * i, 0);
                leftPages[i-2] = leftGo;
            }
            CloseLeftPageOfActive();
        }
        /// <summary>
        /// 翻页时进行页面之间的切换及材质的更改
        /// </summary>
        /// <param name="dic">翻页的方向</param>
        public void FlipPage(int dic)
        {
            if (dic == -1) LeftClickPage();
            if (dic == 1) RightClickPage();
        }
        /// <summary>
        /// 右向左翻页
        /// </summary>
        private void RightClickPage()
        {
            //当书页翻到最后一页时，不作处理
            if (currentpage >= pagestextures.Length - 2) return;
            //对书页的特殊处理，即在翻前面两页的时候
            if (currentpage > 0 && currentpage < 3)
            {
                //控制左边页面数组是否需要反转
                if (isLeftFlipPage)
                    isLeftFlipPage = false;
                //在第一二页时，由右向左翻页，右边页面的材质的贴图需要往后延伸两个，因为在上一次翻页时，右边页面数组已经进行了反转，保证了数组中的
                //第二个元素位于下方的，所以对其进行贴图的更换
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage + 1]);
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage + 2]);
                //而左边页面由于特殊性，在翻到第三页之前，对其进行了两次数组反转，保证了数组中的第一个元素位于下方
                leftPages[0].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage]);
                leftPages[0].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage + 1]);
            }
            else
            {
                //否则就是开始正常的翻页切换了，需要在第一次时，对左边页面的数组进行反转，保证左边页面数组中的第二个元素位于下方
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage + 1]);
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage + 2]);

                if (!isRightFlipPage)
                {
                    isRightFlipPage = true;
                    Array.Reverse(leftPages);
                }
                leftPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage]);
                leftPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage + 1]);
            }
        }
        /// <summary>
        /// 左向右翻页
        /// </summary>
        private void LeftClickPage()
        {
            //对翻页中第一二页的特殊性，进行特殊处理
            if (currentpage > 0 && currentpage < 3)
            {
                //设置左边页面数组可以进行反转
                if (isRightFlipPage)
                    isRightFlipPage = false;
                //控制左边页面进行反转，且保证只能反转一次
                if (!isLeftFlipPage)
                {
                    isLeftFlipPage = true;
                    Array.Reverse(leftPages);
                }
                //因为上面已经反转了，所以左边页面数组中的第二个元素位于下方，对其可以直接使用当前页面的贴图
                leftPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage]);
                leftPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage]);
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage - 1]);
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage]);
            }
            else
            {
                //在书页的最开始的两页，更换材质需要最前面的
                leftPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage - 2]);
                leftPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage - 1]);
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[currentpage - 1]);
                rightPages[1].GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[currentpage]);
            }
        }
        /// <summary>
        /// 将左边的页面全部隐藏
        /// </summary>
        private void CloseLeftPageOfActive()
        {
            foreach (GameObject go in leftPages)
            {
                go.SetActive(false);
            }
        }
        /// <summary>
        /// 显示和隐藏对象
        /// </summary>
        public void ShowHideObject(float time)
        {
            //清空已经显示的物体的集合
            if (bookMap.ContainsKey(currentpage))
            {
                showObject.Clear();
                //判断是否可以进行视野之间的切换
                if (ViewController.Instance.is3DView)
                    ViewController.Instance.Change3DView(time, () =>
                     {
                         for (int i = 0; i < bookMap[currentpage].Count; i++)
                         {
                             Debug.Log("有3D物体显示了 " + bookMap[currentpage ][i]);
                             bookMap[currentpage ][i].SetActive(true);
                             showObject.Add(bookMap[currentpage][i]);
                         }
                     });
            }
            if (spriteObject.Count > 0)
            {
                for (int i = 0; i < spriteObject.Count; i++)
                {
                    spriteObject[i].SetActive(false);
                }
            }
            if (spriteMap.ContainsKey(currentpage + 1))
            {
                spriteMap[currentpage + 1].SetActive(true);
                if (!spriteObject.Contains(spriteMap[currentpage + 1]))
                    spriteObject.Add(spriteMap[currentpage + 1]);
            }
            if (uiBtnSpriteMap.ContainsKey(currentpage + 1))
            {
                uiBtnSprites.Clear();
                foreach (var item in uiBtnSpriteMap[currentpage+1])
                {
                    item.SetActive(true);
                    uiBtnSprites.Add(item);
                }
            }
        }
        /// <summary>
        /// 快速翻页到指定页面
        /// </summary>
        /// <param name="targetPage"></param>
        public void SkipPageToTargetPage(int targetPage)
        {
            //如果输入的页码与当前页码相同，则不作处理
            if (targetPage.CompareTo(currentpage) == 0) return;
            if (targetPage > pagestextures.Length || targetPage < 0) return;
            //如果输入的页码大于当前页码，则向右翻书
            if (targetPage > currentpage)
            {
                currentpage = targetPage;
                FlipPage(1);
                rightPages[0].GetComponentInChildren<PageTurning>().MaterialOfAngleRotate(0, 180, 1f, true);
            }
            else
            {
                currentpage = targetPage;
                FlipPage(-1);
                leftPages[0].GetComponentInChildren<PageTurning>().MaterialOfAngleRotate(1, 180, 1, true);
            }
        }
    }
}
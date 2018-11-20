using DG.Tweening;
using MyCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace PJW.Book
{
    public class BookDummy : MonoSingleton<BookDummy>
    {
        public Dictionary<int, List<GameObject>> bookMap = new Dictionary<int, List<GameObject>>();
        public Dictionary<int, GameObject> spriteMap = new Dictionary<int, GameObject>();
        //书中的所有按钮的精灵
        private Dictionary<int, List<GameObject>> uiBtnSpriteMap = new Dictionary<int, List<GameObject>>();
        //书中的所有视频文件
        private Dictionary<int, List<VideoClip>> bookVideoClip = new Dictionary<int, List<VideoClip>>();
        [HideInInspector]
        //书中所有的音频文件
        public List<AudioClip> bookSoundClip = new List<AudioClip>();
        [HideInInspector]
        public List<GameObject> showObject = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> uiBtnSprites = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> spriteObject = new List<GameObject>();
        //[Tooltip("默认本地文件XML文件")]
        //public string localXMLFile;
        [Tooltip("书中每页的图片内容")]
        public Texture[] pagestextures;
        [Tooltip("书的页面数")]
        public int pagesnumber;
        [Tooltip("当前页码")]
        public int currentpage = 1;
        [Tooltip("书页的预制件")]
        public GameObject PagePrefab;
        [Tooltip("页面的父物体")]
        public Transform PageParent;

        [Tooltip("封面页面")]
        public GameObject firstPage;
        [Tooltip("尾页")]
        public GameObject endPage;
        [Tooltip("2d精灵物体的父物体")]
        public GameObject SpriteParent;
        [HideInInspector]
        public GameObject[] Pages;
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

        public Dictionary<int, GameObject> rightPage = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> leftPage = new Dictionary<int, GameObject>();
        private void Awake()
        {

            GameCore.Instance.BookDummy = this;
        }

        private void Start()
        {
            if (player != null)
            {
                player.Stop();
                player.GetComponent<RectTransform>().DOScale(0, 0);
            }
            //if (GameCore.Instance.GeneratePage != null)
            //{
            //    GameCore.Instance.GeneratePage.player.SetTargetAudioSource(0, GameCore.Instance.SoundManager.source);
            //}
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
                            GameObject go = Resources.Load<GameObject>("Prefabs/Books/" + bookName + "/Object/" + temp);
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
                            GameObject go = Resources.Load<GameObject>("Prefabs/Books/" + bookName + "/Sprites/" + temp);
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
                    AudioClip clip = Resources.Load<AudioClip>("Prefabs/Books/" + bookName + "/Sound/" + item.Attribute("sound").Value);
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

                            VideoClip videoClip = Resources.Load<VideoClip>("Prefabs/Books/" + bookName + "/Video/" + temp);
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
                            GameObject go = Resources.Load<GameObject>("Prefabs/Books/" + bookName + "/UIButtonSprite/" + temp);
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
                Texture t = Resources.Load<Texture>("Prefabs/Books/" + bookName + "/Textures/" + item.Attribute("pageName").Value);
                pagestextures[pagesnumber] = t;
                pagesnumber++;
            }
            #endregion
            
            CanvasController.Instance.ShowAllPage(pagesnumber - 2);
            if (notGenerateSoundManager)
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
                    Texture t = GameCore.Instance.asset.LoadAsset<Texture>(names[i]);
                    pagestextures[pagesnumber] = t;
                    pagesnumber++;
                }
                else if (names[i].Split('.')[1].Equals("mp3"))
                {
                    AudioClip clip = GameCore.Instance.asset.LoadAsset<AudioClip>(names[i]);
                    clip.name = names[i].Split('.')[0];
                    bookSoundClip.Add(clip);
                }
                else if (names[i].Split('.')[1].Equals("mp4"))
                {
                    VideoClip videoClip = GameCore.Instance.asset.LoadAsset<VideoClip>(names[i]);
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
            Pages = new GameObject[pagestextures.Length];
            foreach (var item in firstPage.GetComponentsInChildren<MeshRenderer>())
            {
                if (item.name == "see")
                    item.material.SetTexture("_MainTex", pagestextures[0]);
                if (item.name == "back")
                    item.material.SetTexture("_MainTex", pagestextures[1]);
            }
            foreach (var item in endPage.GetComponentsInChildren<MeshRenderer>())
            {
                if (item.name == "see")
                    item.material.SetTexture("_MainTex", pagestextures[pagesnumber - 2]);
                if (item.name == "back")
                    item.material.SetTexture("_MainTex", pagestextures[pagesnumber - 1]);
            }
            GameObject rightGo;
            //生成左右两边的页面
            for (int i = 2; i < pagestextures.Length / 2 + 2; i++)
            {
                rightGo = Instantiate(PagePrefab, Vector3.zero, Quaternion.identity, PageParent);
                rightGo.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", pagestextures[i - 1]);
                rightGo.GetComponentInChildren<MeshRenderer>().material.SetTexture("_SecTex", pagestextures[i]);
                //rightGo.transform.parent = PageParent;
                rightGo.name = "rightPage" + i;
                rightGo.transform.localPosition = new Vector3(1.351f, 0.04f - 0.01f * i, 0);
                Pages[i - 2] = rightGo;
                
                
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
                            Debug.Log("有3D物体显示了 " + bookMap[currentpage][i]);
                            bookMap[currentpage][i].SetActive(true);
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
                foreach (var item in uiBtnSpriteMap[currentpage + 1])
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

                Pages[currentpage].GetComponentInChildren<PageTurning>().MaterialOfAngleRotate(0, 180, 1f, true);
            }
            else
            {
                currentpage = targetPage;

                Pages[currentpage].GetComponentInChildren<PageTurning>().MaterialOfAngleRotate(1, 180, 1, true);
            }
        }
    }
}
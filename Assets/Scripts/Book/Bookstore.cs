using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class Bookstore : MonoBehaviour
{
    //Books
    public GUISkin skin1;
    public bool BookstoreEnabled = false;
    public bool Localxml = false;
    public bool EnableCurrentPage = true;
    public bool EnableIsNewBook = true;
    public bool EnableBooksSize = false;
    public bool EnableBooksFromXML = false;
    public string[] BookstoreBookNames;
    public string[] BookstoreInfo;
    public string[] BookstoreXML;
    public string[] BookstoreX;
    public string[] BookstoreY;
    public string[] BookstoreIsNew;
    public string[] BookstoreBookApps;
    public GameObject[] BooksGameObjects;
    //Set to XMLBOOKSURL the url of the XML file for books if LocalXml = false to read from the Web. Cross Domain policy needs to be valid. 
    public string XMLBOOKSURL;
    public GameObject rafia;
    public GameObject newbooks;
    public int bookdistance;
    public int rafiadistance;
    public Texture2D lefticon;
    public Texture2D righticon;
    Ray myray = new Ray();
    RaycastHit myhit = new RaycastHit();
    public Texture2D[] covers;
    bool cammove;
    private Camera mainCamera;
    public float speedcammovement;
    public bool enablelights;
    public bool daylightson;
    public bool nightallbooklighton;
    public bool nighteachbooklighton;
    public Light MainLight;
    public Texture downloadicon;
    public bool downloadiconbool;
    void Start()
    {
        mainCamera = Camera.main;
        //加载书是否采用XML进行加载
        if (EnableBooksFromXML)
        {
            StartCoroutine(LoadBooksFromXML());
        }
        else
        {
            StartCoroutine(LoadBooks());
        }
    }
    void LateUpdate()
    {
        if (cammove == true)
        {
            if (mainCamera)
            {
                mainCamera.transform.position = Vector3.MoveTowards(new Vector3(mainCamera.transform.position.x, 2.865031f, -4.295908f), new Vector3(GameObject.Find("" + booknum + "").transform.position.x - 1, 2.865031f, -4.295908f), speedcammovement);

                if (mainCamera.transform.position.x == GameObject.Find("" + booknum + "").transform.position.x)
                {
                    cammove = false;
                }
            }
        }
    }
    void Update()
    {
        //main light settings
        if (daylightson)
        {
            MainLight.intensity = 0.76f;
        }

        if (nightallbooklighton)
        {
            MainLight.intensity = 0.16f;
        }

        if (nighteachbooklighton)
        {
            MainLight.intensity = 0.16f;
        }
        myray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(myray, out myhit, 1000000))
            {

                if (myhit.transform.name == "Box03")
                {

                    int bookNumber = int.Parse(myhit.transform.parent.parent.gameObject.name);

                    PlayerPrefs.SetString("BookName", BookstoreBookNames[bookNumber]);
                    PlayerPrefs.SetString("BookInfo", BookstoreInfo[bookNumber]);

                    if (EnableBooksSize)
                    {
                        PlayerPrefs.SetString("BookX", BookstoreX[bookNumber]);
                        PlayerPrefs.SetString("BookY", BookstoreY[bookNumber]);
                    }
                    if (EnableBooksFromXML)
                    {
                        PlayerPrefs.SetString("BookXML", BookstoreXML[bookNumber]);
                        SceneManager.LoadScene("Book");
                    }
                    else
                    {
                        SceneManager.LoadScene(BookstoreBookApps[bookNumber]);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 利用XML进行书的加载及实例化
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadBooksFromXML()
    {
        downloadiconbool = true;
        if (BookstoreEnabled)
        {
            Array.Clear(BookstoreBookNames, 0, BookstoreBookNames.Length);
            Array.Clear(BookstoreInfo, 0, BookstoreInfo.Length);
            Array.Clear(BookstoreXML, 0, BookstoreXML.Length);

            if (EnableBooksSize)
            {
                Array.Clear(BookstoreX, 0, BookstoreX.Length);
                Array.Clear(BookstoreY, 0, BookstoreY.Length);
                Array.Clear(covers, 0, covers.Length);
            }
        }
        //Code for books
        string URLString4;
        if (BookstoreEnabled)
        {
            if (Localxml)
                URLString4 = "file://" + Application.dataPath + "/Books/XMLContent/books.xml";
            else
                URLString4 = "" + XMLBOOKSURL + "" + "books.xml";
            Debug.Log(URLString4);
            WWW ww4 = new WWW(URLString4);
            yield return ww4;
            string XmlString4 = ww4.text;
            XmlDocument XmlData4 = new XmlDocument();
            XmlData4.LoadXml(XmlString4);
            XmlElement root4 = XmlData4.DocumentElement;

            //resizes array of images to the XML page Nodes
            Array.Resize(ref BookstoreBookNames, root4.ChildNodes.Count);
            Array.Resize(ref BookstoreInfo, root4.ChildNodes.Count);
            Array.Resize(ref BookstoreXML, root4.ChildNodes.Count);
            Array.Resize(ref covers, root4.ChildNodes.Count);
            if (EnableBooksSize)
            {
                Array.Resize(ref BookstoreX, root4.ChildNodes.Count);
                Array.Resize(ref BookstoreY, root4.ChildNodes.Count);

            }
            for (int i = 0; i < root4.ChildNodes.Count / 4; ++i)
            {
                GameObject clone;
                //change the following dimensions if you want to use a different bookcase
                clone = Instantiate(rafia, new Vector3((i + 1) * rafiadistance + 4.559234f, 0, 5f), Quaternion.identity) as GameObject;
            }

            int records4 = 0;
            foreach (XmlNode thisnode in root4.ChildNodes)
            {
                GameObject clone;
                clone = Instantiate(newbooks, new Vector3(records4 * bookdistance, -0.3f, 0.3f), Quaternion.identity) as GameObject;
                clone.name = records4.ToString();

                clone.transform.Find("New Text").GetComponent<TextMesh>().text = thisnode.Attributes["name"].Value;

                WWW w = null;
                if (Localxml)
                {
                    w = new WWW("file://" + Application.dataPath + "/Resources/Files/" + thisnode.Attributes["coverimage"].Value);
                }
                else
                {
                    w = new WWW(thisnode.Attributes["coverimage"].Value);
                }

                yield return w;
                Texture2D tex = new Texture2D(4, 4);
                w.LoadImageIntoTexture(tex);
                covers[records4] = tex;
                clone.transform.Find("book/Box03").GetComponent<Renderer>().material.mainTexture = tex;

                if (EnableBooksSize)
                {
                    clone.transform.Find("book/Box01").transform.localScale = new Vector3(float.Parse(thisnode.Attributes["x"].Value), 1f, float.Parse(thisnode.Attributes["y"].Value));
                    clone.transform.Find("book/Box02").transform.localScale = new Vector3(float.Parse(thisnode.Attributes["x"].Value), 1f, float.Parse(thisnode.Attributes["y"].Value));
                    clone.transform.Find("book/Box03").transform.localScale = new Vector3(float.Parse(thisnode.Attributes["x"].Value), 1f, float.Parse(thisnode.Attributes["y"].Value));
                }

                int currentpage = 0;

                currentpage = PlayerPrefs.GetInt(thisnode.Attributes["name"].Value + "currentpage");

                if (EnableCurrentPage && currentpage > 0)
                {
                    clone.transform.Find("Plane").GetComponent<Renderer>().material.mainTexture = righticon;
                }

                if (EnableIsNewBook && thisnode.Attributes["isnew"].Value == "1")
                {
                    clone.transform.Find("Plane").GetComponent<Renderer>().material.mainTexture = lefticon;
                }

                BookstoreBookNames[records4] = thisnode.Attributes["name"].Value;
                BookstoreInfo[records4] = thisnode.Attributes["info"].Value;
                BookstoreXML[records4] = thisnode.Attributes["xmlfile"].Value;
                if (EnableBooksSize)
                {
                    BookstoreX[records4] = thisnode.Attributes["x"].Value;
                    BookstoreY[records4] = thisnode.Attributes["y"].Value;
                }
                records4 += 1;
            }
            downloadiconbool = false;
        }
    }
    /// <summary>
    /// 将所有的书加载到书架上
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadBooks()
    {
        downloadiconbool = true;
        for (int i = 0; i < BookstoreBookNames.Length % 3; ++i)
        {
            //change the following dimensions if you want to use a different bookcase
            GameObject temp = Instantiate(rafia, new Vector3((i + 1) * rafiadistance + 4.559234f, 0, 5f), Quaternion.identity) as GameObject;
        }

        int records4 = 0;
        GameObject clone;
        for (int i = 0; i < BookstoreBookNames.Length; ++i)
        {
            {
                clone = Instantiate(newbooks, new Vector3(records4 * bookdistance, 0, 0), Quaternion.identity) as GameObject;
                clone.name = records4.ToString();
                clone.transform.Find("New Text").GetComponent<TextMesh>().text = BookstoreBookNames[i];
                clone.transform.Find("book/Box03").GetComponent<Renderer>().material.mainTexture = covers[i];
                if (EnableBooksSize)
                {
                    clone.transform.localScale = new Vector3(float.Parse(BookstoreX[i]), 1f, float.Parse(BookstoreY[i]));
                }

                int currentpage = 0;

                currentpage = PlayerPrefs.GetInt(BookstoreBookNames[i] + "currentpage");

                if (EnableCurrentPage && currentpage > 0)
                {
                    clone.transform.Find("Plane").GetComponent<Renderer>().material.mainTexture = righticon;
                }
                if (EnableIsNewBook && BookstoreIsNew[i] == "1")
                {
                    clone.transform.Find("Plane").GetComponent<Renderer>().material.mainTexture = lefticon;
                }
                records4 += 1;
            }
            downloadiconbool = false;
        }
        yield return null;
    }
    public Texture backiconlightchoose;
    public Texture[] lighttextures;
    int lighttexturesint;
    void OnGUI()
    {

        GUI.skin = skin1;
        if (enablelights)
        {
            if (lighttexturesint == 0)
            {
                daylightson = true;
                nightallbooklighton = false;
                nighteachbooklighton = false;
            }
            else if (lighttexturesint == 1)
            {
                daylightson = false;
                nightallbooklighton = true;
                nighteachbooklighton = false;
            }
            else
            {
                daylightson = false;
                nightallbooklighton = false;
                nighteachbooklighton = true;
            }

        }



        GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 150, 150, 50), "Choose Book");
        //GUI.Label(new Rect(Screen.width/2-55,Screen.height-50,300,50),"Created by Snailrush");
        if (enablelights)
        {
            GUI.DrawTexture(new Rect(10, 0, 30, 40), backiconlightchoose);

            if (GUI.Button(new Rect(10, 5, 30, 30), lighttextures[lighttexturesint], "invisiblebutton"))
            {

                if (lighttexturesint < lighttextures.Length - 1)
                {

                    lighttexturesint++;
                }
                else
                {
                    lighttexturesint = 0;

                }

            }
        }

        if (booknum > 0)
        {
            if (GUI.Button(new Rect(10, Screen.height - 100, 70, 50), "", "backbutton"))
            {
                if (booknum == 0)
                {

                }
                else
                {
                    booknum--;
                    cammove = true;
                }

            }
        }

        if (downloadiconbool)
        {
            GUI.Label(new Rect(Screen.width - 40, 15, 20, 20), downloadicon);
        }

        if (booknum < BookstoreBookNames.Length - 1)
        {
            if (GUI.Button(new Rect(Screen.width - 60, Screen.height - 100, 70, 50), "", "frontbutton"))
            {
                if (booknum == BookstoreBookNames.Length)
                {

                }
                else
                {
                    booknum++;
                    cammove = true;
                }
            }
        }


    }
    public int booknum;
    int currentbook = 0;
}
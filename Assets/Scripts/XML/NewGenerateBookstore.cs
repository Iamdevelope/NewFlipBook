using LitJson;
using PJW.Book;
using PJW.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public class NewGenerateBookstore : MonoBehaviour {
    private Dictionary<string, List<GameObject>> allBooks = new Dictionary<string, List<GameObject>>();
    private List<GameObject> currentShowObject = new List<GameObject>();
    private int index = 0;
    private Ray myray;
    private RaycastHit myhit;
    /// <summary>
    /// 书城中的书的数量
    /// </summary>
    public int bookNum;
    public GameObject bookPrefab;
    public float bookDistance;
    private GameObject temp;
    private Scene2Back scene;

    private void Awake()
    {
        scene = FindObjectOfType<Scene2Back>();
        GameCore.Instance.NewGenerateBookstore = this;
        GameCore.Instance.effectPositionZ = 5;
    }

    private void Update()
    {
        myray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(myray, out myhit, 1000000))
            {

                if (myhit.transform.name == "Box03")
                {
                    GameCore.Instance.PlaySoundBySoundName();
                    string bookName = myhit.transform.parent.parent.gameObject.name;
                    PlayerPrefs.SetString("AssetBundle", bookName + "." + bookName);
                    if (GameCore.Instance.asset != null)
                    {
                        GameCore.Instance.asset.Unload(true);
                        GameCore.Instance.asset = null;
                    }
                    Vector3 temp = myhit.transform.parent.parent.localPosition;
                    temp.x -= 1 + FindObjectOfType<Scene2Back>().pos * 18.24f;
                    temp.y = temp.y + 3;
                    myhit.transform.GetComponentInParent<BookAutoAnimation>().Clicked(Camera.main.transform.position);
                    //FindObjectOfType<CameraMove>().MoveToTarget(temp);
                    //StartCoroutine(ChangeScene());
                }
            }
        }
    }
    /// <summary>
    /// 通过XML文档读取所有书籍信息
    /// </summary>
    /// <param name="bookXML">所有书籍XML文档路径</param>
    /// <param name="name">当前书籍所属类别</param>
    /// <param name="currentClassType">当前书籍所属班级类别</param>
    public void LoadAllBookByXML(string bookXML,string name,string currentClassType)
    {
        if (currentShowObject.Count > 0)
        {
            foreach (var item in currentShowObject)
            {
                item.SetActive(false);
            }
        }
        index = 0;
        //bookNum = 0;
        currentShowObject.Clear();
        XDocument document = XDocument.Load(bookXML);
        XElement root = document.Root;
        XElement ele = root.Element("Books");
        IEnumerable<XElement> bookTypes = root.Elements();
        
        string str = "";
        foreach (var bookType in bookTypes)
        {
            string bt = bookType.Attribute("bookType").Value;
            if (bt.Equals(name))
            {
                str = bt + "_";
                foreach (var classType in bookType.Elements())
                {
                    string ct = classType.Attribute("classType").Value;
                    if (ct.Equals(currentClassType))
                    {
                        str += ct;
                        if (allBooks.ContainsKey(str))
                        {
                            foreach (var item in allBooks[str])
                            {
                                bookNum++;
                                Debug.Log(item.name);
                                item.SetActive(true);
                                currentShowObject.Add(item);
                            }
                            scene.ShowHideButton();
                            return;
                        }
                        GameObject bookParent = new GameObject("AllBooks");
                        allBooks[str] = new List<GameObject>();
                        foreach (var book in classType.Elements())
                        {
                            string bookImagePath = book.Element("bookImagePath").Value;
                            Texture t = Resources.Load<Texture>(bookImagePath);
                            temp = Instantiate(bookPrefab, new Vector3(index * bookDistance + bookPrefab.transform.position.x, bookPrefab.transform.position.y, bookPrefab.transform.position.z), Quaternion.identity);
                            temp.name = book.Attribute("Name").Value;
                            temp.transform.parent = bookParent.transform;
                            temp.transform.Find("book/Box03").GetComponent<Renderer>().material.mainTexture = t;
                            index++;
                            bookNum++;
                            allBooks[str].Add(temp);
                            currentShowObject.Add(temp);
                        }
                    }
                }
            }
        }
        scene.ShowHideButton();
        GameCore.Instance.OpenLoadingPanel(Vector3.zero);
    }

    /// <summary>
    /// 通过JSON文档读取所有书籍信息
    /// </summary>
    /// <param name="bookJSON">所有书籍JSON文档路径</param>
    /// <param name="name">当前书籍所属类别</param>
    /// <param name="currentClassType">当前书籍所属班级类别</param>
    public void LoadAllBookByJSON(string bookJSON, string name, string currentClassType)
    {
        if (currentShowObject.Count > 0)
        {
            foreach (var item in currentShowObject)
            {
                item.SetActive(false);
            }
        }
        index = 0;
        currentShowObject.Clear();

        StreamReader reader = new StreamReader(bookJSON, Encoding.UTF8);
        string arr = File.ReadAllText(bookJSON, Encoding.UTF8);
        JsonReader jsonReader = new JsonReader(arr);
        //JsonData data = JsonMapper.ToObject(jsonReader);
        Books books = JsonMapper.ToObject<Books>(jsonReader);
        foreach (var bookType in books.BookTypes)
        {

            string str = "";
            string bookTypeName = bookType.BookTypeName;
            if (bookTypeName.Equals(name))
            {

                str = bookTypeName + "_";
                foreach (var classType in bookType.ClassTypes)
                {
                    string classTypeName = classType.ClassTypeName;
                    if (classTypeName.Equals(currentClassType))
                    {

                        str += classTypeName;
                        if (allBooks.ContainsKey(str))
                        {
                            foreach (var item in allBooks[str])
                            {
                                bookNum++;
                                Debug.Log(item.name);
                                item.SetActive(true);
                                currentShowObject.Add(item);
                            }
                            scene.ShowHideButton();
                            return;
                        }
                        GameObject bookParent = new GameObject("AllBooks");
                        allBooks[str] = new List<GameObject>();
                        foreach (var book in classType.Book)
                        {
                            Texture texture = Resources.Load<Texture>(book.BookImage);
                            temp = Instantiate(bookPrefab, new Vector3(index * bookDistance + bookPrefab.transform.position.x, bookPrefab.transform.position.y, bookPrefab.transform.position.z), Quaternion.identity);
                            temp.name = book.Name;
                            temp.transform.parent = bookParent.transform;
                            temp.transform.Find("book/Box03").GetComponent<Renderer>().material.mainTexture = texture;
                            index++;
                            bookNum++;
                            allBooks[str].Add(temp);
                            currentShowObject.Add(temp);
                        }
                    }
                }

                scene.ShowHideButton();
                GameCore.Instance.OpenLoadingPanel(Vector3.zero);
            }
        }
    }
}
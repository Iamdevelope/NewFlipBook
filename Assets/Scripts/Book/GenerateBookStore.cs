using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PJW.Book
{
    /// <summary>
    /// 生成书架上的书
    /// </summary>
    public class GenerateBookStore : MonoBehaviour
    {
        public GameObject bookPrefab;
        public float bookDistance;
        private GameObject temp;
        private Ray myray;
        private RaycastHit myhit;
        private int index = 0;
        /// <summary>
        /// 书城中的书的数量
        /// </summary>
        public int bookNum;
        private void Awake()
        {
            GameCore.Instance.GenerateBookStore = this;
            GameCore.Instance.effectPositionZ = 5;
        }
        private void Start()
        {
            //foreach (var item in GameCore.allBooksName)
            //{
            //    if (item.Equals("allbookimage.allbookimage"))
            //        LoadAllBookByAssetBundle(item);
            //}
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
                        FindObjectOfType<CameraMove>().MoveToTarget(temp);
                        //StartCoroutine(ChangeScene());
                    }
                }
            }
        }

        private IEnumerator ChangeScene()
        {

            SceneManager.LoadSceneAsync("Book");
            //PlayerPrefs.SetString("AssetBundle", "dinosaurchangefoam.dinosaurchangefoam");
            yield return StartCoroutine(WaitLoadingNextScene("Book"));
        }
        private IEnumerator WaitLoadingNextScene(string v)
        {
            if (SceneManager.GetActiveScene().name != v)
            {
                GameCore.Instance.OpenLoadingPanel(Vector3.one);
                yield return null;
            }
        }

        public void LoadAllBookByXML(string bookXML)
        {
            XDocument document = XDocument.Load(bookXML);
            XElement root = document.Root;
            XElement ele = root.Element("Book");
            IEnumerable<XElement> xElements = root.Elements();

            GameObject parent = new GameObject("AllBooks");
            foreach (XElement item in xElements)
            {
                Texture t = Resources.Load<Texture>("AllBookImage/" + item.Attribute("Name").Value);
                temp = Instantiate(bookPrefab, new Vector3(index * bookDistance + bookPrefab.transform.position.x, bookPrefab.transform.position.y, bookPrefab.transform.position.z), Quaternion.identity);
                temp.name = item.Attribute("Name").Value;
                temp.transform.parent = parent.transform;
                //temp.transform.Find("New Text").GetComponent<TextMesh>().text = temp.name;
                temp.transform.Find("book/Box03").GetComponent<Renderer>().material.mainTexture = t;
                index++;
                bookNum++;
            }
            GameCore.Instance.OpenLoadingPanel(Vector3.zero);
        }
        public void LoadAllBookByAssetBundle(string assetBundleName)
        {
            GameObject parent = new GameObject("AllBooks");
            GameCore.Instance.asset = AssetBundle.LoadFromFile(assetBundleName);
            string[] allBookNames = GameCore.Instance.asset.GetAllAssetNames();
            for (int i = 0; i < allBookNames.Length; i++)
            {
                allBookNames[i] = allBookNames[i].Split('/')[3];
            }
            for (int i = 0; i < allBookNames.Length; i++)
            {
                if (allBookNames[i].Split('.')[1].Equals("jpg"))
                {
                    temp = Instantiate(bookPrefab, new Vector3(i * bookDistance + bookPrefab.transform.position.x, bookPrefab.transform.position.y, bookPrefab.transform.position.z), Quaternion.identity);
                    temp.name = allBookNames[i].Split('.')[0];
                    //temp.transform.Find("New Text").GetComponent<TextMesh>().text = temp.name;
                    temp.transform.parent = parent.transform;
                    Texture texture = GameCore.Instance.asset.LoadAsset<Texture>(allBookNames[i]);
                    temp.transform.Find("book/Box03").GetComponent<Renderer>().material.mainTexture = texture;
                    bookNum++;
                }
            }
            GameCore.Instance.OpenLoadingPanel(Vector3.zero);
        }
    }
}
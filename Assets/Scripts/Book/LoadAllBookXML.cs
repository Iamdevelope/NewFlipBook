using PJW.Book;
using PJW.Json;
using System.IO;
using UnityEngine;

public class LoadAllBookXML : MonoBehaviour {
    
    private string path;
    [Tooltip("资源是否采用Resources进行加载")]
    public bool loadAssetIsResources;
    [Tooltip("AssetBundle资源名")]
    private string assetBundleFile;
    public bool isXML;
    /// <summary>
    /// 根据书籍类型生成XML
    /// </summary>
    /// <param name="name">书籍所属类型属性</param>
    /// <param name="classType">书籍所属班级类型属性</param>
    public void GenerateAllBook(string name,string classType)
    {
        //在每次进行书本的生成时，将书本数量清空
        GameCore.Instance.NewGenerateBookstore.bookNum = 0;
        if (loadAssetIsResources)
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (isXML)
                {
                    path = GameCore.Instance.BookOfConfig + "allBooks.xml";
                    if (!File.Exists(path))
                        NewGenerateAllbookXMLFile.GetBookContentByFile(path, () => StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByXML(path, name, classType)));
                    else
                        StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByXML(path, name, classType));
                }
                else
                {
                    path = GameCore.Instance.BookOfConfig + "books.json";
                    if (!File.Exists(path))
                        GenerateAllBookJSONFile.GetBookContentByFile(path, () => StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByJSON(path, name, classType)));
                    else
                        StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByJSON(path, name, classType));
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (isXML)
                {
                    path = GameCore.Instance.BookOfConfig + "allBooks.xml";
                    if (!File.Exists(path))
                        NewGenerateAllbookXMLFile.GetBookContentByFile(path, () => StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByXML(path, name, classType)));
                    else
                        StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByXML(path, name, classType));
                }
                else
                {
                    path = GameCore.Instance.BookOfConfig + "books.json";
                    if (!File.Exists(path))
                        GenerateAllBookJSONFile.GetBookContentByFile(path, () => StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByJSON(path, name, classType)));
                    else
                        StartCoroutine(GameCore.Instance.NewGenerateBookstore.LoadAllBookByJSON(path, name, classType));
                }
            }
        }
        //否则采用AssetBundle进行资源加载
        else
        {
            path = GameCore.Instance.SavePath;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var item in GameCore.allBooksName)
            {
                if (item.Equals("allbookimage.allbookimage"))
                {
                    assetBundleFile = item;
                    DownLoadMainfest(path,name,classType);
                }
            }
        }
    }

    private void DownLoadMainfest( string savePath,string bookType,string classType)
    {
        string path = savePath + assetBundleFile;
        if (File.Exists(path))
        {
            //在进行资源加载之前将所有AssetBundle资源卸载，以防止资源重复加载出错
            //AssetBundle.UnloadAllAssetBundles(true);
            //GameCore.Instance.GenerateBookStore.LoadAllBookByAssetBundle(path);
            GameCore.Instance.NewGenerateBookstore.LoadAllBookByAssetBundle(path,bookType,classType);
        }
    }

    public void Reset()
    {
        File.Delete(path);
    }
}
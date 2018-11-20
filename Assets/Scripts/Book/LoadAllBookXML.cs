using PJW.Book;
using PJW.Common;
using PJW.HotUpdate;
using PJW.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAllBookXML : MonoBehaviour {
    
    private string path;
    [Tooltip("资源是否采用Resources进行加载")]
    public bool loadAssetIsResources;
    [Tooltip("AssetBundle资源名")]
    public string assetBundleFile;
    /// <summary>
    /// 根据书籍类型生成XML
    /// </summary>
    /// <param name="name">书籍所属类型属性</param>
    /// <param name="classType">书籍所属班级类型属性</param>
    public void GenerateAllBook(string name,string classType)
    {
        GameCore.Instance.NewGenerateBookstore.bookNum = 0;
        if (loadAssetIsResources)
        {
            //path = GameCore.Instance.LocalXMLPath + name + "/" + classType + "/" + name + classType + ".xml";
            path = GameCore.Instance.LocalXMLPath + "allBooks.xml";

            //path = GameCore.Instance.LocalXMLPath + "books.json";
            //if (!File.Exists(GameCore.Instance.LocalXMLPath + "books.json"))
            //    GenerateAllBookJSONFile.GetBookContentByFile(GameCore.Instance.LocalXMLPath + "books.json", () => GameCore.Instance.NewGenerateBookstore.LoadAllBookByJSON(path, name, classType));
            //else
            //    GameCore.Instance.NewGenerateBookstore.LoadAllBookByJSON(path, name, classType);
            ////如果需要创建的书本的xml文件不存在，则对其进行创建
            //if (!File.Exists(path))
            //    GenerateAllBookXML.GetBookContentByFile(path, name, classType, () => GameCore.Instance.GenerateBookStore.LoadAllBookByXML(path));
            //else
            //    GameCore.Instance.GenerateBookStore.LoadAllBookByXML(path);
            //如果需要创建的书本的xml文件不存在，则对其进行创建
            if (!File.Exists(path))
                NewGenerateAllbookXMLFile.GetBookContentByFile(path, () => GameCore.Instance.NewGenerateBookstore.LoadAllBookByXML(path, name, classType));
            else
                GameCore.Instance.NewGenerateBookstore.LoadAllBookByXML(path, name, classType);
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
                    DownLoadMainfest(path);
                }
            }
        }
    }

    private void DownLoadMainfest( string savePath)
    {
        string path = savePath + assetBundleFile;
        if (File.Exists(path))
        {
            //在进行资源加载之前将所有AssetBundle资源卸载，以防止资源重复加载出错
            AssetBundle.UnloadAllAssetBundles(true);
            GameCore.Instance.GenerateBookStore.LoadAllBookByAssetBundle(path);
        }
    }

    public void Reset()
    {
        File.Delete(path);
    }
}
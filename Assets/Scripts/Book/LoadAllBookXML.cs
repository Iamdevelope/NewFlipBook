using PJW.Book;
using PJW.Common;
using PJW.HotUpdate;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAllBookXML : MonoBehaviour {

    [Tooltip("默认本地文件XML文件")]
    public string bookName;
    private string path;
    [Tooltip("资源是否采用Resources进行加载")]
    public bool loadAssetIsResources;
    [Tooltip("AssetBundle资源名")]
    public string assetBundleFile;
    private void Start()
    {
        //如果是采用Resources进行资源加载
        if (loadAssetIsResources)
        {
            path = GameCore.Instance.LocalXMLPath + bookName;
            //如果需要创建的书本的xml文件不存在，则对其进行创建
            if (!File.Exists(path))
                GenerateAllBookXML.GetBookContentByFile(bookName, () => GameCore.Instance.GenerateBookStore.LoadAllBookByXML(path));
            else
                GameCore.Instance.GenerateBookStore.LoadAllBookByXML(path);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using PJW.Common;
using PJW.Book;
using PJW.HotUpdate;

public class StartLoadBookXML : MonoBehaviour {
    [Tooltip("默认本地文件XML文件")]
    public string bookName;
    private string path;
    [Tooltip("资源是否采用Resources进行加载")]
    public bool loadAssetIsResources;
    [HideInInspector]
    public string assetBundleFile;
    private void Start()
    {
        if (PlayerPrefs.HasKey("AssetBundle"))
        {
            assetBundleFile = PlayerPrefs.GetString("AssetBundle");
            bookName = assetBundleFile.Split('.')[0] + ".xml";
        }
        //如果是采用Resources进行资源加载
        if (loadAssetIsResources)
        {
            path = GameCore.Instance.LocalXMLPath + bookName;
            ////如果需要创建的书本的xml文件不存在，则对其进行创建
            //if (!File.Exists(path))
            //    GenerateXML.GetBookContentByFile(bookName, () => GameCore.Instance.GeneratePage.LoadBook(path));
            //else
            //    GameCore.Instance.GeneratePage.LoadBook(path);
            //如果需要创建的书本的xml文件不存在，则对其进行创建
            if (!File.Exists(path))
                GenerateXML.GetBookContentByFile(bookName, () => GameCore.Instance.GeneratePage.LoadBook(path));
            else
                GameCore.Instance.GeneratePage.LoadBook(path);
        }
        //否则采用AssetBundle进行资源加载
        else
        {
            path = GameCore.Instance.SavePath;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DownLoadMainfest(GameCore.Instance.URL,path);
        }
    }

    private void DownLoadMainfest(string url,string savePath)
    {
        string path = savePath + assetBundleFile;
        if (File.Exists(path))
        {
            GameCore.Instance.GeneratePage.LoadBookByAssetBundle(path);
        }
        //如果当前需要加载的资源不存在，则需要进行下载
        else
           StartCoroutine(GenerateAssetBundlePath.GenerateAssetBundle(url, savePath, assetBundleFile, () => GameCore.Instance.GeneratePage.LoadBookByAssetBundle(path)));
    }

    public void Reset()
    {
        File.Delete(path);
    }
}
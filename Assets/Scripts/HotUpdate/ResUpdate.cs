using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO;
using System.Net;
using System.Xml;
using System.IO.Compression;

public class ResUpdate
{
    public static ResUpdate Instance;
    public static string MAIN_VERSION_FILE = "VersionMd5.xml";                                  //版本文件  
    static string SERVER_RES_URL;                                                               //服务器资源地址  
    static string LOCAL_RES_OUT_PATH = "";                                                      //本地资源地址  
    public static string LOCAL_DECOMPRESS_RES = "";                                             //本地解压资源  
    public static Dictionary<string, ResReference> LocalResOutVersion = new Dictionary<string, ResReference>();   //本地包外资源版本  
    static Dictionary<string, int> ServerResVersion = new Dictionary<string, int>();            //服务器资源版本  
    static List<string> NeedDownFiles = new List<string>();                                     //需要下载的文件  

    //资源引用  
    public struct ResReference
    {
        public bool isFinish;                  //是否存在包外  
        public bool isUnZip;                   //是否解压DownFile  
        public int version;                    //版本号  
        public ResReference(bool isFinish, int version, bool isUnZip)
        {
            this.isFinish = isFinish;
            this.version = version;
            this.isUnZip = isUnZip;
        }
    }

    public int updatedNum = 0;                             //更新数量  
    public int totalNeedUpdateNum = 0;                     //需要更新总数量  
    public long curFileTotalNum = 0;                       //当前文件总数量  
    public long curFileNum = 0;                            //当前文件数量  
    public Thread downloadThread;                          //下载线程  

    static ResUpdate()
    {
        Instance = new ResUpdate();
#if UNITY_ANDROID
        SERVER_RES_URL = "http://192.168.0.106:80/AssetBundle/Android/";
#elif UNITY_IPHONE  
          SERVER_RES_URL = "http://127.0.0.1:88/AssetBundle/IOS/";  
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN  
        SERVER_RES_URL = "http://127.0.0.1:88/AssetBundle/Windows32/";  
#endif
        System.Net.ServicePointManager.DefaultConnectionLimit = 512;//  
        LOCAL_RES_OUT_PATH = Application.persistentDataPath + "/AssetBundle/";
        LOCAL_DECOMPRESS_RES = LOCAL_RES_OUT_PATH + "DeCompress/";
        Debug.Log("@@ LOCAL_RES_OUT_PATH: " + LOCAL_RES_OUT_PATH);
        //SERVER_RES_URL += SGConstant.mStr_CURVERSION + "/";  
    }
    private static bool isDownload = false;

    public static void StartDownLoad(bool isAuto = true)
    {
        if (isAuto)
        {
            //if (AndroidSDKTool.GetNetWorkType() == NetWorkType.WIFI)
            //{
            //    Debug.Log("start download resource");
            //    if (!isDownload)
            //    {
            //        //CoroutineManager.DoCoroutine(Instance.LoadVersion());  
            //    }
            //}
            //else
            //{
            //    Debug.Log("no wifi");
            //}
        }
        else
        {
            if (!isDownload)
            {
                //CoroutineManager.DoCoroutine(Instance.LoadVersion());
            }
        }
        //NewBehaviourScript.Instance.StartCoroutine(Instance.LoadVersion());
    }

    //加载版本  
    public IEnumerator LoadVersion()
    {
        Debug.Log("@@ LoadVersion");
        isDownload = true;
        //清空变量  
        //  
        ServerResVersion.Clear();
        NeedDownFiles.Clear();
        string serverMainVersion = "";
        //读取本地配置文件  
        string localVersion = System.IO.Path.Combine(LOCAL_RES_OUT_PATH, MAIN_VERSION_FILE);
        if (File.Exists(localVersion))
            Instance.ParseLocalVersionFile(localVersion, LocalResOutVersion);
        //  
        //取得服务器版本  
        string versionUrl = SERVER_RES_URL + "VersionMd5_1.0/" + MAIN_VERSION_FILE;
        Debug.Log("@@ versionUrl: " + versionUrl);
        WWW sw = new WWW(versionUrl);
        yield return sw;
        if (!string.IsNullOrEmpty(sw.error))
        {
            Debug.LogError("Server Version ..." + sw.error);
        }
        else
        {
            serverMainVersion = sw.text;
            Debug.Log(serverMainVersion);
            ParseVersionFile(serverMainVersion, ServerResVersion); //检查包外资源是否比包内资源新，添加字典  
        }
        if (string.IsNullOrEmpty(sw.text))
        {
            Debug.LogError("无法连接服务器");
            //加载一个场景  
        }
        else
        {//可以连接服务器  
            //对比本地和服务器的配置  
            CompareServerVersion();
            //开启下载  
            DownLoadResByThread();
        }
    }

    public delegate void HandleFinishDownload(WWW www);

    //在多线程环境中只要我们用下面的方式实例化 HashTable 就可以了  
    Hashtable ht = Hashtable.Synchronized(new Hashtable());
    public void DownLoadResByThread()//新建线程下载
    {
        downloadThread = new Thread(new ThreadStart(DownFile));
        downloadThread.Start();
    }

    //更新本地的version配置  
    private void UpdateLocalVersionFile()
    {
        XmlDocument xmldoc = new XmlDocument();
        XmlElement xmlelem;
        //加入一个根元素  
        xmlelem = xmldoc.CreateElement("", "VersionNum", "");
        xmldoc.AppendChild(xmlelem);
        foreach (KeyValuePair<string, ResReference> kvp in LocalResOutVersion)
        {
            XmlElement xel = xmldoc.CreateElement("File");                 //创建一个<Node>节点  
            xel.SetAttribute("FileName", kvp.Key);                         //设置该节点 FileName 属性  
            xel.SetAttribute("Num", kvp.Value.version.ToString());         //设置该节点Num属性  
            xel.SetAttribute("isFinish", kvp.Value.isFinish.ToString());   //设置该节点isFinish属性  
            xel.SetAttribute("isUnZip", kvp.Value.isUnZip.ToString());     //设置该节点isUnZip属性  
            xmlelem.AppendChild(xel);                                      //添加到<Empoloyees>节点中  
        }
        xmldoc.Save(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE);               //XML保存地址
    }

    //与服务器版本比较  
    private void CompareServerVersion()
    {
        foreach (var version in ServerResVersion)//遍历服务器资源版本  
        {
            string fileName = version.Key;//文件名  
            int serverVersion = version.Value;//文件大小  
            //如果本地配置表中无资源、版本号不匹配，或者未下载完成，就下载  
            if ((LocalResOutVersion.ContainsKey(fileName) && LocalResOutVersion[fileName].version != ServerResVersion[fileName])//无资源  
                || !LocalResOutVersion.ContainsKey(fileName)//不匹配  
                || (LocalResOutVersion.ContainsKey(fileName))//
                && LocalResOutVersion[fileName].version == ServerResVersion[fileName]//版本
                && !LocalResOutVersion[fileName].isFinish)//未完成  
            {
                NeedDownFiles.Add(fileName);//需下载的文件名  
                Debug.Log("@@ 需下载资源文件名： " + fileName);
            }
        }
        totalNeedUpdateNum = NeedDownFiles.Count;//需要下载的文件数量  
        updatedNum = 0;
        Debug.Log(string.Format("需下载资源数量：{0}", totalNeedUpdateNum));
        //删除旧资源  
        foreach (var fileName in NeedDownFiles)
        {
            string localPath = LOCAL_RES_OUT_PATH + fileName;//本地下载文件   
            string localUnZipPath = LOCAL_DECOMPRESS_RES;//本地解压文件夹

            if (File.Exists(localPath))
            {
                File.Delete(localPath);//删除文件
            }
            if (Directory.Exists(localUnZipPath + fileName.Substring(0, fileName.IndexOf("."))))
            {
                Directory.Delete(localUnZipPath + fileName.Substring(0, fileName.IndexOf(".")), true);//删除文件夹
                Debug.Log("@@ 删除文件夹" + localUnZipPath + fileName.Substring(0, fileName.IndexOf(".")));
            }
        }
    }

    /// <summary>  
    /// 检查包外资源是否比包内资源新  
    /// </summary>  
    /// <param name="content"></param>  
    /// <param name="dict"></param>  
    private void ParseVersionFile(string content, Dictionary<string, int> dict)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(content);
        XmlNodeList nodes = doc.GetElementsByTagName("File");//找出Xml中节点未File的合集  

        for (int i = 0; i < nodes.Count; i++)
        {
            XmlAttribute att = nodes[i].Attributes["FileName"];//找出属性为FileName  
            if (!dict.ContainsKey(att.Value))//如果字典不存在该属性  
            {
                dict.Add(att.Value, int.Parse(nodes[i].Attributes["Num"].Value));//找出属性为Num -- 加入字典  
            }
            else
            {
                Debug.Log("Dict has same key ----->" + att.Value);
            }
        }
    }

    /// <summary>  
    /// 将xml版本号转换为字典数据  
    /// </summary>  
    /// <param name="filename"></param>  
    /// <param name="dict"></param>  
    private void ParseLocalVersionFile(string filename, Dictionary<string, ResReference> dict)
    {
        if (filename == null || filename.Length == 0)
        {
            return;
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);
        XmlNodeList nodes = doc.GetElementsByTagName("File");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlAttribute att = nodes[i].Attributes["FileName"];
            if (!dict.ContainsKey(att.Value))
            {
                dict.Add(att.Value, new ResReference(bool.Parse(nodes[i].Attributes["isFinish"].Value),
                    int.Parse(nodes[i].Attributes["Num"].Value),
                    bool.Parse(nodes[i].Attributes["isUnZip"].Value)
                    ));
            }
            else
            {
                Debug.Log("Dict has same key ----->" + att.Value);
            }
        }
    }

    //支持断点续传  
    private void DownFile()
    {
        if (updatedNum >= NeedDownFiles.Count)
        {
            UpdateLocalVersionFile();
            return;
        }
        string fileName = NeedDownFiles[updatedNum];       //需要更新文件  
        string serverPath = SERVER_RES_URL + fileName;     //服务器资源路径  
        //判断目录  
        string[] fileNameArr = fileName.Split('/');
        string filePath = "";
        for (int i = 0; i < fileNameArr.Length - 1; i++)
        {
            filePath += fileNameArr[i] + "/";
        }
        filePath = LOCAL_RES_OUT_PATH + filePath;          //下载文件目录  
        string localPath = LOCAL_RES_OUT_PATH + fileName;  //下载文件  
        string localUnZipPath = LOCAL_DECOMPRESS_RES;      //本地解压文件夹
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        if (!Directory.Exists(LOCAL_DECOMPRESS_RES))
        {
            Directory.CreateDirectory(LOCAL_DECOMPRESS_RES);
        }
        bool isRight = false;                              //是否下载好  
        //上个版本先删除（并删除解压好的文件）  
        if (LocalResOutVersion.ContainsKey(fileName)
            && (LocalResOutVersion[fileName].version < ServerResVersion[fileName]))
        {
            File.Delete(localPath);                        //删除文件
            if (Directory.Exists(localUnZipPath + fileName.Substring(0, fileName.IndexOf("."))))
            {
                Directory.Delete(localUnZipPath + fileName.Substring(0, fileName.IndexOf(".")), true);//删除文件夹
                Debug.Log("@@ 删除文件夹" + fileName.Substring(0, fileName.IndexOf(".")));
            }
        }
        FileStream fs = null;
        HttpWebRequest requestGetCount = null;
        HttpWebResponse responseGetCount = null;
        try
        {
            requestGetCount = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(serverPath);
            responseGetCount = (HttpWebResponse)requestGetCount.GetResponse();
            curFileTotalNum = responseGetCount.ContentLength;
            if (File.Exists(localPath))
            {
                fs = File.OpenWrite(localPath);          //打开流  
                curFileNum = fs.Length;                  //通过字节流的长度确定当前的下载位置  
                if (curFileTotalNum - curFileNum <= 0)
                {
                    isRight = true;
                }
                fs.Seek(curFileNum, SeekOrigin.Current); //移动文件流中的当前指针  
            }
            else
            {
                fs = new FileStream(localPath, FileMode.CreateNew);
                curFileNum = 0;
            }
        }
        catch (Exception ex)
        {
            if (fs != null)
            {
                fs.Close();
            }
            UpdateLocalVersionTemp(fileName, false, false);
            UpdateLocalVersionFile();
            isRight = false;
            Debug.Log(ex.ToString());
        }
        finally
        {
            if (responseGetCount != null)
            {
                responseGetCount.Close();
                responseGetCount = null;
            }
            if (requestGetCount != null)
            {
                requestGetCount.Abort();
                requestGetCount = null;
            }
        }
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        Stream ns = null;
        try
        {
            //本地未下载完成  
            if (!isRight)
            {
                request = (HttpWebRequest)HttpWebRequest.Create(serverPath);
                if (curFileNum > 0)
                {
                    request.AddRange((int)curFileNum);             //设置Range值  
                }
                response = (HttpWebResponse)request.GetResponse();
                //向服务器请求，获得服务器回应数据流  
                ns = response.GetResponseStream();
                byte[] nbytes = new byte[1024];
                int nReadSize = 0;
                nReadSize = ns.Read(nbytes, 0, 1024);
                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    nReadSize = ns.Read(nbytes, 0, 1024);
                    curFileNum += nReadSize;
                }
                isRight = true;
                fs.Flush();
                fs.Close();
                ns.Close();
                request.Abort();
            }
            UpdateLocalVersionTemp(fileName, true, false);
            //解压（防止更新下载不全，解压报错的文件占坑）  
            if (Directory.Exists(localUnZipPath + fileName.Substring(0, fileName.IndexOf("."))))
            {
                Directory.Delete(localUnZipPath + fileName.Substring(0, fileName.IndexOf(".")), true);//删除文件夹
                Debug.Log("@@ 删除文件夹" + fileName.Substring(0, fileName.IndexOf(".")));
            }
            //解压工具  
            var err = "";
            //CompressUtil.UnZipFile(localPath, localUnZipPath, out err);
            
            UpdateLocalVersionTemp(fileName, true, true);
            updatedNum++;
            Debug.Log("down" + updatedNum + "/" + totalNeedUpdateNum + "," + fileName + "Loading complete");
        }
        catch (Exception ex)
        {
            if (fs != null)
            {
                fs.Close();
            }
            UpdateLocalVersionTemp(fileName, false, false);
            isRight = false;
            Debug.Log(ex.ToString());
            //解压出错，删除下载文件  
            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
        }
        finally
        {
            if (ns != null)
            {
                ns.Close();
                ns = null;
            }
            if (response != null)
            {
                response.Close();
                response = null;
            }
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            //下载下一个
            DownFile();
        }
        //下载结束  
        isDownload = false;
    }

    private string UrlNoCache(string url)
    {
        return url + "?data=" + DateTime.Now.Ticks;
    }

    private void UpdateLocalVersionTemp(string fileName, bool isFinish, bool isUnZip)
    {
        //下载更新版本号  
        if (LocalResOutVersion.ContainsKey(fileName))
        {
            LocalResOutVersion[fileName] = new ResReference(isFinish, ServerResVersion[fileName], isUnZip);
        }
        else
        {
            LocalResOutVersion.Add(fileName, new ResReference(isFinish, ServerResVersion[fileName], isUnZip));
        }
        UpdateLocalVersionFile();
    }
}
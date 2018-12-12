using PJW.Book;
using PJW.Book.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace PJW.HotUpdate
{
    /// <summary>
    /// 利用AssetBundle加载资源
    /// </summary>
    public class LoadAssetByAssetBundle : MonoBehaviour
    {
        private List<string> updateAssetName = new List<string>();
        private WWW progressWWW;
        private int index = 0;
        [HideInInspector]
        public Text progressText;
        [HideInInspector]
        public Slider slider;
        private WWW www;
        public string assetBundleName;
        public bool isLoadAssetByAsset;
        private DownLoadPanel loadPanel;

        private HttpWebRequest request;
        private HttpWebResponse response;
        private bool isDownload;
        private long curFileNum;
        private int updatedNum;
        private string totalNeedUpdateNum;
        private Dictionary<string, int> LocalResOutVersion = new Dictionary<string, int>();
        private Dictionary<string, int> ServerResVersion = new Dictionary<string, int>();
        private List<string> NeedDownFiles = new List<string>();
        //版本文件
        public static string MAIN_VERSION_FILE = "VersionMd5.xml";
        //服务器资源地址
        static string SERVER_RES_URL;
        //本地资源地址  
        static string LOCAL_RES_OUT_PATH = "";
        //本地解压资源  
        public static string LOCAL_DECOMPRESS_RES = "";
        //当前文件总数量  
        public long curFileTotalNum = 0;
        private Thread thread;

        private void Start()
        {
            Debug.Log(Application.temporaryCachePath+ " is temporaryCachePath ");
            loadPanel = FindObjectOfType<DownLoadPanel>();
            if (!Directory.Exists(GameCore.Instance.SavePath))
                Directory.CreateDirectory(GameCore.Instance.SavePath);
            if (isLoadAssetByAsset)
                OnClickUpdate();
            SERVER_RES_URL = @"ftp://192.168.1.110:66/ABFiles/";
            LOCAL_RES_OUT_PATH = Application.persistentDataPath + "/AssetBundles";
            LOCAL_DECOMPRESS_RES = LOCAL_RES_OUT_PATH + "/DeCompress";

            //StartCoroutine(WWWLoad.DownFile(SERVER_RES_URL + "/README.txt", GameCore.Instance.SavePath+"/README.txt", null, () => { Debug.Log("success download readme.txt"); }));
            //HTTPLoad.DownLoad(SERVER_RES_URL + "/README.txt", GameCore.Instance.SavePath, "README.txt", () => { Debug.Log("success download"); });
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void OnClickUpdate()
        {
            StartCoroutine(DownLoadMainfest(GameCore.Instance.URL, GameCore.Instance.SavePath));
        }
        /// <summary>
        /// 将保存所有Asset的Manifest下载，然后进行一一比对，判断是否有需要下载的资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">资源保存的路径</param>
        /// <returns></returns>
        private IEnumerator DownLoadMainfest(string url, string savePath)
        {

            string[] urlAllAssetNames = null;
            www = new WWW(url + "/" + assetBundleName);
            yield return www;
            if (!GameCore.Instance.asset)
                GameCore.Instance.asset = www.assetBundle;
            AssetBundleManifest manifest = GameCore.Instance.asset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            urlAllAssetNames = manifest.GetAllAssetBundles();
            if (Directory.Exists(savePath))
            {
                DirectoryInfo directory = new DirectoryInfo(savePath);
                FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);

                foreach (var item in urlAllAssetNames)
                {
                    GameCore.allBooksName.Add(item);
                    updateAssetName.Add(item);
                }
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (GameCore.allBooksName.Contains(file.Name) && !file.Name.EndsWith(".meta"))
                            updateAssetName.Remove(file.Name);
                    }
                }
            }
            if (updateAssetName.Count > 0)
            {
                Debug.Log("有资源需要更新");
                //slider.gameObject.GetComponentInParent<DownLoadPanel>().Reset(Vector3.one, 0.3f);
                loadPanel.Reset(Vector3.one, 0.3f);
                //DownLoad(savePath);
                thread = new Thread(new ThreadStart(DownLoadMainfestByFTP));
                thread.Start();
                //DownLoadMainfestByHttp();
            }
            else
            {
                Debug.Log("没有资源需要更新");
                //没有资源需要更新
                www.Dispose();
                www = null;
            }
            
        }

        /// <summary>
        /// 资源下载，支持断点续传
        /// </summary>
        private void DownLoadMainfestByFTP()
        {
            Debug.Log(" curret update the asset of num is " + updatedNum + " the updateAssetName of count is " + updateAssetName.Count);
            if (updatedNum >= updateAssetName.Count)
            {
                UpdateLocalVersionFile();
                return;
            }
            string fileName = updateAssetName[updatedNum];       //需要更新文件  
            string serverPath = SERVER_RES_URL + fileName;     //服务器资源路径  
                                                               //判断目录  
            string[] fileNameArr = fileName.Split('/');
            string filePath = "";
            for (int i = 0; i < fileNameArr.Length - 1; i++)
            {
                filePath += fileNameArr[i] + "/";
            }
            filePath = LOCAL_RES_OUT_PATH + filePath;          //下载文件目录  
            string localPath = LOCAL_RES_OUT_PATH + "/" + fileName;  //下载文件  
            string localUnZipPath = LOCAL_DECOMPRESS_RES;      //本地解压文件夹
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!Directory.Exists(LOCAL_DECOMPRESS_RES))
            {
                Directory.CreateDirectory(LOCAL_DECOMPRESS_RES);
            }
            //是否下载好  
            bool isRight = false; 
            //上个版本先删除（并删除解压好的文件）  
            if (LocalResOutVersion.ContainsKey(fileName)
                && (LocalResOutVersion[fileName] < ServerResVersion[fileName]))
            {
                File.Delete(localPath);                        //删除文件
                if (Directory.Exists(localUnZipPath + fileName.Substring(0, fileName.IndexOf("."))))
                {
                    Directory.Delete(localUnZipPath + fileName.Substring(0, fileName.IndexOf(".")), true);//删除文件夹
                    Debug.Log("@@ 删除文件夹" + fileName.Substring(0, fileName.IndexOf(".")));
                }
            }
            FileStream fs = null;
            FtpWebRequest requestGetCount = null;

            FtpWebResponse responseGetCount = null;
            try
            {
                requestGetCount = (FtpWebRequest)WebRequest.Create(serverPath);
                responseGetCount = (FtpWebResponse)requestGetCount.GetResponse();
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
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            Stream ns = null;
            try
            {
                //本地未下载完成  
                if (!isRight)
                {
                    request = (FtpWebRequest)HttpWebRequest.Create(serverPath);

                    //if (curFileNum > 0)
                    //{
                    //    request.
                    //    request.AddRange((int)curFileNum);             //设置Range值  
                    //}
                    response = (FtpWebResponse)request.GetResponse();

                    long l = response.ContentLength;
                    Debug.Log(" the content of length is " + l);
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
                DownLoadMainfestByFTP();
            }
            //下载结束  
            isDownload = false;
        }

        /// <summary>
        /// 资源下载，支持断点续传
        /// </summary>
        private void DownLoadMainfestByHttp()
        {
            if (updatedNum >= updateAssetName.Count)
            {
                UpdateLocalVersionFile();
                return;
            }
            string fileName = updateAssetName[updatedNum];       //需要更新文件  
            string serverPath = SERVER_RES_URL + fileName;     //服务器资源路径  
                                                               //判断目录  
            string[] fileNameArr = fileName.Split('/');
            string filePath = "";
            for (int i = 0; i < fileNameArr.Length - 1; i++)
            {
                filePath += fileNameArr[i] + "/";
            }
            filePath = LOCAL_RES_OUT_PATH + filePath;          //下载文件目录  
            string localPath = LOCAL_RES_OUT_PATH +"/"+ fileName;  //下载文件  
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
                && (LocalResOutVersion[fileName] < ServerResVersion[fileName]))
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
                return;
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
                DownLoadMainfestByHttp();
            }
            //下载结束  
            isDownload = false;
        }

        private void UpdateLocalVersionFile()
        {
            Debug.Log("all res haved update ");
            loadPanel.DownOver();
            Debug.Log("UpdateLocalVersionFile");
        }

        private void UpdateLocalVersionTemp(string fileName, bool isFinish, bool unZip)
        {
            Debug.Log(fileName + " ---- " + isFinish + " ******* " + unZip);
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="savePath"></param>
        private void DownLoad(string savePath)
        {
            if (index >= updateAssetName.Count)
            {
                www.Dispose();
                progressWWW.Dispose();
                www = null;
                progressWWW = null;
                //progressText.text = string.Format("下载进度:{0:F}% \n当前正在下载第{1}个资源，共{2}个资源。", 100, updateAssetName.Count, updateAssetName.Count);
                //slider.value = 1;
                //StartCoroutine(loadPanel.DownOver());
                loadPanel.DownOver();
                //需要更新的资源下载完毕,将需要更新的资源列表清空
                updateAssetName.Clear();
                Debug.Log("所有资源更新完毕");
                return;
            }
            StartCoroutine(GenerateAssetBundle(GameCore.Instance.URL, savePath, updateAssetName[index], () => {
                DownLoad(savePath);
                index++;
                }));
        }

        /// <summary>
        /// 创建AssetBundle资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">下载的资源保存路径</param>
        /// <param name="assetBundleName">资源名字</param>
        /// <param name="callBack">回调</param>
        public IEnumerator GenerateAssetBundle(string url, string savePath, string assetBundleName, Action callBack)
        {
            progressWWW = new WWW(url + "/" + assetBundleName);
            loadPanel.StartDownLoad(progressWWW, index, updateAssetName.Count);
            while (!progressWWW.isDone)
            {
                yield return null;
                Debug.Log("正在下载资源...");
            }
            yield return progressWWW;
            if (progressWWW.isDone)
            {
                Debug.Log("资源下载完毕...");
                CreateFile(savePath + assetBundleName, progressWWW.bytes, callBack);
            }
        }

        /// <summary>
        /// 将下载的资源保存到指定路劲
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="bytes"></param>
        private void CreateFile(string savePath, byte[] bytes, Action callBack)
        {
            FileStream fs = new FileStream(savePath, FileMode.Append);
            fs.Write(bytes, 0, bytes.Length);
            //利用文件流进行写数据时，会进行缓存，Flush就是不让它缓存，直接写到文件
            fs.Flush();
            //关闭流，并将所有资源释放
            fs.Close();
            //释放流
            fs.Dispose();
            progressWWW.Dispose();
            //释放www
            if (callBack != null)
                callBack();
        }

        public void Reset()
        {
            File.Delete(GameCore.Instance.SavePath);
        }
    }
}
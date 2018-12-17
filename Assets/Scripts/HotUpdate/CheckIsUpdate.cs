
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using PJW.Book.UI;
using System.Security.Cryptography;
using PJW.Book;
using System.Threading;
using System.Net;
using LitJson;
using System.IO.Compression;

namespace PJW.HotUpdate
{
    [SerializeField]
    public class MyFile
    {
        public string FilePath { get; set; }
        public List<FileName> FileNames { get; set; }
    }
    [SerializeField]
    public class FileName
    {
        public string Name { get; set; }
    }
    [SerializeField]
    public class FilePath
    {
        public List<MyFile> myFiles { get; set; }
    }
    /// <summary>
    /// 检查是否需要更新
    /// </summary>
    public class CheckIsUpdate : MonoBehaviour
    {
        /// <summary>
        /// 服务器IP
        /// </summary>
        private string url ;
        /// <summary>
        /// 服务器端口
        /// </summary>
        public string port;
        public string service;
        public string fileName;
        public string localFileMD5;
        private string savePath;
        private string dbName = "flipBook.db"; // 数据库名称
        private string dbPath; // 数据库路径
        private string filePathAndName ;
        public bool isUpdate;
        private string pathAndName;
        private int index = 0;
        private int num = 0;
        private DownLoadPanel loadPanel;
        private List<string> neadUpdateAssetList = new List<string>();
        public List<string> fileNameList = new List<string>();
        public List<string> fileMD5 = new List<string>();
        private Dictionary<string, string> filePathAndMD5 = new Dictionary<string, string>();
        private WWW progressWWW;
        private Thread thread;
        private WWWLoad wwwLoad;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ZipFile(Application.persistentDataPath+"/Books/ConfigContent/");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                UnZipFile(Application.persistentDataPath + "/temp2.zip");
            }
        }

        private void UnZipFile(string v)
        {
            using (FileStream stream = new FileStream(Application.persistentDataPath + "/temp2.zip", FileMode.Open, FileAccess.Read))
            {
                using (GZipStream gZip = new GZipStream(stream, CompressionMode.Decompress))
                {
                    FileStream fs = new FileStream(Application.persistentDataPath + "/test.txt",FileMode.Create);
                    byte[] by = new byte[1024 * 1024];
                    int r = gZip.Read(by, 0, by.Length);
                    while (r > 0)
                    {
                        fs.Write(by, 0, r);
                        r = gZip.Read(by, 0, r);
                    }
                }
            }
        }

        private void ZipFile(string fileName)
        {
            string[] strs = Directory.GetFiles(fileName);
            string text = "";
            FileStream stream = new FileStream(Application.persistentDataPath + "/temp2.zip", FileMode.Create, FileAccess.Write);
            GZipStream gZip = new GZipStream(stream, CompressionMode.Compress);
            StreamWriter writer = new StreamWriter(gZip);
            foreach (var item in strs)
            {
                Debug.Log(" ******* " + item);
                FileStream file = new FileStream(item, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(file);
                text += reader.ReadToEnd();
                reader.Dispose();
                file.Dispose();
            }
            
            writer.Write(text);
            writer.Dispose();
            gZip.Dispose();
            stream.Dispose();
        }

        public void Init()
        {
            savePath = Application.persistentDataPath + "/";
            wwwLoad = new WWWLoad();
            loadPanel = FindObjectOfType<DownLoadPanel>();
            //得到文件的MD5
            GetAllAssetOfMD5(Application.persistentDataPath + "/AssetBundles", GetOver);
            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                pathAndName = GameCore.Instance.LocalConfigPath;
                url = Application.streamingAssetsPath;
                dbPath = Application.streamingAssetsPath + "/" + dbName;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                pathAndName = GameCore.Instance.LocalConfigPath;
                //url = "jar:file://" + Application.dataPath + "!/assets";
                url = Application.streamingAssetsPath;
                localFileMD5 = Application.persistentDataPath + "/filemd5.txt";
                dbPath = Application.persistentDataPath + "/" + dbName;
                if (!File.Exists(dbPath)) // 如果数据库不存在,则复制到持久化目录下
                    StartCoroutine(CopyDB());
            }

            //ResourcesSaveToConfig.GetAllResMD5(Application.persistentDataPath + "/AssetBundle", Application.persistentDataPath + "/assetBundle.json");

            //thread = new Thread(ThreadDownLoadAsset);
            //thread.Start();
            //ThreadDownLoadAsset();
        }
        /// <summary>
        /// 开启线程进行资源更新
        /// </summary>
        private void ThreadDownLoadAsset()
        {
            filePathAndName = pathAndName + "/filePathAndName.json";
            
            Debug.Log(filePathAndName + " from checkisUpdate class ");
            if (!Directory.Exists(pathAndName))
            {
                Directory.CreateDirectory(pathAndName);
            }
            //如果当前没有下载过资源，则直接从服务器上下载
            if (!File.Exists(filePathAndName))
            {
                StartCoroutine(FirstDownLoadAsset());
            }
            //否则的话，就与服务器文件进行对比，判断是否需要进行资源更新
            else
            {
                //StartCoroutine(DownLoadAsset());

            }
            StartCoroutine(CheckUpdate());
        }

        /// <summary>
        /// 检查是否需要更新
        /// </summary>
        private IEnumerator CheckUpdate()
        {
            WWW www = new WWW(savePath + "/assetMD5.txt");
            yield return www;
            if (!File.Exists(Application.persistentDataPath + "/assetMD5.txt"))
            {
                File.WriteAllBytes(Application.persistentDataPath + "/assetMD5.txt", www.bytes);
                string[] str = File.ReadAllLines(Application.persistentDataPath + "/assetMD5.txt");
                foreach (string item in str)
                {
                    string temp = item.Split('|')[0];
                    neadUpdateAssetList.Add(temp);
                }
                UpdateAsset(GameCore.Instance.LocalConfigPath + "/assets");
            }
            else
            {
                CheckMD5IsNeadUpdate(www.text);
            }
        }
        /// <summary>
        /// 进行资源更新
        /// </summary>
        /// <param name="neadUpdateAssetList">需要更新的资源列表</param>
        /// <returns></returns>
        private void UpdateAsset(string savePath)
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            if (neadUpdateAssetList.Count <= 0) return;
            if (index >= neadUpdateAssetList.Count)
            {
                progressWWW.Dispose();
                progressWWW = null;
                //StartCoroutine(loadPanel.DownOver());
                loadPanel.DownOver();
                //需要更新的资源下载完毕,将需要更新的资源列表清空
                neadUpdateAssetList.Clear();
                Debug.Log("所有资源更新完毕");

                return;
            }
            StartCoroutine(GenerateAssetBundle(savePath, neadUpdateAssetList[index], () => {
                index++;
                UpdateAsset(savePath);
            }));
        }

        /// <summary>
        /// 创建AssetBundle资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">下载的资源保存路径</param>
        /// <param name="assetBundleName">资源名字</param>
        /// <param name="callBack">回调</param>
        public IEnumerator GenerateAssetBundle(string savePath, string assetBundleName, Action callBack)
        {
            Debug.Log(assetBundleName);
            progressWWW = new WWW( assetBundleName);
            loadPanel.StartDownLoad(progressWWW, index, neadUpdateAssetList.Count);
            while (!progressWWW.isDone)
            {
                yield return null;
                Debug.Log("正在下载资源...");
            }
            yield return progressWWW;
            string name = assetBundleName.Split('/')[8];
            if (progressWWW.isDone)
            {
                Debug.Log("资源下载完毕...");
                CreateFile(savePath + "/" + name, progressWWW.bytes, callBack);
                
            }
        }
        /// <summary>
        /// 通过获取文件的MD5值，判断是否需要更新
        /// </summary>
        /// <param name="content"></param>
        private void CheckMD5IsNeadUpdate(string content)
        {
            File.WriteAllText(Application.persistentDataPath + "/temp.txt", content);
            string[] temp = File.ReadAllLines(Application.persistentDataPath + "/temp.txt");
            string[] arr = File.ReadAllLines(Application.persistentDataPath + "/assetMD5.txt");
            for (int i = 0; i < temp.Length; i++)
            {
                bool isExist = false;
                for (int j = 0; j < arr.Length; j++)
                {
                    string t = temp[i].Split('|')[0];
                    if (arr[j].Split('|')[0].Equals(t))
                    {
                        isExist = true;
                        if (!arr[j].Split('|')[1].Equals(temp[i].Split('|')[1]))
                        {
                            neadUpdateAssetList.Add(t);
                        }
                    }
                }
                if(!isExist)
                    neadUpdateAssetList.Add(temp[i].Split('|')[0]);
            }
            File.Delete(Application.persistentDataPath + "/temp.txt");
            if (neadUpdateAssetList.Count > 0)
            {
                File.Delete(Application.persistentDataPath + "/assetMD5.txt");
                File.WriteAllText(Application.persistentDataPath + "/assetMD5.txt", content);
            }
            UpdateAsset(GameCore.Instance.LocalConfigPath + "/assets");
        }
        /// <summary>
        /// 下载资源目录文件，且开启下载
        /// </summary>
        /// <returns></returns>
        private void DownLoadAsset()
        {
            if (!Directory.Exists(pathAndName +"/AllBookImage"))
            {
                Directory.CreateDirectory(pathAndName + "/AllBookImage");
            }
            string str = File.ReadAllText(filePathAndName);

            Dictionary<string, List<string>> keyValues = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(str);
            DownBookAsset(keyValues);
        }
        /// <summary>
        /// 第一次下载资源
        /// </summary>
        /// <returns></returns>
        private IEnumerator FirstDownLoadAsset()
        {
            Debug.Log(url + "filePathAndName.json   from firstDownLoadAsset of checkIsUpdate class ");
            WWW www = new WWW(url + "/filePathAndName.json");
            yield return www;
            FileStream stream = File.Create(filePathAndName);
            stream.Write(www.bytes, 0, www.bytes.Length);
            stream.Close();
            stream.Dispose();
            DownLoadAsset();
        }
        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="keyValues">需要下载的资源列表</param>
        public void DownBookAsset(Dictionary<string,List<string>> keyValues)
        {
            loadPanel.Reset(Vector3.one, 0.3f);
            if (num <= keyValues.Keys.Count - 1)
            {
                string key = DictionaryHelp<string, List<string>>.GetKeyFromDictionary(keyValues, num);
                Debug.Log(key + " ------------ " + num);
                DownAsset(key, keyValues[key], () =>
                     {
                         num++;
                         //Debug.Log("这里是回调函数, and the count of value is " + keyValues[key].Count);
                         DownBookAsset(keyValues);
                     });
            }
            else
            {
                Debug.Log("下载结束");
                //开始下载最新资源
                CheckUpdate();
                loadPanel.DownOver();
            }
        }
        /// <summary>
        /// 进行资源下载
        /// </summary>
        /// <param name="key">需要下载的资源名</param>
        /// <param name="values">需要下载的资源列表</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private void DownAsset(string key,List<string> values,Action callBack)
        {
            if (index <= values.Count - 1)
            {
                string path = url + key + "/" + values[index];
                if (!Directory.Exists(pathAndName + key))
                    Directory.CreateDirectory(pathAndName + key);
                string filePath = pathAndName + key + "/" + values[index];
                StartCoroutine(WWWLoad.DownFile(path, filePath, loadPanel.StartDownLoad, () =>
                {
                    index++;
                    DownAsset(key, values, callBack);
                }, values[index]));
                Debug.Log(index);
                //WWW www = new WWW(path);
                //loadPanel.StartDownLoad(www, num, 5);
                //yield return www;


                //CreateFile(filePath, www, values[index], () =>
                //{
                //    index++;
                //    StartCoroutine(DownAsset(key, values, callBack));
                //});
            }
            //当资源更新结束
            else
            {
                index = 0;
                callBack();
            }

            //if (index <= values.Count - 1)
            //{
            //    string path = url + key + "/" + values[index];
            //    WWW www = new WWW(path);
            //    loadPanel.StartDownLoad(www, num, 5);
            //    yield return www;
            //    if (!Directory.Exists(pathAndName + key))
            //        Directory.CreateDirectory(pathAndName + key);
            //    string filePath = pathAndName + key + "/" + values[index];
            //    CreateFile(filePath, www, values[index], () =>
            //     {
            //         index++;
            //         StartCoroutine(DownAsset(key, values, callBack));
            //     });
            //}
            ////当资源更新结束
            //else
            //{
            //    yield return null;
            //    index = 0;
            //    callBack();
            //}
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
        /// <summary>
        /// 将下载的资源进行写入
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="www">下载的WWW</param>
        /// <param name="fileName">文件名</param>
        /// <param name="callBack">回调函数</param>
        private void CreateFile(string filePath,WWW www,string fileName,Action callBack)
        {
            //Debug.Log(filePath + " ******** " + fileName);
            FileStream stream = null;
            if (!File.Exists(filePath))
            {
                stream = File.Create(filePath);
                Texture2D texture = new Texture2D(www.texture.width, www.texture.height);
                www.LoadImageIntoTexture(texture);
                texture.name = fileName;
                byte[] bytes = texture.EncodeToJPG();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                stream.Dispose();
                if (callBack != null)
                    callBack();
            }
        }
        public void GetOver()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            Debug.Log(savePath+"  -------  ");
            string text = "";
            for (int i = 0; i < fileNameList.Count; i++)
            {
                text += string.Format("\r\n" + fileNameList[i] + "|" + fileMD5[i]);
            }
            text = text.TrimStart();
            File.WriteAllText(savePath + fileName, text);
            //string temp = JsonMapper.ToJson(filePathAndMD5);
            //Debug.Log(temp);
            //byte[] bytes = Encoding.UTF8.GetBytes(text);
            //FileStream stream = File.Open(savePath + fileName, FileMode.OpenOrCreate);
            //stream.Write(bytes, 0, bytes.Length);
            //stream.Close();
            //stream.Dispose();
        }
        /// <summary>
        /// 复制文件到持久化目录
        /// </summary>
        /// <returns></returns>
        private IEnumerator CopyDB()
        {
            // 从StreamingAssets目录使用WWW下载data.db
            WWW www = new WWW(Application.streamingAssetsPath + "/" + dbName);
            yield return www; // 等待下载完毕
            //下载完毕后写到persistentDataPath路径
            File.WriteAllBytes(dbPath, www.bytes);
        }
        /// <summary>
        /// 得到资源的MD5值
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public string GetAssetOfMD5(string assetPath)
        {
            try
            {
                FileStream file = File.Open(assetPath, FileMode.Open);
                //byte[] bytes = Encoding.UTF8.GetBytes(assetPath);
                MD5 fileMD5 = new MD5CryptoServiceProvider();
                byte[] md5Bytes = fileMD5.ComputeHash(file);
                //byte[] md5Bytes = fileMD5.ComputeHash(bytes);
                string str = "";
                foreach (var item in md5Bytes)
                {
                    str += Convert.ToString(item, 16);
                }
                return str;
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e);
                return "";
            }
        }
        public void GetAllAssetOfMD5(string assetPath, Action callBack)
        {
            string[] arr = Directory.GetFiles(assetPath);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].Replace('\\', '/');
                string md5 = GetAssetOfMD5(arr[i]);
                arr[i] = arr[i].Replace(Application.persistentDataPath + "/AssetBundles/", "");
                Debug.Log(arr[i]);
                filePathAndMD5[arr[i]] = md5;
                fileNameList.Add(arr[i]);
                fileMD5.Add(md5);
            }
            if (callBack != null)
                callBack();
        }
    }
}
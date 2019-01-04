
using PJW.Book;
using PJW.Datas;
using PJW.MVC.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace PJW.MVC.Model
{
    /// <summary>
    /// 资源是否更新处理
    /// </summary>
    public class ResourcesProxy : BaseProxy
    {
        public new const string NAME = "ResourcesProxy";
        private List<string> neadUpdateAssetList;
        private int index;
        //版本文件
        public static string MAIN_VERSION_FILE = "/assetMD5.txt";
        //服务器资源地址
        static string SERVER_RES_URL;
        //本地资源地址  
        static string LOCAL_RES_OUT_PATH = "";
        //本地解压资源  
        public static string LOCAL_DECOMPRESS_RES = "";
        //当前文件总数量  
        public long curFileTotalNum = 0;
        private WWW progressWWW;
        private Thread thread;
        public ResourcesProxy()
        {
            ProxyName = NAME;
            SERVER_RES_URL = @"ftp://192.168.1.110:66/";
            LOCAL_RES_OUT_PATH = Application.persistentDataPath + "/AssetBundles";
            LOCAL_DECOMPRESS_RES = LOCAL_RES_OUT_PATH + "/DeCompress";
            neadUpdateAssetList = new List<string>();
            MessageData = new MessageData();
        }
        /// <summary>
        /// 检查网络是否满足更新条件
        /// </summary>
        public void CheckNet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                MessageData.Color = Color.red;
                MessageData.Message = " 没有网络，请连接网络后再重新打开软件。";
                SendNotification(NotificationArray.UPDATE + NotificationArray.FAILURE, MessageData);
            }
            else
            {
                SendNotification(NotificationArray.CHECK + NotificationArray.UPDATE);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="rd">资源</param>
        /// <param name="isUpdate">是否可以更新</param>
        public void Update(ResourcesData rd)
        {
            foreach (string item in neadUpdateAssetList)
            {
                if (File.Exists(LOCAL_RES_OUT_PATH + item))
                    File.Delete(LOCAL_RES_OUT_PATH + item);
            }
            UpdateAsset();
        }
        /// <summary>
        /// 检查需要更新的资源
        /// </summary>
        public void CheckIsUpdate()
        {
            GameCore.Instance.StartCoroutine(CheckUpdate());
        }
        /// <summary>
        /// 检查是否需要更新
        /// </summary>
        private IEnumerator CheckUpdate()
        {
            WWW www = new WWW(SERVER_RES_URL + MAIN_VERSION_FILE);
            yield return www;
            //如果本地不存在主文件，则表示第一次下载，需要将全部资源进行更新
            if (!File.Exists(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE))
            {
                File.WriteAllBytes(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE, www.bytes);
                string[] str = File.ReadAllLines(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE);
                foreach (string item in str)
                {
                    string temp = item.Split('|')[0];
                    neadUpdateAssetList.Add(temp);
                }
            }
            else
            {
                CheckMD5IsNeadUpdate(www.text);
            }
            if (neadUpdateAssetList.Count > 0)
            {
                Debug.Log("有资源需要更新");
                if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                {
                    MessageData.Color = Color.red;
                    MessageData.Message = " 当前为4G网络，更新资源会消耗大量流量，确认继续更新? ";
                    MessageData.Type = NotificationArray.START + NotificationArray.UPDATE;
                    SendNotification(NotificationArray.UPDATE + NotificationArray.CHECK, MessageData);
                }
                else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    //SendNotification(NotificationArray.CHECK + NotificationArray.UPDATE);
                    SendNotification(NotificationArray.START + NotificationArray.UPDATE);
                }
            }
            else
            {
                Debug.Log("没有资源需要更新");
                SendNotification(NotificationArray.UPDATE + NotificationArray.SUCCESS);
            }
        }
        /// <summary>
        /// 进行资源更新
        /// </summary>
        /// <param name="neadUpdateAssetList">需要更新的资源列表</param>
        /// <returns></returns>
        private void UpdateAsset()
        {
            if (!Directory.Exists(LOCAL_RES_OUT_PATH))
                Directory.CreateDirectory(LOCAL_RES_OUT_PATH);
            if (neadUpdateAssetList.Count <= 0) return;
            if (index >= neadUpdateAssetList.Count)
            {
                progressWWW.Dispose();
                progressWWW = null;
                GameCore.Instance.GetDownLoadPanel().DownOver();
                //需要更新的资源下载完毕,将需要更新的资源列表清空
                neadUpdateAssetList.Clear();
                Debug.Log("所有资源更新完毕");
                SendNotification(NotificationArray.UPDATE + NotificationArray.SUCCESS);
                return;
            }
            GenerateAssetBundle(SERVER_RES_URL + "ABFiles/" + neadUpdateAssetList[index], () =>
            {
                index++;
                UpdateAsset();
            });
        }
        /// <summary>
        /// 创建AssetBundle资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">下载的资源保存路径</param>
        /// <param name="assetBundleName">资源名字</param>
        /// <param name="callBack">回调</param>
        public void GenerateAssetBundle(string path,  Action callBack)
        {
            Debug.Log(path);
            progressWWW = new WWW(path);
            GameCore.Instance.GetDownLoadPanel().StartDownLoad(progressWWW, index, neadUpdateAssetList.Count);
            while (!progressWWW.isDone)
            {
                Debug.Log("正在下载资源...");
            }
            string name = neadUpdateAssetList[index];
            if (progressWWW.isDone)
            {
                Debug.Log("资源下载完毕...");
                CreateFile(LOCAL_RES_OUT_PATH + "/" + name, progressWWW.bytes, callBack);
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
            fs.Close();
            fs.Dispose();
            progressWWW.Dispose();
            if (callBack != null)
                callBack();
        }
        /// <summary>
        /// 通过获取文件的MD5值，判断是否需要更新
        /// </summary>
        /// <param name="content"></param>
        private void CheckMD5IsNeadUpdate(string content)
        {
            File.WriteAllText(LOCAL_RES_OUT_PATH + "/temp.txt", content);
            string[] temp = File.ReadAllLines(LOCAL_RES_OUT_PATH + "/temp.txt");
            string[] arr = File.ReadAllLines(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE);
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
                if (!isExist)
                    neadUpdateAssetList.Add(temp[i].Split('|')[0]);
            }
            File.Delete(LOCAL_RES_OUT_PATH + "/temp.txt");
            if (neadUpdateAssetList.Count > 0)
            {
                File.Delete(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE);
                File.WriteAllText(LOCAL_RES_OUT_PATH + MAIN_VERSION_FILE, content);
            }
        }
    }
}
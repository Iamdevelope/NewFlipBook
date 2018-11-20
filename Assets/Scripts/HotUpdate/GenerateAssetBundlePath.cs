using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PJW.HotUpdate
{

    public class GenerateAssetBundlePath
    {
        private static WWW www;
        /// <summary>
        /// 创建AssetBundle资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">下载的资源保存路径</param>
        /// <param name="assetBundleName">资源名字</param>
        /// <param name="callBack">回调</param>
        public static IEnumerator GenerateAssetBundle( string url, string savePath, string assetBundleName, Action callBack)
        {
            www = new WWW(url + "/" + assetBundleName);
            while (!www.isDone)
            {
                yield return null;
                Debug.Log("正在下载资源...");
            }
            yield return www;
            if (www.isDone)
            {
                Debug.Log("资源下载完毕...");
                CreateFile(savePath + assetBundleName, www.bytes, callBack);
            }
        }
        /// <summary>
        /// 下载多个资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="assetNames"></param>
        /// <param name="callBack"></param>
        public static void GenerateAllAsset(string url,string savePath,List<string> assetNames,Action callBack)
        {

        }

        /// <summary>
        /// 将下载的资源保存到指定路劲
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="bytes"></param>
        private static void CreateFile(string savePath, byte[] bytes, Action callBack)
        {
            FileStream fs = new FileStream(savePath, FileMode.Append);
            fs.Write(bytes, 0, bytes.Length);
            //利用文件流进行写数据时，会进行缓存，Flush就是不让它缓存，直接写到文件
            fs.Flush();
            //关闭流，并将所有资源释放
            fs.Close();
            //释放流
            fs.Dispose();
            //释放www
            www.Dispose();
            if (callBack != null)
                callBack();
        }
    }
}

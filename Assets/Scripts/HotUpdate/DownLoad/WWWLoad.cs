using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace PJW.HotUpdate
{
    /// <summary>
    /// WWW进行资源加载
    /// </summary>
    public class WWWLoad
    {
        private static WWW www;
        static Stopwatch stopwatch = new Stopwatch();
        

        public static IEnumerator DownFile(string url, string savePath, Action<WWW> callBack = null, Action action = null, string fileName = null)
        {
            FileInfo fi = new FileInfo(savePath);
            stopwatch.Start();
            UnityEngine.Debug.Log("Start of time " + Time.realtimeSinceStartup);
            www = new WWW(url);
            while (!www.isDone)
            {
                yield return null;
                if (callBack != null)
                    callBack(www);
            }
            yield return www;
            if (www.isDone)
            {
                byte[] bytes = www.bytes;
                if (fileName == null)
                    CreateFile(savePath, bytes, action);
                else
                    CreateFile(savePath, www, fileName, action);
            }
        }
        /// <summary>
        /// 将下载的资源保存到指定路劲
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="bytes"></param>
        private static void CreateFile(string savePath, byte[] bytes,Action action)
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
            www = null;
            stopwatch.Stop();
            UnityEngine.Debug.Log("downover,used time " + stopwatch.ElapsedMilliseconds);
            UnityEngine.Debug.Log("end time is " + Time.realtimeSinceStartup);
            if (action != null)
                action();
        }
        /// <summary>
        /// 将下载的资源进行写入
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="www">下载的WWW</param>
        /// <param name="fileName">文件名</param>
        /// <param name="callBack">回调函数</param>
        private static void CreateFile(string filePath, WWW www, string fileName, Action callBack)
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
                www.Dispose();
                www = null;
                if (callBack != null)
                    callBack();
            }
        }
    }
}
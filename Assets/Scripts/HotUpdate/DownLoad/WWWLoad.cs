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
        private WWW www;
        static Stopwatch stopwatch = new Stopwatch();

        public IEnumerator DownFile(string url, string savePath, Action<WWW> callBack)
        {
            FileInfo fi = new FileInfo(savePath);
            stopwatch.Start();
            UnityEngine.Debug.Log("Start of time " + Time.realtimeSinceStartup);
            www = new WWW(url);
            while (!www.isDone)
            {
                yield return 0;
                if (callBack != null)
                    callBack(www);
            }
            yield return www;
            if (www.isDone)
            {
                byte[] bytes = www.bytes;
                CreateFile(savePath, bytes);
            }
        }
        /// <summary>
        /// 将下载的资源保存到指定路劲
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="bytes"></param>
        private void CreateFile(string savePath, byte[] bytes)
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
            stopwatch.Stop();
            UnityEngine.Debug.Log("downover,used time " + stopwatch.ElapsedMilliseconds);
            UnityEngine.Debug.Log("end time is " + Time.realtimeSinceStartup);
        }
    }
}
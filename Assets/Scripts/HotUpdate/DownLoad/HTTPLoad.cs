using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace PJW.HotUpdate
{
    /// <summary>
    /// 通过HTTP进行资源加载
    /// </summary>
    public class HTTPLoad
    {
        /// <summary>
        /// 下载进度
        /// </summary>
        static public float progress { get; private set; }
        /// <summary>
        /// 标识，由于下载采用子线程下载，所以需要控制是否停止
        /// </summary>
        static private bool isStop;
        /// <summary>
        /// 是否下载完成
        /// </summary>
        public static bool isDone { get; private set; }
        /// <summary>
        /// 用于下载资源的子线程
        /// </summary>
        private static Thread thread;
        /// <summary>
        /// 读写时时长等待
        /// </summary>
        const int ReadWriteTimeOut = 2 * 1000;
        /// <summary>
        /// 时长等待
        /// </summary>
        const int TimeOutWait = 5 * 1000;
        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="url">下载的路径</param>
        /// <param name="savePath">保存的路径</param>
        /// <param name="fileName">保存的文件名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="threadPriority"></param>
        public static void DownLoad(string url, string savePath, string fileName, Action callBack, System.Threading.ThreadPriority threadPriority = System.Threading.ThreadPriority.Normal)
        {
            isStop = false;
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //利用子线程进行资源下载
            thread = new Thread(delegate ()
              {
                  stopwatch.Start();
                  //判断路径是否存在
                  if (Directory.Exists(savePath))
                      Directory.Delete(savePath,true);
                  Directory.CreateDirectory(savePath);
                  //这是要下载的文件名，比如从服务器下载a.zip到D盘，保存的文件名是test
                  string filePath = savePath + "/" + fileName;
                  //使用流操作文件
                  FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                  //获取文件现在的长度
                  long fileLength = fs.Length;
                  //获取下载文件的总长度
                  long totalLength = GetLength(url);
                  Debug.Log(url + " " + fileName + "  " + totalLength + "已下载的文件大小：" + fileLength);
                  Debug.LogFormat("<color=green>文件:{0} 已下载{1}M，剩余{2}M</color>", fileName, fileLength / 1024 / 1024, (totalLength - fileLength) / 1024 / 1024);
                  //如果没下载完
                  if (fileLength < totalLength)
                  {
                      //断点续传核心，设置本地文件流的起始位置
                      fs.Seek(fileLength, SeekOrigin.Begin);
                      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                      //request.ReadWriteTimeout = ReadWriteTimeOut;
                      request.Timeout = TimeOutWait;
                      //断点续传核心，设置远程访问文件流的起始位置
                      //request.AddRange((int)fileLength);
                      Stream stream = request.GetResponse().GetResponseStream();
                      byte[] buffer = new byte[1024];
                      //使用流读取内容到buffer中
                      //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
                      int length = stream.Read(buffer, 0, buffer.Length);
                      while (length > 0)
                      {
                          //如果客户端关闭，停止下载
                          if (isStop) break;
                          //将内容再写入本地文件中
                          fs.Write(buffer, 0, length);
                          //计算进度
                          fileLength += length;
                          progress = (float)fileLength / (float)totalLength;
                          //类似尾递归
                          length = stream.Read(buffer, 0, buffer.Length);

                      }
                      stream.Close();
                      stream.Dispose();
                  }
                  else
                      progress = 1;
                  stopwatch.Stop();
                  Debug.Log("download time " + stopwatch.ElapsedMilliseconds);
                  fs.Close();
                  fs.Dispose();
                  //如果下载完毕，执行回调
                  if (progress == 1)
                  {
                      isDone = true;
                      if (callBack != null) callBack();
                      thread.Abort();
                  }
              });
            //开启子线程
            thread.IsBackground = true;
            thread.Priority = threadPriority;
            thread.Start();
        }
        /// <summary>
        /// 得到URL中资源的长度
        /// </summary>
        /// <param name="url"></param>
        static private long GetLength(string url)
        {
            WebResponse response = null;
            Debug.Log(url);
            try
            {
                WebRequest wr = WebRequest.Create(url);
                wr.Method = "HEAD";
                response = wr.GetResponse();
                //hwp.Method = "HEAD";
            }
            catch(WebException e)
            {
                Debug.Log(e);
            }
            return response.ContentLength;
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="url">下载的路径</param>
        /// <param name="savePath">保存的路径</param>
        /// <param name="fileName">保存的文件名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="threadPriority"></param>
        static public void DownLoadByFTP(string url, string savePath, string fileName, Action callBack, System.Threading.ThreadPriority threadPriority = System.Threading.ThreadPriority.Normal)
        {
            isStop = false;
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //利用子线程进行资源下载
            thread = new Thread(delegate ()
            {
                stopwatch.Start();
                //判断路径是否存在
                if (Directory.Exists(savePath))
                    Directory.Delete(savePath, true);
                Directory.CreateDirectory(savePath);
                //这是要下载的文件名，比如从服务器下载a.zip到D盘，保存的文件名是test
                string filePath = savePath + "/" + fileName;
                //使用流操作文件
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                //获取文件现在的长度
                long fileLength = fs.Length;
                //获取下载文件的总长度
                long totalLength = GetLengthInFTP(url);
                Debug.Log(url + " " + fileName + "  " + totalLength + "已下载的文件大小：" + fileLength);
                Debug.LogFormat("<color=green>文件:{0} 已下载{1}M，剩余{2}M</color>", fileName, fileLength / 1024 / 1024, (totalLength - fileLength) / 1024 / 1024);
                //如果没下载完
                if (fileLength < totalLength)
                {
                    //断点续传核心，设置本地文件流的起始位置
                    fs.Seek(fileLength, SeekOrigin.Begin);
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                    //request.ReadWriteTimeout = ReadWriteTimeOut;
                    request.Timeout = TimeOutWait;
                    //断点续传核心，设置远程访问文件流的起始位置
                    //request.AddRange((int)fileLength);
                    Stream stream = request.GetResponse().GetResponseStream();
                    byte[] buffer = new byte[1024];
                    //使用流读取内容到buffer中
                    //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
                    int length = stream.Read(buffer, 0, buffer.Length);
                    while (length > 0)
                    {
                        //如果客户端关闭，停止下载
                        if (isStop) break;
                        //将内容再写入本地文件中
                        fs.Write(buffer, 0, length);
                        //计算进度
                        fileLength += length;
                        progress = (float)fileLength / (float)totalLength;
                        //类似尾递归
                        length = stream.Read(buffer, 0, buffer.Length);

                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                    progress = 1;
                stopwatch.Stop();
                Debug.Log("download time " + stopwatch.ElapsedMilliseconds);
                fs.Close();
                fs.Dispose();
                //如果下载完毕，执行回调
                if (progress == 1)
                {
                    isDone = true;
                    if (callBack != null) callBack();
                    thread.Abort();
                }
            });
            //开启子线程
            thread.IsBackground = true;
            thread.Priority = threadPriority;
            thread.Start();
        }

        static private long GetLengthInFTP(string url)
        {
            FtpWebResponse response = null;
            Debug.Log(url);
            try
            {
                FtpWebRequest wr = (FtpWebRequest)WebRequest.Create(url);
                //wr.Method = "HEAD";
                response = (FtpWebResponse)wr.GetResponse();
                //hwp.Method = "HEAD";
            }
            catch (WebException e)
            {
                Debug.Log(e);
            }
            return response.ContentLength;
        }

        /// <summary>
        /// 程序中途退出，停止下载
        /// </summary>
        static public void Close()
        {
            isStop = true;
        }
    }
}
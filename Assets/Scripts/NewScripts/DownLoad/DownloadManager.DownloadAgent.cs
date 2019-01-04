using PJW.Task;
using System;
using System.IO;

namespace PJW.Download
{
    internal partial class DownloadManager
    {
        /// <summary>
        /// 下载代理器
        /// </summary>
        private sealed class DownloadAgent : ITaskAgent<DownloadTask>, IDisposable
        {
            private readonly IDownLoadAgentManager downLoadAgentManager;
            private DownloadTask task;
            private FileStream fileStream;
            private float flushSize;
            private float waitTime;
            private int startLength;
            private int downloadLength;
            private int saveLength;
            /// <summary>
            /// 资源是否已经释放
            /// </summary>
            private bool disposed;

            public FrameworkAction<DownloadAgent> DownloadAgentStart;
            public FrameworkAction<DownloadAgent, int> DownloadAgentUpdate;
            public FrameworkAction<DownloadAgent, int> DownloadAgentSuccess;
            public FrameworkAction<DownloadAgent, string> DownloadAgentFailure;

            public DownloadAgent(IDownLoadAgentManager downLoadAgentManager)
            {
                if (downLoadAgentManager == null)
                {
                    throw new FrameworkException(" Download agent manager is inviald ");
                }
                this.downLoadAgentManager = downLoadAgentManager;
                task = null;
                fileStream = null;
                flushSize = 0;
                waitTime = 0;
                startLength = 0;
                downloadLength = 0;
                saveLength = 0;
                disposed = false;

                DownloadAgentStart = null;
                DownloadAgentUpdate = null;
                DownloadAgentSuccess = null;
                DownloadAgentFailure = null;

            }
            /// <summary>
            /// 获取本次任务
            /// </summary>
            public DownloadTask GetTask
            {
                get
                {
                    return task;
                }
            }
            /// <summary>
            /// 获取已经等待的时间
            /// </summary>
            public float GetWaitTime
            {
                get { return waitTime; }
            }
            /// <summary>
            /// 获取开始下载时已经存在的文件大小
            /// </summary>
            public int GetStartLength
            {
                get { return startLength; }
            }
            /// <summary>
            /// 获取本次已经下载的大小
            /// </summary>
            public int GetDownloadLength
            {
                get { return downloadLength; }
            }
            /// <summary>
            /// 获取当前大小
            /// </summary>
            public int GetCurrentLength
            {
                get { return startLength += downloadLength; }
            }
            /// <summary>
            /// 获取已经存储的大小
            /// </summary>
            public long SavedLength
            {
                get { return saveLength; }
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            /// <param name="disposing"></param>
            private void Dispose(bool disposing)
            {
                if (disposed)
                {
                    return;
                }
                if (disposing)
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }
                disposed = true;
            }
            /// <summary>
            /// 初始化下载代理
            /// </summary>
            public void Init()
            {
                downLoadAgentManager.DownloadAgentErrorHandler += OnDownloadAgentError;
                downLoadAgentManager.DownloadAgentSuccessHandler += OnDownloadAgentSuccess;
                downLoadAgentManager.DownloadAgentUpdateHandler += OnDownloadAgentUpdate;
            }
            private void OnDownloadAgentUpdate(object sender,DownloadAgentManagerUpdateEventAvgs e)
            {
                waitTime = 0;
                byte[] bytes = e.GetBytes();
                SaveBytes(bytes);
                downloadLength = e.Length;
                if (DownloadAgentUpdate != null)
                {
                    DownloadAgentUpdate(this, bytes != null ? bytes.Length : 0);
                }
            }
            private void OnDownloadAgentSuccess(object sender,DownloadAgentManagerSuccessEventAvgs e)
            {
                waitTime = 0;
                byte[] bytes = e.GetBytes();
                SaveBytes(bytes);
                downloadLength = e.Length;
                if (saveLength != GetCurrentLength)
                {
                    throw new FrameworkException(" Internale download error ");
                }
                downLoadAgentManager.Reset();
                fileStream.Close();
                fileStream = null;
                if (File.Exists(task.GetDownloadPath))
                {
                    File.Delete(task.GetDownloadPath);
                }
                File.Move(Utility.Text.Format("{0}.download", task.GetDownloadPath),task.GetDownloadPath);
                task.DownloadTaskStatus = DownloadTaskStatus.Done;
                if (DownloadAgentSuccess != null)
                {
                    DownloadAgentSuccess(this, bytes != null ? bytes.Length : 0);
                }
                task.Done = true;
            }
            private void OnDownloadAgentError(object sender, DownloadAgentManagerErrorEventAvgs e)
            {
                downLoadAgentManager.Reset();
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream = null;
                }
                task.DownloadTaskStatus = DownloadTaskStatus.Error;
                if (DownloadAgentFailure != null)
                {
                    DownloadAgentFailure(this, e.ErrorMessage);
                }
                task.Done = true;
            }
            private void SaveBytes(byte[] bytes)
            {
                if (bytes == null)
                {
                    return;
                }
                try
                {
                    int length = bytes.Length;
                    fileStream.Write(bytes, 0, length);
                    flushSize += length;
                    saveLength += length;
                    if (flushSize >= task.GetFlushSize)
                    {
                        fileStream.Flush();
                        flushSize = 0;
                    }
                }
                catch(Exception e)
                {
                    OnDownloadAgentError(this, new DownloadAgentManagerErrorEventAvgs(e.Message));
                }
            }
            /// <summary>
            /// 重置下载代理器
            /// </summary>
            public void Reset()
            {
                downLoadAgentManager.Reset();
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream = null;
                }
                task = null;
                flushSize = 0;
                waitTime = 0;
                startLength = 0;
                downloadLength = 0;
                saveLength = 0;
            }
            /// <summary>
            /// 关闭并清理下载代理
            /// </summary>
            public void Shutdown()
            {
                Dispose();
                downLoadAgentManager.DownloadAgentErrorHandler -= OnDownloadAgentError;
                downLoadAgentManager.DownloadAgentSuccessHandler -= OnDownloadAgentSuccess;
                downLoadAgentManager.DownloadAgentUpdateHandler -= OnDownloadAgentUpdate;
            }
            /// <summary>
            /// 开始处理下载任务
            /// </summary>
            /// <param name="task"></param>
            public void Start(DownloadTask task)
            {
                if (task == null)
                {
                    throw new FrameworkException(" Task is invalid ");
                }
                this.task = task;
                task.DownloadTaskStatus = DownloadTaskStatus.Doing;
                string downloadFile = Utility.Text.Format("{0}.download", task.GetDownloadPath);
                try
                {
                    if (File.Exists(downloadFile))
                    {
                        fileStream = File.OpenWrite(downloadFile);
                        fileStream.Seek(0, SeekOrigin.End);
                        startLength = saveLength = (int)fileStream.Length;
                        downloadLength = 0;
                    }
                    else
                    {
                        string directory = Path.GetDirectoryName(task.GetDownloadPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        fileStream = new FileStream(downloadFile, FileMode.Create, FileAccess.Write);
                        startLength = saveLength = downloadLength = 0;
                    }
                    if (DownloadAgentStart != null)
                    {
                        DownloadAgentStart(this);
                    }
                    if (startLength > 0)
                    {
                        downLoadAgentManager.Download(task.GetDownloadUrl, startLength, task.GetUserData);
                    }
                    else
                    {
                        downLoadAgentManager.Download(task.GetDownloadUrl, task.GetUserData);
                    }
                }
                catch(Exception e)
                {
                    OnDownloadAgentError(this, new DownloadAgentManagerErrorEventAvgs(e.Message));
                }
            }
            /// <summary>
            /// 下载代理器轮询
            /// </summary>
            /// <param name="elapseSeconds"></param>
            /// <param name="realElapseSeconds"></param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (task.DownloadTaskStatus == DownloadTaskStatus.Doing)
                {
                    waitTime += realElapseSeconds;
                    if (waitTime >= task.GetTimeOut)
                    {
                        OnDownloadAgentError(this, new DownloadAgentManagerErrorEventAvgs("TimeOut"));
                    }
                }
            }
        }
    }
}
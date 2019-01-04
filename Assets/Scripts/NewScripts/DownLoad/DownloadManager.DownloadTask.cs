
using PJW.Task;

namespace PJW.Download
{
    internal partial class DownloadManager
    {
        /// <summary>
        /// 下载任务
        /// </summary>
        public sealed class DownloadTask : ITask
        {
            private static int serial;

            private readonly int m_SerialId;
            private readonly int m_Priority;
            private bool m_Done;
            private DownloadTaskStatus downloadTaskStatus;
            private readonly string downloadPath;
            private readonly string downloadUrl;
            private readonly float flushSize;
            private readonly float timeOut;
            private readonly object userData;
            /// <summary>
            /// 获取任务序列号
            /// </summary>
            public int GetSerialId
            {
                get
                {
                    return m_SerialId;
                }
            }
            /// <summary>
            /// 获取下载任务优先级
            /// </summary>
            public int GetPriority
            {
                get
                {
                    return m_Priority;
                }
            }
            /// <summary>
            /// 获取下载任务是否完成
            /// </summary>
            public bool Done
            {
                get
                {
                    return m_Done;
                }
                set { m_Done = value; }
            }
            /// <summary>
            /// 下载任务状态
            /// </summary>
            public DownloadTaskStatus DownloadTaskStatus
            {
                get { return downloadTaskStatus; }
                set { downloadTaskStatus = value; }
            }
            /// <summary>
            /// 获取下载保存路径
            /// </summary>
            public string GetDownloadPath
            {
                get { return downloadPath; }
            }
            /// <summary>
            /// 获取数据下载地址
            /// </summary>
            public string GetDownloadUrl
            {
                get { return downloadUrl; }
            }
            /// <summary>
            /// 获取将缓冲区写入磁盘的临界大小
            /// </summary>
            public float GetFlushSize
            {
                get { return flushSize; }
            }
            /// <summary>
            /// 获取下载超时时长，以秒为单位
            /// </summary>
            public float GetTimeOut
            {
                get { return timeOut; }
            }
            /// <summary>
            /// 获取用户自定义数据
            /// </summary>
            public object GetUserData
            {
                get { return userData; }
            }
            /// <summary>
            /// 初始化下载任务的构造函数
            /// </summary>
            /// <param name="downloadPath">下载后存放路径</param>
            /// <param name="downloadUrl">下载的地址</param>
            /// <param name="flushSize">将缓冲区写入磁盘的临界大小</param>
            /// <param name="timeOut">下载超时时长</param>
            /// <param name="userData">用户自定义数据</param>
            /// <param name="priority">下载任务优先级</param>
            public DownloadTask(string downloadPath,string downloadUrl,float flushSize,float timeOut,object userData,int priority)
            {
                m_SerialId = serial++;
                m_Priority = priority;
                m_Done = false;
                downloadTaskStatus = DownloadTaskStatus.Todo;
                this.downloadPath = downloadPath;
                this.downloadUrl = downloadUrl;
                this.flushSize = flushSize;
                this.timeOut = timeOut;
                this.userData = userData;
            }
        }
    }
}
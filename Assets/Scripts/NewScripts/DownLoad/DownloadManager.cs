
using PJW.Task;
using System;

namespace PJW.Download
{
    /// <summary>
    /// 下载管理器
    /// </summary>
    internal partial class DownloadManager : FrameworkModule, IDownLoadManager
    {
        private readonly TaskPool<DownloadTask> taskPool;
        private readonly DownloadCounter downloadCounter;
        private float flushSize;
        private float timeOut;
        private EventHandler<DownloadStartEventArgs> downloadStartEventHandler;
        private EventHandler<DownloadFailureEventArgs> downloadFailureEventHandler;
        private EventHandler<DownloadSuccessEventArgs> downloadSuccessEventHandler;
        private EventHandler<DownloadUpdateEventArgs> downloadUpdateEventHandler;
        /// <summary>
        /// 初始化下载管理器
        /// </summary>
        public DownloadManager()
        {
            taskPool = new TaskPool<DownloadTask>();
            downloadCounter = new DownloadCounter(1, 30);
            flushSize = 1024 * 1024;
            timeOut = 30;
            downloadStartEventHandler = null;
            downloadFailureEventHandler = null;
            downloadSuccessEventHandler = null;
            downloadUpdateEventHandler = null;
        }
        /// <summary>
        /// 获取框架模块优先级
        /// </summary>
        public override int Priority
        {
            get { return 80; }
        }
        /// <summary>
        /// 下载代理总个数
        /// </summary>
        public int TotalAgentCount
        {
            get { return taskPool.GetTotalAgentCount; }
        }
        /// <summary>
        /// 可用下载代理个数
        /// </summary>
        public int CanuseAgentCount
        {
            get { return taskPool.GetFreeAgentCount; }
        }
        /// <summary>
        /// 获取正在工作中的下载代理个数
        /// </summary>
        public int WorkingAgentCount
        {
            get { return taskPool.GetWorkingAgentCount; }
        }
        /// <summary>
        /// 正在等待下载的任务数量
        /// </summary>
        public int WaitingTaskCount
        {
            get { return taskPool.GetWaitingAgentCount; }
        }
        /// <summary>
        /// 获取或设置将缓冲区写入磁盘的临界大小
        /// </summary>
        public float FlushSize
        {
            get { return flushSize; }
            set { flushSize = value; }
        }
        /// <summary>
        /// 超时时长
        /// </summary>
        public float Timeout
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        /// <summary>
        /// 当前下载速度
        /// </summary>
        public float CurrentSpeed
        {
            get { return downloadCounter.GetCurrentSpeed; }
        }
        /// <summary>
        /// 下载开始事件
        /// </summary>
        public event EventHandler<DownloadStartEventArgs> DownLoadStartHandler
        {
            add
            {
                downloadStartEventHandler += value;
            }
            remove
            {
                downloadStartEventHandler -= value;
            }
        }
        /// <summary>
        /// 下载完成事件
        /// </summary>
        public event EventHandler<DownloadSuccessEventArgs> DownLoadSuccessHandler
        {
            add
            {
                downloadSuccessEventHandler += value;
            }
            remove
            {
                downloadSuccessEventHandler -= value;
            }
        }
        /// <summary>
        /// 下载失败事件
        /// </summary>
        public event EventHandler<DownloadFailureEventArgs> DownLoadFailureHandler
        {
            add
            {
                downloadFailureEventHandler += value;
            }
            remove
            {
                downloadFailureEventHandler -= value;
            }
        }
        /// <summary>
        /// 下载更新事件
        /// </summary>
        public event EventHandler<DownloadUpdateEventArgs> DownLoadUpdateHandler
        {
            add
            {
                downloadUpdateEventHandler += value;
            }
            remove
            {
                downloadUpdateEventHandler -= value;
            }
        }
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">下载存放地址</param>
        /// <param name="Downloadurl">下载地址</param>
        /// <returns>新增下载任务的序列编号</returns>
        public int AddDownload(string DownloadPath, string Downloadurl)
        {
            return AddDownload(DownloadPath, Downloadurl, Constant.DefaultPriority, null);
        }
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">下载存放地址</param>
        /// <param name="Downloadurl">下载地址</param>
        /// <param name="priority">下载优先级</param>
        /// <returns>新增下载任务的序列编号</returns>
        public int AddDownload(string DownloadPath, string Downloadurl, int priority)
        {
            return AddDownload(DownloadPath, Downloadurl, priority, null);
        }
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">下载存放地址</param>
        /// <param name="Downloadurl">下载地址</param>
        /// <param name="Userdata">用户自定义数据</param>
        /// <returns>新增下载任务的序列编号</returns>
        public int AddDownload(string DownloadPath, string Downloadurl, object Userdata)
        {
            return AddDownload(DownloadPath, Downloadurl, Constant.DefaultPriority, Userdata);
        }
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">下载存放地址</param>
        /// <param name="Downloadurl">下载地址</param>
        /// <param name="priority">下载优先级</param>
        /// <param name="Userdata">用户自定义数据</param>
        /// <returns>新增下载任务的序列编号</returns>
        public int AddDownload(string DownloadPath, string Downloadurl, int priority, object Userdata)
        {
            if (string.IsNullOrEmpty(DownloadPath))
            {
                throw new FrameworkException(" Download Path is invalid ");
            }
            if (string.IsNullOrEmpty(Downloadurl))
            {
                throw new FrameworkException(" Download url is invalid ");
            }
            if (TotalAgentCount <= 0)
            {
                throw new FrameworkException(" you must add first agent ");
            }
            DownloadTask downloadTask = new DownloadTask(DownloadPath, Downloadurl, flushSize, timeOut, Userdata, priority);
            taskPool.AddTask(downloadTask);
            return downloadTask.GetSerialId;
        }
        /// <summary>
        /// 添加下载代理
        /// </summary>
        /// <param name="downLoadAgentManager"></param>
        public void AddDownloadAgent(IDownLoadAgentManager downLoadAgentManager)
        {
            DownloadAgent agent = new DownloadAgent(downLoadAgentManager);
            agent.DownloadAgentStart += OnDownloadAgentStart;
            agent.DownloadAgentFailure += OnDownloadAgentFailure;
            agent.DownloadAgentSuccess += OnDownloadAgentSuccess;
            agent.DownloadAgentUpdate += OnDownloadAgentUpdate;
            taskPool.AddAgent(agent);
        }
        /// <summary>
        /// 下载代理器更新方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lastDownloadLength"></param>
        private void OnDownloadAgentUpdate(DownloadAgent sender, int lastDownloadLength)
        {
            downloadCounter.RecordDownloadLength(lastDownloadLength);
            if (downloadUpdateEventHandler != null)
            {
                downloadUpdateEventHandler(this, new DownloadUpdateEventArgs(sender.GetTask.GetSerialId, sender.GetTask.GetDownloadPath, sender.GetTask.GetDownloadUrl, sender.GetCurrentLength, sender.GetTask.GetUserData));
            }
        }
        /// <summary>
        /// 下载代理器下载成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lastDownloadLength"></param>
        private void OnDownloadAgentSuccess(DownloadAgent sender, int lastDownloadLength)
        {
            downloadCounter.RecordDownloadLength(lastDownloadLength);
            if (downloadSuccessEventHandler != null)
            {
                downloadSuccessEventHandler(this, new DownloadSuccessEventArgs(sender.GetTask.GetSerialId, sender.GetTask.GetDownloadPath, sender.GetTask.GetDownloadUrl, sender.GetCurrentLength, sender.GetTask.GetUserData));
            }
        }
        /// <summary>
        /// 下载失败时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="errorMessage"></param>
        private void OnDownloadAgentFailure(DownloadAgent sender, string errorMessage)
        {
            if (downloadFailureEventHandler != null)
            {
                downloadFailureEventHandler(this, new DownloadFailureEventArgs(sender.GetTask.GetSerialId, sender.GetTask.GetDownloadPath, sender.GetTask.GetDownloadUrl, errorMessage, sender.GetTask.GetUserData));
            }
        }
        /// <summary>
        /// 在开始下载时调用
        /// </summary>
        /// <param name="sender"></param>
        private void OnDownloadAgentStart(DownloadAgent sender)
        {
            if (downloadStartEventHandler != null)
            {
                downloadStartEventHandler(this, new DownloadStartEventArgs(sender.GetTask.GetSerialId, sender.GetTask.GetDownloadPath, sender.GetTask.GetDownloadUrl, sender.GetCurrentLength, sender.GetTask.GetUserData));
            }
        }

        /// <summary>
        /// 移除所有任务
        /// </summary>
        public void RemoveAllDownload()
        {
            taskPool.RemoveAllTasks();
        }
        /// <summary>
        /// 移除下载任务
        /// </summary>
        /// <param name="serialId">需要移除的下载任务的序列号</param>
        public bool RemoveDownload(int serialId)
        {
            return taskPool.RemoveTask(serialId) != null;
        }
        /// <summary>
        /// 关闭并清理管理器
        /// </summary>
        public override void Shutdown()
        {
            taskPool.Shutdown();
            downloadCounter.Shutdown();
        }
        /// <summary>
        /// 管理器轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            taskPool.Update(elapseSeconds, realElapseSeconds);
            downloadCounter.Update(elapseSeconds, realElapseSeconds);
        }
    }
}
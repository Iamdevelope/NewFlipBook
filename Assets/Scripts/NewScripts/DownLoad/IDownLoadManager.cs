
using System;

namespace PJW.Download
{
    /// <summary>
    /// 下载管理器接口
    /// </summary>
    public interface IDownLoadManager
    {
        /// <summary>
        /// 下载代理总个数
        /// </summary>
        int TotalAgentCount
        {
            get;
        }
        /// <summary>
        /// 可用下载代理个数
        /// </summary>
        int CanuseAgentCount
        {
            get;
        }
        /// <summary>
        /// 获取正在工作中的下载代理个数
        /// </summary>
        int WorkingAgentCount
        {
            get;
        }
        /// <summary>
        /// 正在等待下载的任务数量
        /// </summary>
        int WaitingTaskCount
        {
            get;
        }
        /// <summary>
        /// 获取或设置将缓冲区写入磁盘的临界大小
        /// </summary>
        float FlushSize
        {
            get;
        }
        /// <summary>
        /// 获取或设置下载超时时长
        /// </summary>
        float Timeout
        {
            get;
        }
        /// <summary>
        /// 获取当前下载速度
        /// </summary>
        float CurrentSpeed
        {
            get;
        }
        /// <summary>
        /// 下载开始事件
        /// </summary>
        event EventHandler<DownloadStartEventAvgs> DownLoadStartHandler;
        /// <summary>
        /// 下载成功事件
        /// </summary>
        event EventHandler<DownloadSuccessEventAvgs> DownLoadSuccessHandler;
        /// <summary>
        /// 下载失败事件
        /// </summary>
        event EventHandler<DownloadFailureEventAvgs> DownLoadFailureHandler;
        /// <summary>
        /// 下载更新事件
        /// </summary>
        event EventHandler<DownloadUpdateEventAvgs> DownLoadUpdateHandler;
        /// <summary>
        /// 新增下载辅助器
        /// </summary>
        /// <param name="downLoadAgentManager"></param>
        void AddDownloadAgent(IDownLoadAgentManager downLoadAgentManager);
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">资源保存路径</param>
        /// <param name="Downloadurl">资源服务器地址</param>
        /// <returns>返回资源序列号</returns>
        int AddDownload(string DownloadPath, string Downloadurl);
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">资源保存路径</param>
        /// <param name="Downloadurl">资源服务器地址</param>
        /// <param name="priority">资源下载优先级</param>
        /// <returns>返回资源序列号</returns>
        int AddDownload(string DownloadPath, string Downloadurl, int priority);
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">资源保存路径</param>
        /// <param name="Downloadurl">资源服务器地址</param>
        /// <param name="Userdata">用户自定义数据</param>
        /// <returns>返回资源序列号</returns>
        int AddDownload(string DownloadPath, string Downloadurl, object Userdata);
        /// <summary>
        /// 新增下载任务
        /// </summary>
        /// <param name="DownloadPath">资源保存路径</param>
        /// <param name="Downloadurl">资源服务器地址</param>
        /// <param name="priority">资源下载优先级</param>
        /// <param name="Userdata">用户自定义数据</param>
        /// <returns>返回资源序列号</returns>
        int AddDownload(string DownloadPath, string Downloadurl, int priority, object Userdata);
        /// <summary>
        /// 移除下载任务
        /// </summary>
        /// <param name="serialId">需要移除的下载任务的序列号</param>
        bool RemoveDownload(int serialId);
        /// <summary>
        /// 移除所有下载任务
        /// </summary>
        void RemoveAllDownload();
    }
}
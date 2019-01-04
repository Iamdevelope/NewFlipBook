using System;

namespace PJW.Download
{
    /// <summary>
    /// 下载辅助器管理器接口
    /// </summary>
    public interface IDownLoadAgentManager
    {
        /// <summary>
        /// 下载代理其器错误事件
        /// </summary>
        event EventHandler<DownloadAgentManagerErrorEventAvgs> DownloadAgentErrorHandler;
        /// <summary>
        /// 下载代理器完成事件
        /// </summary>
        event EventHandler<DownloadAgentManagerSuccessEventAvgs> DownloadAgentSuccessHandler;
        /// <summary>
        /// 下载代理器更新事件
        /// </summary>
        event EventHandler<DownloadAgentManagerUpdateEventAvgs> DownloadAgentUpdateHandler;
        /// <summary>
        /// 下载指定地址的数据
        /// </summary>
        /// <param name="downloadUrl">下载地址</param>
        /// <param name="userData">用户自定义数据</param>
        void Download(string downloadUrl, object userData);
        /// <summary>
        /// 下载指定地址的数据
        /// </summary>
        /// <param name="downloadUrl">下载地址</param>
        /// <param name="formPosition">下载的起始位置</param>
        /// <param name="userData">用户自定义数据</param>
        void Download(string downloadUrl, float formPosition, object userData);
        /// <summary>
        /// 下载指定地址的数据
        /// </summary>
        /// <param name="downloadUrl">下载地址</param>
        /// <param name="formPosition">下载的起始位置</param>
        /// <param name="toPosition">下载的结束位置</param>
        /// <param name="userData">用户自定义数据</param>
        void Download(string downloadUrl, float formPosition, float toPosition, object userData);
        /// <summary>
        /// 重置代理器
        /// </summary>
        void Reset();
    }
}
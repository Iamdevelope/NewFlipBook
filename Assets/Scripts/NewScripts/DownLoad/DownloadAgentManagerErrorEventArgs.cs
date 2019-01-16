
namespace PJW.Download
{
    /// <summary>
    /// 下载代理器下载错误事件
    /// </summary>
    public class DownloadAgentManagerErrorEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 管理器下载错误构造函数
        /// </summary>
        /// <param name="errorMessage"></param>
        public DownloadAgentManagerErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
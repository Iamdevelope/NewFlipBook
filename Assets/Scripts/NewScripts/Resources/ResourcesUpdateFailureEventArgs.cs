

namespace PJW.Resources
{
    /// <summary>
    /// 资源更新失败事件
    /// </summary>
    public class ResourcesUpdateFailureEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新失败事件的新实例。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="downloadUrl">下载地址。</param>
        /// <param name="retryCount">已重试次数。</param>
        /// <param name="totalRetryCount">设定的重试次数。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <remarks>当已重试次数达到设定的重试次数时，将不再重试。</remarks>
        public ResourcesUpdateFailureEventArgs(string name, string downloadUrl, int retryCount, int totalRetryCount, string errorMessage)
        {
            Name=name;
            DownloadUrl=downloadUrl;
            RetryCount=retryCount;
            TotalRetryCount=totalRetryCount;
            ErrorMessage=errorMessage;
        }
        public string Name{
            get;
            private set;
        }
        public string DownloadUrl{
            get;
            private set;
        }
        public int RetryCount{
            get;
            private set;
        }
        public int TotalRetryCount{
            get;
            private set;
        }
        public string ErrorMessage{
            get;
            private set;
        }
    }
}
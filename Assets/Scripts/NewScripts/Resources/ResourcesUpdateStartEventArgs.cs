namespace PJW.Resources
{
    /// <summary>
    /// 资源更新开始事件
    /// </summary>
    public class ResourcesUpdateStartEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新开始事件的新实例。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="savePath">资源下载后存放路径。</param>
        /// <param name="downloadUrl">资源下载地址。</param>
        /// <param name="currentLength">当前下载大小。</param>
        /// <param name="zipLength">压缩包大小。</param>
        /// <param name="retryCount">已重试下载次数。</param>
        public ResourcesUpdateStartEventArgs(string name, string savePath, string downloadUrl, int currentLength, int zipLength, int retryCount)
        {
            Name=name;
            SavePath=savePath;
            DownloadUrl=downloadUrl;
            CurrentLength=currentLength;
            ZipLength=zipLength;
            RetryCount=retryCount;
        }
        public string Name{
            get;
            private set;
        }
        public string SavePath{
            get;
            private set;
        }
        public string DownloadUrl{
            get;
            private set;
        }
        public int CurrentLength{
            get;
            private set;
        }
        public int ZipLength{
            get;
            private set;
        }
        public int RetryCount{
            get;
            private set;
        }
    }
}
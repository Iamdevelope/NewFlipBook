namespace PJW.Resources
{
    /// <summary>
    /// 资源更新成功事件
    /// </summary>
    public class ResourcesUpdateSuccessEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新成功事件的新实例。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="savePath">资源下载后存放路径。</param>
        /// <param name="downloadUrl">资源下载地址。</param>
        /// <param name="length">资源大小。</param>
        /// <param name="zipLength">压缩包大小。</param>
        public ResourcesUpdateSuccessEventArgs(string name, string savePath, string downloadUrl, int length, int zipLength)
        {
            Name=name;
            SavePath=savePath;
            DownloadUrl=downloadUrl;
            Length=length;
            ZipLength=zipLength;
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
        public int Length{
            get;
            private set;
        }
        public int ZipLength{
            get;
            private set;
        }
    }
}
namespace PJW.Resources
{
    /// <summary>
    /// 资源更新发生改变事件
    /// </summary>
    public class ResourcesUpdateChangeEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新改变事件的新实例。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="savedPath">资源下载后存放路径。</param>
        /// <param name="downloadUrl">资源下载地址。</param>
        /// <param name="currentLength">当前下载大小。</param>
        /// <param name="zipLength">压缩包大小。</param>
        public ResourcesUpdateChangeEventArgs(string name,string savedPath,string downloadUrl,int currentLength,int zipLength){
            Name=name;
            SavePath=savedPath;
            DownloadUrl=downloadUrl;
            CurrentLength=currentLength;
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
        public int CurrentLength{
            get;
            private set;
        }
        public int ZipLength{
            get;
            private set;
        }
    }
}
namespace PJW.Download
{
    /// <summary>
    /// 下载更新事件
    /// </summary>
    public class DownloadUpdateEventAvgs : FrameworkEventAvgs
    {
        /// <summary>
        /// 下载资源的序列号
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }
        /// <summary>
        /// 资源下载的保存本地地址
        /// </summary>
        public string DownloadPath
        {
            get;
            private set;
        }
        /// <summary>
        /// 资源的url地址
        /// </summary>
        public string DownloadUrl
        {
            get;
            private set;
        }
        /// <summary>
        /// 当前资源下载长度
        /// </summary>
        public int CurrentLength
        {
            get;
            private set;
        }
        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
        /// <summary>
        /// 下载更新事件实例
        /// </summary>
        /// <param name="SerialId"></param>
        /// <param name="DownloadPath"></param>
        /// <param name="DownloadUrl"></param>
        /// <param name="CurrentLength"></param>
        /// <param name="Userdata"></param>
        public DownloadUpdateEventAvgs(int SerialId, string DownloadPath, string DownloadUrl, int CurrentLength, object Userdata)
        {
            this.SerialId = SerialId;
            this.DownloadPath = DownloadPath;
            this.DownloadUrl = DownloadUrl;
            this.CurrentLength = CurrentLength;
            this.UserData = Userdata;
        }
    }
}
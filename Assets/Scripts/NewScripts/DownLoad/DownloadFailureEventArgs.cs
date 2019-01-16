
namespace PJW.Download
{
    /// <summary>
    /// 下载失败事件
    /// </summary>
    public class DownloadFailureEventArgs : FrameworkEventArgs
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
        /// 用户自定义数据
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
        public string ErrorMessage{
            get;
            private set;
        }
        /// <summary>
        /// 下载失败事件实例
        /// </summary>
        /// <param name="SerialId"></param>
        /// <param name="DownloadPath"></param>
        /// <param name="DownloadUrl"></param>
        /// <param name="errorMessage"></param>
        /// <param name="Userdata"></param>
        public DownloadFailureEventArgs(int SerialId, string DownloadPath, string DownloadUrl, string errorMessage, object Userdata)
        {
            this.SerialId = SerialId;
            this.DownloadPath = DownloadPath;
            this.DownloadUrl = DownloadUrl;
            ErrorMessage=errorMessage;
            this.UserData = Userdata;
        }
    }
}
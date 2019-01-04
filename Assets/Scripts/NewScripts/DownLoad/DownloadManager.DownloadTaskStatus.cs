
namespace PJW.Download
{
    internal partial class DownloadManager
    {
        /// <summary>
        /// 下载任务状态枚举
        /// </summary>
        public enum DownloadTaskStatus
        {
            /// <summary>
            /// 即将下载
            /// </summary>
            Todo,
            /// <summary>
            /// 正在下载
            /// </summary>
            Doing,
            /// <summary>
            /// 下载完成
            /// </summary>
            Done,
            /// <summary>
            /// 下载出错
            /// </summary>
            Error
        }
    }
}

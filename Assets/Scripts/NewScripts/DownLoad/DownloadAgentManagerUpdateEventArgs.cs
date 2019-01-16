
namespace PJW.Download
{
    /// <summary>
    /// 下载代理器更新事件
    /// </summary>
    public class DownloadAgentManagerUpdateEventArgs : FrameworkEventArgs
    {
        private readonly byte[] Bytes;
        /// <summary>
        /// 管理器下载更新构造函数
        /// </summary>
        /// <param name="length"></param>
        /// <param name="Bytes"></param>
        public DownloadAgentManagerUpdateEventArgs(int length, byte[] Bytes)
        {
            Length = length;
            this.Bytes = Bytes;
        }
        /// <summary>
        /// 下载的数据长度
        /// </summary>
        public int Length
        {
            get;
            private set;
        }
        public byte[] GetBytes()
        {
            return Bytes;
        }
    }
}
namespace PJW.Resources
{
    /// <summary>
    /// 加载资源代理辅助器异步读取二进制流完成事件
    /// </summary>
    public class LoadResourcesAgentHelperReadBytesCompleteEventArgs:FrameworkEventArgs
    {
        private readonly byte[] _Bytes;

        /// <summary>
        /// 加载资源代理辅助器异步读取二进制流完成事件构造函数
        /// </summary>
        /// <param name="bytes">二进制流</param>
        /// <param name="loadType">加载类型</param>
        public LoadResourcesAgentHelperReadBytesCompleteEventArgs(byte[] bytes,LoadType loadType){
            _Bytes=bytes;
            LoadType=loadType;
        }
        public LoadType LoadType{
            get;
            private set;
        }
        public byte[] GetBytes(){
            return _Bytes;
        }
    }
}
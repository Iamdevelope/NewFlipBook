namespace PJW.Resources
{
    /// <summary>
    /// 加载资源代理辅助器异步解析二进制完成事件
    /// </summary>
    public class LoadResourcesAgentHelperParseBytesCompleteEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 加载资源代理辅助器异步解析二进制完成事件构造函数
        /// </summary>
        /// <param name="resource">资源</param>
        public LoadResourcesAgentHelperParseBytesCompleteEventArgs(object resource){
            Resource=resource;
        }
        public object Resource{
            get;
            private set;
        }
    }
}
namespace PJW.Resources
{
    /// <summary>
    /// 加载资源代理辅助器异步加载完成事件
    /// </summary>
    public class LoadResourcesAgentHelperLoadCompleteEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 加载资源代理辅助器异步加载完成事件构造函数
        /// </summary>
        /// <param name="asset">资源</param>
        public LoadResourcesAgentHelperLoadCompleteEventArgs(object asset){
            Asset=asset;
        }
        public object Asset{
            get;
            private set;
        }
    }
}
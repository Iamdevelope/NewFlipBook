namespace PJW.Resources
{
    /// <summary>
    /// 加载资源代理器异步加载资源更新事件
    /// </summary>
    public sealed class LoadResourcesAgentHelperUpdateEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 加载资源代理器异步加载资源更新事件构造函数
        /// </summary>
        /// <param name="loadResourcesProgressType">加载资源进度类型</param>
        /// <param name="progress">加载进度</param>
        public LoadResourcesAgentHelperUpdateEventArgs(LoadResourcesProgressType loadResourcesProgressType,float progress){
            LoadResourcesProgressType=loadResourcesProgressType;
            Progress=progress;
        }
        public LoadResourcesProgressType LoadResourcesProgressType{
            get;
            private set;
        }
        public float Progress{
            get;
            private set;
        }
    }
}
namespace PJW.Resources
{
    /// <summary>
    /// 加载资源代理辅助器错误事件
    /// </summary>
    public class LoadResourcesAgentHelperErrorEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 加载资源代理辅助器错误事件构造函数
        /// </summary>
        /// <param name="loadResourceStatus">加载资源类型</param>
        /// <param name="errorMessage">错误信息</param>
        public LoadResourcesAgentHelperErrorEventArgs(LoadResourceStatus loadResourceStatus,string errorMessage){
            LoadResourceStatus=loadResourceStatus;
            ErrorMessage=errorMessage;
        }
        public LoadResourceStatus LoadResourceStatus{
            get;
            private set;
        }
        public string ErrorMessage{
            get;
            private set;
        }
    }
}
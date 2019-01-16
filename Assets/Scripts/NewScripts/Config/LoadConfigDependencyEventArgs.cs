namespace PJW.Config
{
    /// <summary>
    /// 加载配置时的加载依赖事件
    /// </summary>
    public class LoadConfigDependencyEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 加载配置时加载依赖构造函数
        /// </summary>
        /// <param name="configAssetName">配置名</param>
        /// <param name="configDependencyName">配置依赖名</param>
        /// <param name="loadedCount">已加载的依赖个数</param>
        /// <param name="totalCount">总共依赖个数</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadConfigDependencyEventArgs(string configAssetName,string configDependencyName,
        int loadedCount,int totalCount,object userData){
            ConfigAssetName=configAssetName;
            ConfigDependencyName=configDependencyName;
            LoadedCount=loadedCount;
            TotalCount=totalCount;
            UserData=userData;
        }
        /// <summary>
        /// 配置资源名
        /// </summary>
        /// <value></value>
        public string ConfigAssetName{
            get;
            private set;
        }
        /// <summary>
        /// 配置依赖名
        /// </summary>
        /// <value></value>
        public string ConfigDependencyName{
            get;
            private set;
        }
        /// <summary>
        /// 已加载个数
        /// </summary>
        /// <value></value>
        public int LoadedCount{
            get;
            private set;
        }
        /// <summary>
        /// 总共加载依赖资源数量
        /// </summary>
        /// <value></value>
        public int TotalCount{
            get;
            private set;
        }
        /// <summary>
        /// 用户自定义数据
        /// </summary>
        /// <value></value>
        public object UserData{
            get;
            private set;
        }
    }
}
namespace PJW.Config
{
    /// <summary>
    /// 加载配置更新事件
    /// </summary>
    public class LoadConfigUpdateEventArgs:FrameworkEventArgs
    {
        /// <summary>
        /// 配置名
        /// </summary>
        /// <value></value>
        public string ConfigName{
            get;
            private set;
        }
        /// <summary>
        /// 更新进度
        /// </summary>
        /// <value></value>
        public float Progress{
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

        /// <summary>
        /// 加载配置更新事件构造函数
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="progress">更新进度</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadConfigUpdateEventArgs(string configName,float progress,object userData){
            ConfigName=configName;
            Progress=progress;
            UserData=userData;
        }
    }
}
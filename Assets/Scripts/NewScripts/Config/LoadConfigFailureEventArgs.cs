namespace PJW.Config
{
    /// <summary>
    /// 加载失败事件
    /// </summary>
    public class LoadConfigFailureEventArgs:FrameworkEventArgs
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
        /// 错误原因
        /// </summary>
        /// <value></value>
        public string ErrorMessage{
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
        /// 加载配置失败事件构造函数
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="errorMessage">错误原因</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadConfigFailureEventArgs(string configName,string errorMessage,object userData){
            ConfigName=configName;
            ErrorMessage=errorMessage;
            UserData=userData;
        }
    }
}
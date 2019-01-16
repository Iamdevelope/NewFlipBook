namespace PJW.Config
{
    /// <summary>
    /// 加载配置成功事件
    /// </summary>
    public class LoadConfigSuccessEventArgs:FrameworkEventArgs
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
        /// 持续时长
        /// </summary>
        /// <value></value>
        public float Duration{
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
        /// <param name="duration">加载持续时长</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadConfigSuccessEventArgs(string configName,float duration,object userData){
            ConfigName=configName;
            Duration=duration;
            UserData=userData;
        }
    }
}
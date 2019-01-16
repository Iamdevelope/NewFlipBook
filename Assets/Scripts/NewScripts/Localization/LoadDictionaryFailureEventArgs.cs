namespace PJW.Localization
{
    /// <summary>
    /// 加载字典失败事件
    /// </summary>
    public class LoadDictionaryFailureEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 字典加载失败构造函数
        /// </summary>
        /// <param name="dictionaryName">字典名</param>
        /// <param name="failureMsg">失败信息</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadDictionaryFailureEventArgs(string dictionaryName,string failureMsg,object userData)
        {
            DictionaryName = dictionaryName;
            FailureMessage = failureMsg;
            UserData = userData;
        }
        public string FailureMessage
        {
            get;
            private set;
        }
        public string DictionaryName
        {
            get;
            private set;
        }
        public object UserData
        {
            get;
            private set;
        }
    }
}
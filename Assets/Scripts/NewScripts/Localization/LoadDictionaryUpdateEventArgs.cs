
namespace PJW.Localization
{
    /// <summary>
    /// 加载字典更新事件
    /// </summary>
    public class LoadDictionaryUpdateEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 字典加载更新构造函数
        /// </summary>
        /// <param name="dictionaryName">字典名</param>
        /// <param name="progress">更新进度</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadDictionaryUpdateEventArgs(string dictionaryName,float progress,object userData)
        {
            DictionaryName = dictionaryName;
            Progress = progress;
            UserData = userData;
        }

        public float Progress
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
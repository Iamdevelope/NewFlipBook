
namespace PJW.Localization
{
    /// <summary>
    /// 加载字典成功事件
    /// </summary>
    public class LoadDictionarySuccessEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 加载成功事件构造函数
        /// </summary>
        /// <param name="dictionaryName">字典名</param>
        /// <param name="duration">加载持续时长</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadDictionarySuccessEventArgs(string dictionaryName,float duration,object userData)
        {
            DictionaryName = dictionaryName;
            Duration = duration;
            UserData = userData;
        }

        public float Duration
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

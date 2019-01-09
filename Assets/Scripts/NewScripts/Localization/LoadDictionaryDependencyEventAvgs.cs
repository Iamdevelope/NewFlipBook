
namespace PJW.Localization
{
    /// <summary>
    /// 加载字典依赖事件
    /// </summary>
    public class LoadDictionaryDependencyEventAvgs : FrameworkEventAvgs
    {
        /// <summary>
        /// 加载依赖事件
        /// </summary>
        /// <param name="dictionaryName">字典名</param>
        /// <param name="dependencyAssetName">依赖资源名</param>
        /// <param name="totalCount">总加载依赖资源数量</param>
        /// <param name="loadedCount">当前已经加载依赖资源数量</param>
        /// <param name="userData">用户自定义数据</param>
        public LoadDictionaryDependencyEventAvgs(string dictionaryName,string dependencyAssetName,int totalCount,int loadedCount,object userData)
        {
            DictionaryName = dictionaryName;
            DependencyAssetName = dependencyAssetName;
            TotalCount = totalCount;
            LoadedCount = loadedCount;
            UserData = userData;
        }
        public string DependencyAssetName
        {
            get;
            private set;
        }
        public int TotalCount
        {
            get;
            private set;
        }
        public int LoadedCount
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
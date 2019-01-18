namespace PJW.Scene
{
    /// <summary>
    /// 加载场景依赖资源事件
    /// </summary>
    public sealed class LoadSceneDependencyAssetEventArgs : FrameworkEventArgs
    {
        public LoadSceneDependencyAssetEventArgs(string sceneName,string dependencyAssetName,int loadedDependencyAssetCounts,int totalDependencyAssetCounts,object userData)
        {
            SceneName=sceneName;
            DependencyAssetName=dependencyAssetName;
            LoadedDependencyAssetCounts=loadedDependencyAssetCounts;
            TotalDependencyAssetCounts=totalDependencyAssetCounts;
            UserData=userData;
        }
        public string SceneName{
            get;
            private set;
        }
        public string DependencyAssetName{
            get;
            private set;
        }
        public int LoadedDependencyAssetCounts{
            get;
            private set;
        }
        public int TotalDependencyAssetCounts{
            get;
            private set;
        }
        public object UserData{
            get;
            private set;
        }
    }
}
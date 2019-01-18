namespace PJW.Scene
{
    /// <summary>
    /// 加载场景更新事件
    /// </summary>
    public sealed class LoadSceneUpdateEventArgs : FrameworkEventArgs
    {
        public LoadSceneUpdateEventArgs(string sceneName,float progress,object userData)
        {
            SceneName=sceneName;
            Progress=progress;
            UserData=userData;
        }
        public string SceneName{
            get;
            private set;
        }
        public float Progress{
            get;
            private set;
        }
        public object UserData{
            get;
            private set;
        }
    }
}
namespace PJW.Scene
{
    /// <summary>
    /// 下载场景失败事件
    /// </summary>
    public sealed class UnloadSceneFailureEventArgs : FrameworkEventArgs
    {
        public UnloadSceneFailureEventArgs(string sceneName,object userData)
        {
            SceneName=sceneName;
            UserData=userData;
        }
        public string SceneName{
            get;
            private set;
        }
        public object UserData{
            get;
            private set;
        }
    }
}
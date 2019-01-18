namespace PJW.Scene
{
    /// <summary>
    /// 加载场景失败事件
    /// </summary>
    public sealed class LoadSceneFailureEventArgs : FrameworkEventArgs
    {
        public LoadSceneFailureEventArgs(string sceneName,string errorMessage,object userData)
        {
            SceneName=sceneName;
            ErrorMessage=errorMessage;
            UserData=userData;
        }
        public string SceneName{
            get;
            private set;
        }
        public string ErrorMessage{
            get;
            private set;
        }
        public object UserData{
            get;
            private set;
        }
    }
}
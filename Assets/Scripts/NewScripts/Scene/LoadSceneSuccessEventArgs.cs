namespace PJW.Scene
{
    /// <summary>
    /// 加载场景成功事件
    /// </summary>
    public sealed class LoadSceneSuccessEventArgs : FrameworkEventArgs
    {
        public LoadSceneSuccessEventArgs(string sceneName,float duration,object userData)
        {
            SceneName=sceneName;
            Duration=duration;
            UserData=userData;
        }
        public string SceneName{
            get;
            private set;
        }
        public float Duration{
            get;
            private set;
        }
        public object UserData{
            get;
            private set;
        }
    }
}
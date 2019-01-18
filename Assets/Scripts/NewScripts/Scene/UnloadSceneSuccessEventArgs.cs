namespace PJW.Scene
{
    /// <summary>
    /// 卸载场景成功事件
    /// </summary>
    public sealed class UnloadSceneSuccessEventArgs : FrameworkEventArgs
    {
        public UnloadSceneSuccessEventArgs(string sceneName,object userData){
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
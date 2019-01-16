
namespace PJW.Resources
{

    /// <summary>
    /// 卸载场景失败委托
    /// </summary>
    /// <param name="sceneName">要卸载的场景名</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void UnloadSceneFailureCallback(string sceneName, object userData);

    /// <summary>
    /// 卸载场景失败委托
    /// </summary>
    /// <param name="sceneName">要卸载的场景名</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void UnloadSceneSuccessCallback(string sceneName, object userData);
    
    public class UnloadSceneCallbacks
    {
        private readonly UnloadSceneFailureCallback unloadSceneFailureCallback;
        private readonly UnloadSceneSuccessCallback unloadSceneSuccessCallback;

        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback) : this(unloadSceneSuccessCallback,null)
        {

        }
        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback,UnloadSceneFailureCallback unloadSceneFailureCallback)
        {
            if (unloadSceneSuccessCallback == null)
            {
                throw new FrameworkException(" Unload Scene Success callback is invalid ");
            }
            this.unloadSceneSuccessCallback = unloadSceneSuccessCallback;
            this.unloadSceneFailureCallback = unloadSceneFailureCallback;
        }
        public UnloadSceneFailureCallback GetUnloadSceneFailureCallback
        {
            get { return unloadSceneFailureCallback; }
        }
        public UnloadSceneSuccessCallback GetUnloadSceneSuccessCallback
        {
            get { return unloadSceneSuccessCallback; }
        }
    }
}
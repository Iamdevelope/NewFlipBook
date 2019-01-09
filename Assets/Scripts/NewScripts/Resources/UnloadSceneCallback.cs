
namespace PJW.Resources
{
    public class UnloadSceneCallback
    {
        private readonly UnloadSceneFailureCallback unloadSceneFailureCallback;
        private readonly UnloadSceneSuccessCallback unloadSceneSuccessCallback;

        public UnloadSceneCallback(UnloadSceneSuccessCallback unloadSceneSuccessCallback) : this(unloadSceneSuccessCallback,null)
        {

        }
        public UnloadSceneCallback(UnloadSceneSuccessCallback unloadSceneSuccessCallback,UnloadSceneFailureCallback unloadSceneFailureCallback)
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
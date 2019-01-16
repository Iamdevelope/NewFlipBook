namespace PJW.Resources
{
    /// <summary>
    /// 加载场景资源时的依赖回调
    /// </summary>
    /// <param name="sceneAssetName">场景资源名</param>
    /// <param name="dependencyName">依赖资源名</param>
    /// <param name="loadedCount">当前已加载依赖资源数量</param>
    /// <param name="totalCount">总依赖资源数量</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadSceneDependencyCallback(string sceneAssetName,string dependencyName,int loadedCount,int totalCount,object userData);
    
    /// <summary>
    /// 加载场景资源错误回调
    /// </summary>
    /// <param name="sceneAssetName">场景资源名</param>
    /// <param name="loadResourceStatus">资源状态</param>
    /// <param name="errorMessage">错误原因</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadSceneFailureCallback(string sceneAssetName,LoadResourceStatus loadResourceStatus,string errorMessage,object userData);

    /// <summary>
    /// 加载场景成功回调
    /// </summary>
    /// <param name="sceneAssetName">场景资源名</param>
    /// <param name="duration">加载所有时长</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadSceneSuccessCallback(string sceneAssetName,float duration,object userData);

    /// <summary>
    /// 加载场景资源更新回调
    /// </summary>
    /// <param name="sceneAssetName">场景资源名</param>
    /// <param name="progress">加载进度</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadSceneUpdateCallback(string sceneAssetName,float progress,object userData);
    
    /// <summary>
    /// 加载场景资源回调函数集
    /// </summary>
    public class LoadSceneCallbacks
    {
        private readonly LoadSceneDependencyCallback _LoadSceneDependencyCallback;
        private readonly LoadSceneFailureCallback _LoadSceneFailureCallback;
        private readonly LoadSceneSuccessCallback _LoadSceneSuccessCallback;
        private readonly LoadSceneUpdateCallback _LoadSceneUpdateCallback;

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        /// <param name="loadSceneDependencyCallback">加载场景依赖回调</param>
        /// <param name="loadSceneFailureCallback">加载场景失败回调</param>
        /// <param name="loadSceneUpdateCallback">加载场景更新回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback,LoadSceneDependencyCallback loadSceneDependencyCallback,
        LoadSceneFailureCallback loadSceneFailureCallback,LoadSceneUpdateCallback loadSceneUpdateCallback){
            if(loadSceneSuccessCallback==null){
                throw new FrameworkException(" the load scene callback is invalid ");
            }
            _LoadSceneSuccessCallback=loadSceneSuccessCallback;
            _LoadSceneDependencyCallback=loadSceneDependencyCallback;
            _LoadSceneFailureCallback=loadSceneFailureCallback;
            _LoadSceneUpdateCallback=loadSceneUpdateCallback;
        }

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback)
        :this(loadSceneSuccessCallback,null,null,null){

        }

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        /// <param name="loadSceneDependencyCallback">加载场景依赖回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback,LoadSceneDependencyCallback loadSceneDependencyCallback)
        :this(loadSceneSuccessCallback,loadSceneDependencyCallback,null,null){

        }

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        /// <param name="loadSceneFailureCallback">加载场景失败回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback, LoadSceneFailureCallback loadSceneFailureCallback)
        :this(loadSceneSuccessCallback,null,loadSceneFailureCallback,null){

        }

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        /// <param name="loadSceneUpdateCallback">加载场景更新回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback,LoadSceneUpdateCallback loadSceneUpdateCallback)
        :this(loadSceneSuccessCallback,null,null,loadSceneUpdateCallback){

        }

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        /// <param name="loadSceneDependencyCallback">加载场景依赖回调</param>
        /// <param name="loadSceneFailureCallback">加载场景失败回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback,LoadSceneDependencyCallback loadSceneDependencyCallback, LoadSceneFailureCallback loadSceneFailureCallback)
        :this(loadSceneSuccessCallback,loadSceneDependencyCallback,loadSceneFailureCallback,null){

        }

        /// <summary>
        /// 加载场景资源回调函数实例
        /// </summary>
        /// <param name="loadSceneSuccessCallback">加载场景成功回调</param>
        /// <param name="loadSceneDependencyCallback">加载场景依赖回调</param>
        /// <param name="loadSceneUpdateCallback">加载场景更新回调</param>
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSceneSuccessCallback,LoadSceneDependencyCallback loadSceneDependencyCallback,LoadSceneUpdateCallback loadSceneUpdateCallback)
        :this(loadSceneSuccessCallback,loadSceneDependencyCallback,null,loadSceneUpdateCallback){

        }
        
        public LoadSceneDependencyCallback GetLoadSceneDependencyCallback{
            get{return _LoadSceneDependencyCallback;}
        }
        public LoadSceneFailureCallback GetLoadSceneFailureCallback{
            get{return _LoadSceneFailureCallback;}
        }
        public LoadSceneSuccessCallback GetLoadSceneSuccessCallback{
            get{return _LoadSceneSuccessCallback;}
        }
        public LoadSceneUpdateCallback GetLoadSceneUpdateCallback{
            get{return _LoadSceneUpdateCallback;}
        }
    }
}
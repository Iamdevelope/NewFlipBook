namespace PJW.Resources
{

    /// <summary>
    /// 加载资源时加载依赖资源回调函数。
    /// </summary>
    /// <param name="assetName">要加载的资源名称。</param>
    /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
    /// <param name="loadedCount">当前已加载依赖资源数量。</param>
    /// <param name="totalCount">总共加载依赖资源数量。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadAssetDependencyCallback(string assetName, string dependencyName, int loadedCount,int totalCount,object userData);

    /// <summary>
    /// 加载失败委托
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="loadResourceStatus">资源状态</param>
    /// <param name="errorMessage">错误原因</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadAssetFailureCallback(string assetName,LoadResourceStatus loadResourceStatus,string errorMessage,object userData);

    /// <summary>
    /// 加载资源成功委托
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="loadedAsset">已加载资源</param>
    /// <param name="duration">加载所用时长</param>
    /// <param name="userData">用户自定义</param>
    public delegate void LoadAssetSuccessCallback(string assetName,object loadedAsset,float duration,object userData);

    /// <summary>
    /// 加载资源更新委托
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="progress">更新进度</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadAssetUpdateCallback(string assetName,float progress,object userData);

    /// <summary>
    /// 加载资源回调函数集
    /// </summary>
    public sealed class LoadAssetCallbacks
    {
        private readonly LoadAssetDependencyCallback _LoadAssetDependencyCallback;
        private readonly LoadAssetFailureCallback _LoadAssetFailureCallback;
        private readonly LoadAssetSuccessCallback _LoadAssetSuccessCallback;
        private readonly LoadAssetUpdateCallback _LoadAssetUpdateCallback;


        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback)
        :this(loadAssetSuccessCallback,null,null,null){

        }

        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        /// <param name="loadAssetDependencyCallback">加载资源依赖回调</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,LoadAssetDependencyCallback loadAssetDependencyCallback)
        :this(loadAssetSuccessCallback,loadAssetDependencyCallback,null,null){

        }

        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        /// <param name="loadAssetFailureCallback">加载资源失败回调</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,LoadAssetFailureCallback loadAssetFailureCallback)
        :this(loadAssetSuccessCallback,null,loadAssetFailureCallback,null){

        }

        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        /// <param name="loadAssetUpdateCallback">加载资源更新回调</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,LoadAssetUpdateCallback loadAssetUpdateCallback)
        :this(loadAssetSuccessCallback,null,null,loadAssetUpdateCallback){
            
        }

        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        /// <param name="loadAssetDependencyCallback">加载资源依赖回调</param>
        /// <param name="loadAssetFailureCallback">加载资源失败回调</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,LoadAssetDependencyCallback loadAssetDependencyCallback,LoadAssetFailureCallback loadAssetFailureCallback)
        :this(loadAssetSuccessCallback,loadAssetDependencyCallback,loadAssetFailureCallback,null){

        }

        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        /// <param name="loadAssetDependencyCallback">加载资源依赖回调</param>
        /// <param name="loadAssetUpdateCallback">加载资源更新回调</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,LoadAssetDependencyCallback loadAssetDependencyCallback,LoadAssetUpdateCallback loadAssetUpdateCallback)
        :this(loadAssetSuccessCallback,loadAssetDependencyCallback,null,loadAssetUpdateCallback){

        }

        /// <summary>
        /// 加载资源回调函数集构造函数
        /// </summary>
        /// <param name="loadAssetSuccessCallback">加载资源成功回调,必须传</param>
        /// <param name="loadAssetDependencyCallback">加载资源依赖回调</param>
        /// <param name="loadAssetFailureCallback">加载资源失败回调</param>
        /// <param name="loadAssetUpdateCallback">加载资源更新回调</param>
        public LoadAssetCallbacks(LoadAssetSuccessCallback loadAssetSuccessCallback,LoadAssetDependencyCallback loadAssetDependencyCallback,LoadAssetFailureCallback loadAssetFailureCallback,
        LoadAssetUpdateCallback loadAssetUpdateCallback){
            if(loadAssetSuccessCallback==null){
                throw new FrameworkException(" Load asset success callback is invalid ");
            }
            _LoadAssetDependencyCallback=loadAssetDependencyCallback;
            _LoadAssetFailureCallback=loadAssetFailureCallback;
            _LoadAssetSuccessCallback=loadAssetSuccessCallback;
            _LoadAssetUpdateCallback=loadAssetUpdateCallback;
        }
        
        public LoadAssetDependencyCallback GetLoadAssetDependencyCallback{
            get{return _LoadAssetDependencyCallback;}
        }
        public LoadAssetFailureCallback GetLoadAssetFailureCallback{
            get{return _LoadAssetFailureCallback;}
        }
        public LoadAssetSuccessCallback GetLoadAssetSuccessCallback{
            get{return _LoadAssetSuccessCallback;}
        }
        public LoadAssetUpdateCallback GetLoadAssetUpdateCallback{
            get{return _LoadAssetUpdateCallback;}
        }
    }
}
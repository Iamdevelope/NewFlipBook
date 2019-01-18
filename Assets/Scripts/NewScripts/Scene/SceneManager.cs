using System;
using System.Collections.Generic;
using PJW.Resources;

namespace PJW.Scene
{
    /// <summary>
    /// 加载场景管理器
    /// </summary>
    internal sealed class SceneManager : FrameworkModule, ISceneManager
    {
        private readonly List<string> _LoadedSceneAssetNames;
        private readonly List<string> _LoadingSceneAssetNames;
        private readonly List<string> _UnloadingSceneAssetNames;
        private readonly LoadSceneCallbacks _LoadSceneCallbacks;
        private readonly UnloadSceneCallbacks _UnloadSceneCallbacks;
        private IResourcesManager _ResourcesManager;
        private EventHandler<LoadSceneDependencyAssetEventArgs> _LoadSceneDependencyAssetHandler;
        private EventHandler<LoadSceneFailureEventArgs> _LoadSceneFailureHandler;
        private EventHandler<LoadSceneSuccessEventArgs> _LoadSceneSuccessHandler;
        private EventHandler<LoadSceneUpdateEventArgs> _LoadSceneUpdateHandler;
        private EventHandler<UnloadSceneFailureEventArgs> _UnloadSceneFailureHandler;
        private EventHandler<UnloadSceneSuccessEventArgs> _UnloadSceneSuccessHandler;

        /// <summary>
        /// 场景管理器实例
        /// </summary>
        public SceneManager()
        {
            _LoadedSceneAssetNames = new List<string>();
            _LoadingSceneAssetNames = new List<string>();
            _UnloadingSceneAssetNames = new List<string>();

            _LoadSceneCallbacks=new LoadSceneCallbacks(OnLoadSceneSuccessCallback,OnLoadSceneDependencyCallback,OnLoadSceneFailureCallback,OnLoadSceneUpdateCallback);
            _UnloadSceneCallbacks=new UnloadSceneCallbacks(OnUnloadSceneSuccessCallback,OnUnloadSceneFailureCallback);
            _ResourcesManager=null;

            _LoadSceneDependencyAssetHandler=null;
            _LoadSceneFailureHandler=null;
            _LoadSceneSuccessHandler=null;
            _LoadSceneUpdateHandler=null;
            _UnloadSceneFailureHandler=null;
            _UnloadSceneSuccessHandler=null;
        }

        /// <summary>
        /// 场景管理器优先级
        /// </summary>
        /// <value></value>
        public override int Priority
        {
            get
            {
                return 60;
            }
        }

        /// <summary>
        /// 加载场景依赖资源事件
        /// </summary>
        public event EventHandler<LoadSceneDependencyAssetEventArgs> LoadSceneDependencyAsset
        {
            add
            {
                _LoadSceneDependencyAssetHandler+=value;
            }
            remove
            {
                _LoadSceneDependencyAssetHandler-=value;
            }
        }
        
        /// <summary>
        /// 加载场景失败事件
        /// </summary>
        public event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure
        {
            add
            {
                _LoadSceneFailureHandler+=value;
            }
            remove
            {
                _LoadSceneFailureHandler-=value;
            }
        }
        
        /// <summary>
        /// 加载场景成功事件
        /// </summary>
        public event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess
        {
            add
            {
                _LoadSceneSuccessHandler+=value;
            }
            remove
            {
                _LoadSceneSuccessHandler-=value;
            }
        }
        
        /// <summary>
        /// 加载场景更新事件
        /// </summary>
        public event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate
        {
            add
            {
                _LoadSceneUpdateHandler+=value;
            }
            remove
            {
                _LoadSceneUpdateHandler-=value;
            }
        }
        
        /// <summary>
        /// 卸载场景失败事件
        /// </summary>
        public event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure
        {
            add
            {
                _UnloadSceneFailureHandler+=value;
            }
            remove
            {
                _UnloadSceneFailureHandler-=value;
            }
        }
        
        /// <summary>
        /// 卸载场景成功事件
        /// </summary>
        public event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess
        {
            add
            {
                _UnloadSceneSuccessHandler+=value;
            }
            remove
            {
                _UnloadSceneSuccessHandler-=value;
            }
        }

        /// <summary>
        /// 获取已经加载了场景的资源名
        /// </summary>
        /// <returns>已经加载了场景的资源名</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return _LoadedSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取已经加载了场景的资源名
        /// </summary>
        /// <param name="assetNames">已经加载了场景的资源名</param>
        public void GetLoadedSceneAssetNames(List<string> assetNames)
        {
            if(assetNames==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            assetNames.Clear();
            assetNames.AddRange(_LoadedSceneAssetNames);
        }

        /// <summary>
        /// 获取正在加载的场景中的所有资源名
        /// </summary>
        /// <returns>场景中的所有资源名</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return _LoadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在加载的场景中的所有资源名
        /// </summary>
        /// <param name="assetNames">场景中的所有资源名</param>
        public void GetLoadingSceneAssetNames(List<string> assetNames)
        {
            if(assetNames==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            assetNames.Clear();
            assetNames.AddRange(_LoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取正在卸载场景的所有资源名
        /// </summary>
        /// <returns>已经卸载了场景的所有资源名</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return _UnloadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在卸载场景的所有资源名
        /// </summary>
        /// <param name="assetNames">已经卸载了场景的所有资源名</param>
        public void GetUnloadingSceneAssetNames(List<string> assetNames)
        {
            if(assetNames==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            assetNames.Clear();
            assetNames.AddRange(_UnloadingSceneAssetNames);
        }

        /// <summary>
        /// 场景是否已经加载
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <returns>场景是否已经加载</returns>
        public bool IsLoadedScene(string sceneName)
        {
            if(sceneName==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            return _LoadedSceneAssetNames.Contains(sceneName);
        }

        /// <summary>
        /// 获取场景是否正在加载
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <returns>场景是否已经加载</returns>
        public bool IsLoadingScene(string sceneName)
        {
            if(sceneName==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            return _LoadingSceneAssetNames.Contains(sceneName);
        }

        /// <summary>
        /// 判断场景是否正在卸载
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <returns>场景是否已经卸载</returns>
        public bool IsUnloadingScene(string sceneName)
        {
            if(sceneName==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            return _UnloadingSceneAssetNames.Contains(sceneName);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadScene(string sceneName, int priority, object userData)
        {
            if(sceneName==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            if(_ResourcesManager==null){
                throw new FrameworkException(" You must set resources manager first ");
            }
            if(!IsLoadedScene(sceneName)){
                throw new FrameworkException(Utility.Text.Format("Scene {0} not loaded ",sceneName));
            }
            if(IsLoadingScene(sceneName)){
                throw new FrameworkException(Utility.Text.Format("Scene {0} is loading ",sceneName));
            }
            if(IsUnloadingScene(sceneName)){
                throw new FrameworkException(Utility.Text.Format("Scene {0} is unloading "));
            }
            _LoadingSceneAssetNames.Add(sceneName);
            _ResourcesManager.LoadScene(sceneName,priority,userData,_LoadSceneCallbacks);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="priority">优先级</param>
        public void LoadScene(string sceneName, int priority)
        {
            LoadScene(sceneName,priority,null);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadScene(string sceneName, object userData)
        {
            LoadScene(sceneName,Constant.DefaultPriority,userData);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName,Constant.DefaultPriority,null);
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourcesManager">资源管理器</param>
        public void SetResourcesManager(IResourcesManager resourcesManager)
        {
            if(resourcesManager==null){
                throw new FrameworkException("The resources manager is invalid ");
            }
            _ResourcesManager=resourcesManager;
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="userData">用户自定义数据</param>
        public void UnloadScene(string sceneName, object userData)
        {
            if(sceneName==null){
                throw new FrameworkException("Scene name is invalid ");
            }
            if(_ResourcesManager==null){
                throw new FrameworkException(" You must set resources manager first ");
            }
            if(!IsLoadedScene(sceneName)){
                throw new FrameworkException(Utility.Text.Format("Scene {0} not loaded ",sceneName));
            }
            if(IsLoadingScene(sceneName)){
                throw new FrameworkException(Utility.Text.Format("Scene {0} is loading ",sceneName));
            }
            if(IsUnloadingScene(sceneName)){
                throw new FrameworkException(Utility.Text.Format("Scene {0} is unloading "));
            }
            _UnloadingSceneAssetNames.Add(sceneName);
            _ResourcesManager.UnloadScene(sceneName,_UnloadSceneCallbacks,userData);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void UnloadScene(string sceneName)
        {
            UnloadScene(sceneName,null);
        }

        /// <summary>
        /// 清理并关闭场景管理器
        /// </summary>
        public override void Shutdown()
        {
            foreach (string item in _LoadedSceneAssetNames)
            {
                if(IsUnloadingScene(item)){
                    continue;
                }
                UnloadScene(item);
            }
            _LoadedSceneAssetNames.Clear();
            _LoadingSceneAssetNames.Clear();
            _UnloadingSceneAssetNames.Clear();
        }

        /// <summary>
        /// 场景管理器轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        /// <summary>
        /// 卸载场景成功
        /// </summary>
        /// <param name="sceneName">要卸载的场景名</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnUnloadSceneSuccessCallback(string sceneName, object userData)
        {
            _UnloadingSceneAssetNames.Remove(sceneName);
            _LoadedSceneAssetNames.Remove(sceneName);
            if(_UnloadSceneSuccessHandler!=null){
                _UnloadSceneSuccessHandler(this,new UnloadSceneSuccessEventArgs(sceneName,userData));
            }
        }

        /// <summary>
        /// 卸载场景失败
        /// </summary>
        /// <param name="sceneName">要卸载的场景名</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnUnloadSceneFailureCallback(string sceneName, object userData)
        {
            _UnloadingSceneAssetNames.Remove(sceneName);
            if(_UnloadSceneFailureHandler!=null){
                _UnloadSceneFailureHandler(this,new UnloadSceneFailureEventArgs(sceneName,userData));
                return;
            }
            throw new FrameworkException(Utility.Text.Format("Unload scene failure, scene asset name {0} ",sceneName));
        }
        
        /// <summary>
        /// 加载场景资源错误回调
        /// </summary>
        /// <param name="sceneAssetName">场景资源名</param>
        /// <param name="loadResourceStatus">资源状态</param>
        /// <param name="errorMessage">错误原因</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnLoadSceneFailureCallback(string sceneAssetName, LoadResourceStatus loadResourceStatus, string errorMessage, object userData)
        {
            _LoadingSceneAssetNames.Remove(sceneAssetName);
            string error=Utility.Text.Format("Load scene failure, scene asset name {0}, load resourcesStatus {1}, error {2} ",sceneAssetName,loadResourceStatus.ToString(),errorMessage);
            if(_LoadSceneFailureHandler!=null){
                _LoadSceneFailureHandler(this,new LoadSceneFailureEventArgs(sceneAssetName,error,userData));
                return;
            }
            throw new FrameworkException(error);
        }

        /// <summary>
        /// 加载场景资源时的依赖回调
        /// </summary>
        /// <param name="sceneAssetName">场景资源名</param>
        /// <param name="dependencyName">依赖资源名</param>
        /// <param name="loadedCount">当前已加载依赖资源数量</param>
        /// <param name="totalCount">总依赖资源数量</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnLoadSceneDependencyCallback(string sceneAssetName, string dependencyName, int loadedCount, int totalCount, object userData)
        {
            if(_LoadSceneDependencyAssetHandler!=null){
                _LoadSceneDependencyAssetHandler(this,new LoadSceneDependencyAssetEventArgs(sceneAssetName,dependencyName,loadedCount,totalCount,userData));
            }
        }

        /// <summary>
        /// 加载场景成功回调
        /// </summary>
        /// <param name="sceneAssetName">场景资源名</param>
        /// <param name="duration">加载所有时长</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnLoadSceneSuccessCallback(string sceneAssetName, float duration, object userData)
        {
            _LoadedSceneAssetNames.Add(sceneAssetName);
            _LoadingSceneAssetNames.Remove(sceneAssetName);
            if(_LoadSceneSuccessHandler!=null){
                _LoadSceneSuccessHandler(this,new LoadSceneSuccessEventArgs(sceneAssetName,duration,userData));
            }
        }

        /// <summary>
        /// 加载场景资源更新回调
        /// </summary>
        /// <param name="sceneAssetName">场景资源名</param>
        /// <param name="progress">加载进度</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnLoadSceneUpdateCallback(string sceneAssetName, float progress, object userData)
        {
            if(_LoadSceneUpdateHandler!=null){
                _LoadSceneUpdateHandler(this,new LoadSceneUpdateEventArgs(sceneAssetName,progress,userData));
            }
        }
    }
}
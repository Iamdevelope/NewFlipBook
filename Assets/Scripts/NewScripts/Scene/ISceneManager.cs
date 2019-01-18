using System;
using System.Collections.Generic;
using PJW.Resources;

namespace PJW.Scene
{
    /// <summary>
    /// 加载场景管理器接口
    /// </summary>
    public interface ISceneManager
    {
        
        /// <summary>
        /// 加载场景依赖资源事件
        /// </summary>
        event EventHandler<LoadSceneDependencyAssetEventArgs> LoadSceneDependencyAsset;
        
        /// <summary>
        /// 加载场景失败事件
        /// </summary>
        event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure;
        
        /// <summary>
        /// 加载场景成功事件
        /// </summary>
        event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess;
        
        /// <summary>
        /// 加载场景更新事件
        /// </summary>
        event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate;

        /// <summary>
        /// 卸载场景失败事件
        /// </summary>
        event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure;

        /// <summary>
        /// 卸载场景成功事件
        /// </summary>
        event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess;

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="resourcesManager">资源管理器</param>
        void SetResourcesManager(IResourcesManager resourcesManager);

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">用户自定义数据</param>
        void LoadScene(string sceneName,int priority,object userData);
        
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="priority">优先级</param>
        void LoadScene(string sceneName,int priority);
        
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="userData">用户自定义数据</param>
        void LoadScene(string sceneName,object userData);
        
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        void LoadScene(string sceneName);
        
        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="userData">用户自定义数据</param>
        void UnloadScene(string sceneName,object userData);
        
        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        void UnloadScene(string sceneName);

        /// <summary>
        /// 场景是否已经加载
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <returns>场景是否已经加载</returns>
        bool IsLoadedScene(string sceneName);

        /// <summary>
        /// 获取已经加载了场景的资源名
        /// </summary>
        /// <returns>已经加载了场景的资源名</returns>
        string[] GetLoadedSceneAssetNames();

        /// <summary>
        /// 获取已经加载了场景的资源名
        /// </summary>
        /// <param name="assetNames">已经加载了场景的资源名</param>
        void GetLoadedSceneAssetNames(List<string> assetNames);

        /// <summary>
        /// 获取场景是否正在加载
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <returns>场景是否已经加载</returns>
        bool IsLoadingScene(string sceneName);
        
        /// <summary>
        /// 获取正在加载的场景中的所有资源名
        /// </summary>
        /// <returns>场景中的所有资源名</returns>
        string[] GetLoadingSceneAssetNames();

        /// <summary>
        /// 获取正在加载的场景中的所有资源名
        /// </summary>
        /// <param name="assetNames">场景中的所有资源名</param>
        void GetLoadingSceneAssetNames(List<string> assetNames);

        /// <summary>
        /// 判断场景是否正在卸载
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <returns>场景是否已经卸载</returns>
        bool IsUnloadingScene(string sceneName);
        
        /// <summary>
        /// 获取正在卸载场景的所有资源名
        /// </summary>
        /// <returns>已经卸载了场景的所有资源名</returns>
        string[] GetUnloadingSceneAssetNames();

        /// <summary>
        /// 获取正在卸载场景的所有资源名
        /// </summary>
        /// <param name="assetNames">已经卸载了场景的所有资源名</param>
        void GetUnloadingSceneAssetNames(List<string> assetNames);
    }
}
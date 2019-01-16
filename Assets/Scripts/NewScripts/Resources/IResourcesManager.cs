using System;
using PJW.ObjectPool;
using PJW.Download;

namespace PJW.Resources
{
    /// <summary>
    /// 资源管理器接口
    /// </summary>
    public interface IResourcesManager
    {
        /// <summary>
        /// 获取资源只读路径
        /// </summary>
        /// <value></value>
        string GetReadOnlyPath{get;}

        /// <summary>
        /// 获取资源读写路径
        /// </summary>
        /// <value></value>
        string GetReadWritePath{get;}

        /// <summary>
        /// 获取资源模式
        /// </summary>
        /// <value></value>
        ResourcesMode GetResourcesMode{get;}

        /// <summary>
        /// 获取当前变体
        /// </summary>
        /// <value></value>
        string GetCurrentVariant{get;}

        /// <summary>
        /// 获取当前资源使用的游戏版本号
        /// </summary>
        /// <value></value>
        string GetApplicationGameVersion{get;}

        /// <summary>
        /// 获取当前内部资源版本号
        /// </summary>
        /// <value></value>
        int GetInternalResourcesVersion{get;}
        
        /// <summary>
        /// 获取已准备完毕资源数量
        /// </summary>
        /// <value></value>
        int GetAssetCount{get;}

        /// <summary>
        /// 获取已准备完毕资源数量
        /// </summary>
        /// <value></value>
        int GetResourcesCount{get;}

        /// <summary>
        /// 获取资源组数量
        /// </summary>
        /// <value></value>
        int GetResourcesGroupCount{get;}

        /// <summary>
        /// 资源更新下载地址
        /// </summary>
        /// <value></value>
        string UpdatePrefixUrl{get;set;}

        /// <summary>
        /// 资源更新重试次数
        /// </summary>
        /// <value></value>
        int UpdateRetryCount{get;set;}

        /// <summary>
        /// 获取正在更新资源数量
        /// </summary>
        /// <value></value>
        int GetIsUpdatingCount{get;}

        /// <summary>
        /// 获取正在等待更新资源数量
        /// </summary>
        /// <value></value>
        int GetUpdateWatingCount{get;}

        /// <summary>
        /// 获取资源加载代理总数量
        /// </summary>
        /// <value></value>
        int GetLoadAgentTotalCount{get;}

        /// <summary>
        /// 获取空闲资源代理数量
        /// </summary>
        /// <value></value>
        int GetFreeAgentCount{get;}

        /// <summary>
        /// 获取正在工作中的资源代理数量
        /// </summary>
        /// <value></value>
        int GetWorkingAgentCount{get;}

        /// <summary>
        /// 获取正在等待加载资源的任务数量
        /// </summary>
        /// <value></value>
        int GetLoadWaitingTaskCount{get;}

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔时间
        /// </summary>
        /// <value></value>
        float AssetAutoReleaseInterval{get;set;}
        
        /// <summary>
        /// 资源对象池容量
        /// </summary>
        /// <value></value>
        int AssetCapacity{get;set;}

        /// <summary>
        /// 资源对象池对象过期时间
        /// </summary>
        /// <value></value>
        float AssetExpireTime{get;set;}

        /// <summary>
        /// 资源对象池优先级
        /// </summary>
        /// <value></value>
        int AssetPriority{get;set;}
        
        /// <summary>
        /// 资源更新发生改变事件
        /// </summary>
        event EventHandler<ResourcesUpdateChangeEventArgs> ResourcesUpdateChangeEventArgs;
        
        /// <summary>
        /// 资源更新失败事件
        /// </summary>
        event EventHandler<ResourcesUpdateFailureEventArgs> ResourcesUpdateFailureEventArgs;
        
        /// <summary>
        /// 资源更新开始事件
        /// </summary>
        event EventHandler<ResourcesUpdateStartEventArgs> ResourcesUpdateStartEventArgs;
        
        /// <summary>
        /// 资源更新成功事件
        /// </summary>
        event EventHandler<ResourcesUpdateSuccessEventArgs> ResourcesUpdateSuccessEventArgs;
        
        /// <summary>
        /// 设置资源只读路径
        /// </summary>
        /// <param name="readOnlyPath">资源只读路径</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置资源读写路径
        /// </summary>
        /// <param name="readWritePath">资源读写路径</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 设置资源模式
        /// </summary>
        /// <param name="resourcesMode">资源模式</param>
        void SetResourcesMode(ResourcesMode resourcesMode);

        /// <summary>
        /// 设置当前变体
        /// </summary>
        /// <param name="currentVariant">需要设置的当前变体</param>
        void SetCurretVariant(string currentVariant);

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager"></param>
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置下载管理器
        /// </summary>
        /// <param name="downloadManager"></param>
        void SetDownloadManager(IDownLoadManager downloadManager);

        /// <summary>
        /// 设置解密资源回调函数
        /// </summary>
        /// <param name="decryptResourcesCallback">要设置的解密资源回调函数</param>
        /// <remarks>如果不设置，则使用默认的解密资源回调函数</remarks>
        void SetDecryptResourcesCallback(DecryptResourcesCallback decryptResourcesCallback);

        /// <summary>
        /// 设置资源辅助器
        /// </summary>
        /// <param name="resourcesHelper">资源辅助器</param>
        void SetResourcesHelper(IResourcesHelper resourcesHelper);

        /// <summary>
        /// 增加加载资源代理辅助器
        /// </summary>
        /// <param name="loadResourcesAgentHelper">要增加的资源代理辅助器</param>
        void AddResourcesAgentHelper(ILoadResourcesAgentHelper loadResourcesAgentHelper);

        /// <summary>
        /// 使用单机模式并初始化资源
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成回调函数</param>
        void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback);

        /// <summary>
        /// 检查可更新模式并检查版本资源列表
        /// </summary>
        /// <param name="lastestInternalResourcesVersion">最新的内部资源版本号</param>
        /// <returns>检查版本资源列表结果</returns>
        CheckVersionListResult CheckVersionList(int lastestInternalResourcesVersion);

        /// <summary>
        /// 使用可更新模式并更新版本资源列表
        /// </summary>
        /// <param name="versionListLength">资源列表大小</param>
        /// <param name="versionListHashCode">资源列表哈希值</param>
        /// <param name="versionListZipLength">资源列表压缩后大小</param>
        /// <param name="versionListZipHashCode">资源列表压缩后哈希值</param>
        /// <param name="updateVersionListCallbacks">版本资源列表更新回调函数集</param>
        void UpdateVersionList(int versionListLength,int versionListHashCode,int versionListZipLength,int versionListZipHashCode,UpdateVersionListCallbacks updateVersionListCallbacks);

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成回调函数</param>
        void CheckResources(CheckResourcesCompleteCallback checkResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并更新资源
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成回调</param>
        void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback);
        
        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <returns>资源是否存在</returns>
        bool IsExistAsset(string assetName);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,Type assetType,LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="priority">加载资源的优先级</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,int priority,LoadAssetCallbacks loadAssetCallbacks);
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,object userData,LoadAssetCallbacks loadAssetCallbacks);
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="priority">加载资源的优先级</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,Type assetType,int priority,LoadAssetCallbacks loadAssetCallbacks);
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,Type assetType,object userData,LoadAssetCallbacks loadAssetCallbacks);
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,int priority,object userData,LoadAssetCallbacks loadAssetCallbacks);
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="priority">加载资源的优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(string assetName,Type assetType,int priority,object userData,LoadAssetCallbacks loadAssetCallbacks);
        
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">需要卸载的资源</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">加载的场景名</param>
        /// <param name="priority">加载场景的优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        void LoadScene(string sceneName,int priority,object userData,LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">加载的场景名</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        void LoadScene(string sceneName,object userData,LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">加载的场景名</param>
        /// <param name="priority">加载场景的优先级</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        void LoadScene(string sceneName,int priority,LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">加载的场景名</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        void LoadScene(string sceneName,LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步卸载场景资源
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        void UnloadScene(string sceneName,UnloadSceneCallbacks unloadSceneCallbacks,object userData);

        /// <summary>
        /// 异步卸载场景资源
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        void UnloadScene(string sceneName,UnloadSceneCallbacks unloadSceneCallbacks);
        
        /// <summary>
        /// 获取资源组资源数量
        /// </summary>
        /// <param name="resourcesGroupName">资源组名</param>
        /// <returns></returns>
        int GetResourcesGroupResourceCount(string resourcesGroupName);

        /// <summary>
        /// 获取资源组是否准备好
        /// </summary>
        /// <param name="resourcesGroupName">资源组名</param>
        /// <returns></returns>
        bool GetResourcesGroupReady(string resourcesGroupName);

        /// <summary>
        /// 获取资源组已准备完成资源数量
        /// </summary>
        /// <param name="resourcesGroupName">资源组名</param>
        /// <returns></returns>
        int GetResourcesGroupReadyResourceCount(string resourcesGroupName);

        /// <summary>
        /// 获取资源组总大小
        /// </summary>
        /// <param name="resourcesGroupName">资源组名</param>
        /// <returns></returns>
        int GetResourcesGroupTotalCount(string resourcesGroupName);

        /// <summary>
        /// 获取资源组已准备完成总大小
        /// </summary>
        /// <param name="resourcesGroupName">资源组名</param>
        /// <returns></returns>
        int GetResourcesGroupTotalReadyCount(string resourcesGroupName);

        /// <summary>
        /// 获取资源组进度
        /// </summary>
        /// <param name="resourcesGroupName">资源组名</param>
        /// <returns></returns>
        float GetResourcesGroupProgress(string resourcesGroupName);
    }
}
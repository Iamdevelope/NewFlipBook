using System;
using System.Collections.Generic;
using PJW.ObjectPool;
using PJW.Task;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        private partial class ResourcesLoader
        {
            /// <summary>
            /// 加载资源代理
            /// </summary>
            private sealed partial class LoadResourcesAgent : ITaskAgent<LoadResourcesTaskBase>
            {
                private static readonly HashSet<string> _LoadingAssetNames=new HashSet<string>();
                private static readonly HashSet<string> _LoadingResourcesNames=new HashSet<string>();
                private readonly ILoadResourcesAgentHelper _Helper;
                private readonly IResourcesHelper _ResourcesHelper;
                private readonly IObjectPool<AssetObject> _AssetPool;
                private readonly IObjectPool<ResourcesObject> _ResourcesPool;
                private readonly ResourcesLoader _ResourcesLoader;
                private readonly string _ReadOnlyPath;
                private readonly string _ReadWritePath;
                private readonly DecryptResourcesCallback _DecryptResourcesCallback;
                private readonly LinkedList<string> _LoadingDependencyAssetNames;
                private LoadResourcesTaskBase _Task;
                private WaitingType _WaitingType;
                private bool _IsLoadingAsset;
                private bool _IsLoadingResources;

                /// <summary>
                /// 加载资源代理
                /// </summary>
                /// <param name="loadResourceAgentHelper">加载资源代理辅助器</param>
                /// <param name="resourceHelper">资源辅助器</param>
                /// <param name="assetPool">资源对象池</param>
                /// <param name="resourcePool">资源对象池</param>
                /// <param name="resourceLoader">资源加载器</param>
                /// <param name="readOnlyPath">只读路径</param>
                /// <param name="readWritePath">读写路径</param>
                /// <param name="decryptResourceCallback">解密资源回调</param>
                public LoadResourcesAgent(ILoadResourcesAgentHelper loadResourceAgentHelper, IResourcesHelper resourceHelper, IObjectPool<AssetObject> assetPool, IObjectPool<ResourcesObject> resourcePool, ResourcesLoader resourceLoader, string readOnlyPath, string readWritePath, DecryptResourcesCallback decryptResourceCallback)
                {
                    if (loadResourceAgentHelper == null)
                    {
                        throw new FrameworkException("Load resource agent helper is invalid.");
                    }

                    if (resourceHelper == null)
                    {
                        throw new FrameworkException("Resource helper is invalid.");
                    }

                    if (assetPool == null)
                    {
                        throw new FrameworkException("Asset pool is invalid.");
                    }

                    if (resourcePool == null)
                    {
                        throw new FrameworkException("Resource pool is invalid.");
                    }

                    if (resourceLoader == null)
                    {
                        throw new FrameworkException("Resource loader is invalid.");
                    }

                    if (decryptResourceCallback == null)
                    {
                        throw new FrameworkException("Decrypt resource callback is invalid.");
                    }
                    _Helper=loadResourceAgentHelper;
                    _ResourcesHelper=resourceHelper;
                    _AssetPool=assetPool;
                    _ResourcesPool=resourcePool;
                    _ResourcesLoader=resourceLoader;
                    _ReadOnlyPath=readOnlyPath;
                    _ReadWritePath=readWritePath;
                    _DecryptResourcesCallback=decryptResourceCallback;
                    _LoadingDependencyAssetNames=new LinkedList<string>();
                    _Task=null;
                    _WaitingType=WaitingType.None;
                    _IsLoadingAsset=false;
                    _IsLoadingResources=false;
                }

                /// <summary>
                /// 获取加载资源代理辅助器
                /// </summary>
                /// <value></value>
                public ILoadResourcesAgentHelper GetHelper{
                    get{
                        return _Helper;
                    }
                }

                /// <summary>
                /// 获取加载资源任务
                /// </summary>
                /// <value></value>
                public LoadResourcesTaskBase GetTask
                {
                    get
                    {
                        return _Task;
                    }
                }

                /// <summary>
                /// 初始化加载资源代理
                /// </summary>
                public void Init()
                {
                    _Helper.LoadResourcesAgentHelperErrorEventArgs+=OnLoadResourcesAgentHelperError;
                    _Helper.LoadResourcesAgentHelperLoadCompleteEventArgs+=OnLoadResourcesAgentHelperLoadComplete;
                    _Helper.LoadResourcesAgentHelperParseBytesCompleteEventArgs+=OnLoadResourcesAgentHelperParseBytesComplete;
                    _Helper.LoadResourcesAgentHelperReadBytesCompleteEventArgs+=OnLoadResourcesAgentHelperReadBytesComplete;
                    _Helper.LoadResourcesAgentHelperReadFileCompleteEventArgs+=OnLoadResourcesAgentHelperReadFileComplete;
                    _Helper.LoadResourcesAgentHelperUpdateEventArgs+=OnLoadResourcesAgentHelperUpdate;
                }

                /// <summary>
                /// 重置加载资源代理
                /// </summary>
                public void Reset()
                {
                    _Helper.Reset();
                    _LoadingDependencyAssetNames.Clear();
                    _Task=null;
                    _IsLoadingAsset=false;
                    _IsLoadingResources=false;
                    _WaitingType=WaitingType.None;
                }

                /// <summary>
                /// 关闭并清理加载资源代理
                /// </summary>
                public void Shutdown()
                {
                    Reset();
                    _Helper.LoadResourcesAgentHelperErrorEventArgs-=OnLoadResourcesAgentHelperError;
                    _Helper.LoadResourcesAgentHelperLoadCompleteEventArgs-=OnLoadResourcesAgentHelperLoadComplete;
                    _Helper.LoadResourcesAgentHelperParseBytesCompleteEventArgs-=OnLoadResourcesAgentHelperParseBytesComplete;
                    _Helper.LoadResourcesAgentHelperReadBytesCompleteEventArgs-=OnLoadResourcesAgentHelperReadBytesComplete;
                    _Helper.LoadResourcesAgentHelperReadFileCompleteEventArgs-=OnLoadResourcesAgentHelperReadFileComplete;
                    _Helper.LoadResourcesAgentHelperUpdateEventArgs-=OnLoadResourcesAgentHelperUpdate;
                }

                /// <summary>
                /// 开始处理加载资源任务
                /// </summary>
                /// <param name="task"></param>
                public void Start(LoadResourcesTaskBase task)
                {
                    if(task==null){
                        throw new FrameworkException(" Task is invalid ");
                    }
                    _Task=task;
                    _Task.StartTime=DateTime.Now;
                    if(IsAssetLoading(_Task.GetAssetName)){
                        _WaitingType=WaitingType.WaitForAsset;
                        return;
                    }
                    TryLoadAsset();
                }

                /// <summary>
                /// 加载资源代理轮询
                /// </summary>
                /// <param name="elapseSeconds"></param>
                /// <param name="realElapseSeconds"></param>
                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    if(_WaitingType==WaitingType.None)
                    {
                        return;
                    }
                    if(_WaitingType==WaitingType.WaitForAsset)
                    {
                        if(IsAssetLoading(_Task.GetAssetName))
                        {
                            return;
                        }
                        _WaitingType=WaitingType.None;
                        AssetObject assetObject=_AssetPool.Spawn(_Task.GetAssetName);
                        if(assetObject==null)
                        {
                            TryLoadAsset();
                            return;
                        }
                        OnAssetObjectReady(assetObject);
                        return;
                    }
                    if(_WaitingType==WaitingType.WaitForDependency)
                    {
                        LinkedListNode<string> current=_LoadingDependencyAssetNames.First;
                        while(current!=null){
                            if(!IsAssetLoading(current.Value)){
                                LinkedListNode<string> next=current.Next;
                                if(!_AssetPool.CanSpawn(current.Value)){
                                    OnError(LoadResourceStatus.DependencyAssetError,Utility.Text.Format("Can not find dependency asset object name {0} ",current.Value));
                                    return;
                                }
                                _LoadingDependencyAssetNames.Remove(current);
                                current=next;
                                continue;
                            }
                            current=current.Next;
                        }
                        if(_LoadingDependencyAssetNames.Count>0){
                            return;
                        }
                        _WaitingType=WaitingType.None;
                        OnDependencyAssetReady();
                        return;
                    }
                    if(_WaitingType==WaitingType.WaitForResources)
                    {
                        if(IsResourcesLoading(_Task.GetResourcesInfo.GetResourcesName.GetName)){
                            return;
                        }
                        ResourcesObject resourcesObject=_ResourcesPool.Spawn(_Task.GetResourcesInfo.GetResourcesName.GetName);
                        if(resourcesObject==null){
                            OnError(LoadResourceStatus.DependencyAssetError,Utility.Text.Format("Can not find resource object {0} ",_Task.GetResourcesInfo.GetResourcesName.GetName));
                            return;
                        }
                        _WaitingType=WaitingType.None;
                        OnResourcesReady(resourcesObject);
                        return;
                    }
                }

                /// <summary>
                /// 尝试加载资源
                /// </summary>
                private void TryLoadAsset()
                {
                    if(!_Task.GetIsScene){
                        AssetObject assetObject=_AssetPool.Spawn(_Task.GetAssetName);
                        if(assetObject!=null){
                            OnAssetObjectReady(assetObject);
                            return;
                        }
                    }
                    _IsLoadingAsset=true;
                    _LoadingAssetNames.Add(_Task.GetAssetName);
                    foreach (string item in _Task.GetDependencyAssetNames())
                    {
                        if(!_AssetPool.CanSpawn(item)){
                            if(!IsAssetLoading(item)){
                                OnError(LoadResourceStatus.DependencyAssetError,Utility.Text.Format("Can not find dependency asset object named {0} ",item));
                                return;
                            }
                            _LoadingDependencyAssetNames.AddLast(item);
                        }
                    }
                    if(_LoadingDependencyAssetNames.Count>0){
                        _WaitingType=WaitingType.WaitForDependency;
                        return;
                    }
                    OnDependencyAssetReady();
                }

                /// <summary>
                /// 判断是否是asset资源正在等待加载
                /// </summary>
                /// <param name="getAssetName"></param>
                /// <returns></returns>
                private bool IsAssetLoading(string getAssetName)
                {
                    return _LoadingAssetNames.Contains(getAssetName);
                }
                
                /// <summary>
                /// 判断是否是Resource资源正在等待加载
                /// </summary>
                /// <param name="resourcesName"></param>
                /// <returns></returns>
                private bool IsResourcesLoading(string resourcesName)
                {
                    return _LoadingResourcesNames.Contains(resourcesName);
                }

                private void OnAssetObjectReady(AssetObject assetObject)
                {
                    _Helper.Reset();
                    object asset=assetObject.GetTarget;
                    if(_Task.GetIsScene){
                        _ResourcesLoader._SceneToAssetMap.Add(_Task.GetAssetName,asset);
                    }
                    _Task.OnLoadAssetSuccess(this,asset,(float)(DateTime.Now-_Task.StartTime).TotalSeconds);
                    _Task.Done=true;
                }

                private void OnDependencyAssetReady()
                {
                    if(IsResourcesLoading(_Task.GetResourcesInfo.GetResourcesName.GetName)){
                        _WaitingType=WaitingType.WaitForDependency;
                        return;
                    }
                    ResourcesObject resourcesObject=_ResourcesPool.Spawn(_Task.GetResourcesInfo.GetResourcesName.GetName);
                    if(resourcesObject!=null){
                        OnResourcesReady(resourcesObject);
                        return;
                    }
                    _IsLoadingResources=true;
                    _LoadingResourcesNames.Remove(_Task.GetResourcesInfo.GetResourcesName.GetName);
                    string fullPath=Utility.Path.GetCombinePath(_Task.GetResourcesInfo.GetStorageInReadOnly?_ReadOnlyPath:_ReadWritePath,Utility.Path.GetResourceNameWithSuffix(_Task.GetResourcesInfo.GetResourcesName.FullName));
                    if(_Task.GetResourcesInfo.GetLoadType==LoadType.LoadFromFile){
                        _Helper.ReadFile(fullPath);
                    }
                    else{
                        _Helper.ReadBytes(fullPath,_Task.GetResourcesInfo.GetLoadType);
                    }
                }

                private void OnResourcesReady(ResourcesObject resourcesObject){
                    _Task.LoadMain(this,resourcesObject);
                }

                /// <summary>
                /// 加载资源代理器异步加载资源更新
                /// </summary>
                /// <param name="sender">事件源</param>
                /// <param name="e">加载资源代理器异步加载资源更新事件</param>
                private void OnLoadResourcesAgentHelperUpdate(object sender, LoadResourcesAgentHelperUpdateEventArgs e)
                {
                    _Task.OnLoadAssetUpdate(this,e.LoadResourcesProgressType,e.Progress);
                }

                /// <summary>
                /// 加载资源代理辅助器异步读取文件完成
                /// </summary>
                /// <param name="sender">事件源</param>
                /// <param name="e">加载资源代理辅助器异步读取文件完成事件</param>
                private void OnLoadResourcesAgentHelperReadFileComplete(object sender, LoadResourcesAgentHelperReadFileCompleteEventArgs e)
                {
                    ResourcesObject resourcesObject=new ResourcesObject(_Task.GetResourcesInfo.GetResourcesName.GetName,e.Resource,_ResourcesHelper,_ResourcesLoader._ResourcesDependencyCount);
                    _ResourcesPool.CreateObject(resourcesObject,true);
                    _IsLoadingResources=false;
                    _LoadingResourcesNames.Remove(_Task.GetResourcesInfo.GetResourcesName.GetName);
                    OnResourcesReady(resourcesObject);
                }

                /// <summary>
                /// 加载资源代理辅助器异步读取二进制流完成
                /// </summary>
                /// <param name="sender">事件源</param>
                /// <param name="e">加载资源代理辅助器异步读取二进制流完成事件</param>
                private void OnLoadResourcesAgentHelperReadBytesComplete(object sender, LoadResourcesAgentHelperReadBytesCompleteEventArgs e)
                {
                    byte[] bytes=e.GetBytes();
                    LoadType loadType=(LoadType)e.LoadType;
                    if(loadType==LoadType.LoadFromMemoryAndQuickDecrypt||loadType==LoadType.LoadFromMemoryAndDecrypt)
                    {
                        bytes=_DecryptResourcesCallback(_Task.GetResourcesInfo.GetResourcesName.GetName,_Task.GetResourcesInfo.GetResourcesName.GetVariant,e.LoadType,_Task.GetResourcesInfo.GetLength,_Task.GetResourcesInfo.GetHashCode,_Task.GetResourcesInfo.GetStorageInReadOnly,bytes);
                    }
                    _Helper.ParseBytes(bytes);
                }

                /// <summary>
                /// 加载资源代理辅助器异步解析二进制完成
                /// </summary>
                /// <param name="sender">事件源</param>
                /// <param name="e">加载资源代理辅助器异步解析二进制完成事件</param>
                private void OnLoadResourcesAgentHelperParseBytesComplete(object sender, LoadResourcesAgentHelperParseBytesCompleteEventArgs e)
                {
                    ResourcesObject resourcesObject=new ResourcesObject(_Task.GetResourcesInfo.GetResourcesName.GetName,e.Resource,_ResourcesHelper,_ResourcesLoader._ResourcesDependencyCount);
                    _ResourcesPool.CreateObject(resourcesObject,true);
                    _IsLoadingResources=false;
                    _LoadingResourcesNames.Remove(_Task.GetResourcesInfo.GetResourcesName.GetName);
                    OnResourcesReady(resourcesObject);
                }

                /// <summary>
                /// 加载资源代理辅助器异步加载完成
                /// </summary>
                /// <param name="sender">事件源</param>
                /// <param name="e">加载资源代理辅助器异步加载完成事件</param>
                private void OnLoadResourcesAgentHelperLoadComplete(object sender, LoadResourcesAgentHelperLoadCompleteEventArgs e)
                {
                    AssetObject assetObject=null;
                    if(_Task.GetIsScene)
                    {
                        assetObject=_AssetPool.Spawn(_Task.GetAssetName);
                    }
                    if(assetObject==null)
                    {
                        assetObject=new AssetObject(_Task.GetAssetName,e.Asset,_Task.GetDependencyAssets(),_Task.GetResourcesObject.GetTarget,_AssetPool,_ResourcesPool,_ResourcesHelper,_ResourcesLoader._AssetDependencyCount);
                        _AssetPool.CreateObject(assetObject,true);
                        foreach (object item in _Task.GetDependencyResources())
                        {
                            _Task.GetResourcesObject.AddDependencyResources(item);
                        }
                    }
                    _IsLoadingAsset=false;
                    _LoadingAssetNames.Remove(_Task.GetAssetName);
                    OnAssetObjectReady(assetObject);
                }

                /// <summary>
                /// 加载资源代理辅助器错误
                /// </summary>
                /// <param name="sender">事件源</param>
                /// <param name="e">加载资源代理辅助器错误事件</param>
                private void OnLoadResourcesAgentHelperError(object sender, LoadResourcesAgentHelperErrorEventArgs e)
                {
                    OnError(e.LoadResourceStatus,e.ErrorMessage);
                }

                private void OnError(LoadResourceStatus loadResourceStatus, string errorMessage)
                {
                    _Helper.Reset();
                    _Task.OnLoadAssetFailure(this,loadResourceStatus,errorMessage);
                    if(_IsLoadingAsset){
                        _IsLoadingAsset=false;
                        _LoadingAssetNames.Remove(_Task.GetAssetName);
                    }
                    if(_IsLoadingResources){
                        _IsLoadingResources=false;
                        _LoadingResourcesNames.Remove(_Task.GetResourcesInfo.GetResourcesName.GetName);
                    }
                    _Task.Done=true;
                }
            }
        }
    }
}
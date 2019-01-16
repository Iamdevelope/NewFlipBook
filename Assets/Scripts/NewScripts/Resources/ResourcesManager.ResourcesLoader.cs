using System;
using System.Collections.Generic;
using PJW.ObjectPool;
using PJW.Task;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        /// <summary>
        /// 资源加载器
        /// </summary>
        private sealed partial class ResourcesLoader
        {
            private readonly ResourcesManager _ResourcesManager;
            private readonly TaskPool<LoadResourcesTaskBase> _TaskPool;
            private readonly Dictionary<object, int> _AssetDependencyCount;
            private readonly Dictionary<object, int> _ResourcesDependencyCount;
            private readonly Dictionary<string, object> _SceneToAssetMap;
            private IObjectPool<AssetObject> _AssetPool;
            private IObjectPool<ResourcesObject> _ResourcesPool;

            public ResourcesLoader(ResourcesManager resourcesManager){
                _ResourcesManager=resourcesManager;
                _TaskPool=new TaskPool<LoadResourcesTaskBase>();
                _AssetDependencyCount=new Dictionary<object, int>();
                _ResourcesDependencyCount=new Dictionary<object, int>();
                _SceneToAssetMap=new Dictionary<string, object>();
                _AssetPool=null;
                _ResourcesPool=null;
            }

            /// <summary>
            /// 获取所有加载代理数量
            /// </summary>
            /// <value></value>
            public int GetLoadAgentTotalCount{
                get{
                    return _TaskPool.GetTotalAgentCount;
                }
            }

            /// <summary>
            /// 获取可用加载代理个数
            /// </summary>
            /// <value></value>
            public int GetFreeAgentCount{
                get{
                    return _TaskPool.GetFreeAgentCount;
                }
            }

            /// <summary>
            /// 获取正在使用的加载代理的个数
            /// </summary>
            /// <value></value>
            public int GetWorkingAgentCount{
                get{
                    return _TaskPool.GetWorkingAgentCount;
                }
            }

            /// <summary>
            /// 获取等待执行的任务数量
            /// </summary>
            /// <value></value>
            public int GetLoadWaitingTaskCount{
                get{
                    return _TaskPool.GetWaitingAgentCount;
                }
            }

            /// <summary>
            /// 获取或设置对象池自动释放可释放对象的间隔时间
            /// </summary>
            /// <value></value>
            public float AssetAutoReleaseInterval{
                get{
                    return _AssetPool.AutoReleaseInterval;
                }
                set{
                    _AssetPool.AutoReleaseInterval=value;
                }
            }
            
            /// <summary>
            /// 获取或设置对象池容量
            /// </summary>
            /// <value></value>
            public int AssetCapacity{
                get{
                    return _AssetPool.Capacity;
                }
                set{
                    _AssetPool.Capacity=value;
                }
            }

            /// <summary>
            /// 获取或设置对象池对象过期时间
            /// </summary>
            /// <value></value>
            public float AssetExpireTime{
                get{
                    return _AssetPool.ExpireTime;
                }
                set{
                    _AssetPool.ExpireTime=value;
                }
            }

            /// <summary>
            /// 获取或设置对象池优先级
            /// </summary>
            /// <value></value>
            public int AssetPriority{
                get{
                    return _AssetPool.Priority;
                }
                set{
                    _AssetPool.Priority=value;
                }
            }

            /// <summary>
            /// 获取或设置对象池自动释放可释放对象的间隔时间
            /// </summary>
            /// <value></value>
            public float ResourcesAutoReleaseInterval{
                get{
                    return _ResourcesPool.AutoReleaseInterval;
                }
                set{
                    _ResourcesPool.AutoReleaseInterval=value;
                }
            }
            
            /// <summary>
            /// 获取或设置对象池容量
            /// </summary>
            /// <value></value>
            public int ResourcesCapacity{
                get{
                    return _ResourcesPool.Capacity;
                }
                set{
                    _ResourcesPool.Capacity=value;
                }
            }

            /// <summary>
            /// 获取或设置对象池对象过期时间
            /// </summary>
            /// <value></value>
            public float ResourcesExpireTime{
                get{
                    return _ResourcesPool.ExpireTime;
                }
                set{
                    _ResourcesPool.ExpireTime=value;
                }
            }

            /// <summary>
            /// 获取或设置对象池优先级
            /// </summary>
            /// <value></value>
            public int ResourcesPriority{
                get{
                    return _ResourcesPool.Priority;
                }
                set{
                    _ResourcesPool.Priority=value;
                }
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="assetName">资源名称</param>
            /// <param name="assetType">资源类型</param>
            /// <param name="priority">资源优先级</param>
            /// <param name="userData">用户自定义数据</param>
            /// <param name="loadAssetCallbacks">加载资源回调函数</param>
            public void LoadAsset(string assetName,Type assetType,int priority,object userData,LoadAssetCallbacks loadAssetCallbacks){
                ResourcesInfo? resourcesInfo=null;
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneName">场景名称</param>
            /// <param name="priority">场景优先级</param>
            /// <param name="userData">用户自定义数据</param>
            /// <param name="loadSceneCallbacks">加载场景回调</param>
            public void LoadScene(string sceneName,int priority,object userData,LoadSceneCallbacks loadSceneCallbacks){

            }

            /// <summary>
            /// 卸载资源
            /// </summary>
            /// <param name="asset">需要卸载的资源</param>
            public void UnloadAsset(object asset){
                _AssetPool.Unspawn(asset);
            }

            /// <summary>
            /// 卸载场景
            /// </summary>
            /// <param name="sceneName">需要卸载的场景名</param>
            /// <param name="unloadSceneCallbacks">卸载场景回调</param>
            /// <param name="userData">用户自定义数据</param>
            public void UnloadScene(string sceneName,UnloadSceneCallbacks unloadSceneCallbacks,object userData){

            }

            /// <summary>
            /// 检查资源是否存在
            /// </summary>
            /// <param name="assetName">需要检查的资源名称</param>
            /// <returns>资源是否存在</returns>
            public bool IsExistAsset(string assetName){
                if(string.IsNullOrEmpty(assetName)){
                    return false;
                }
                return _ResourcesManager.GetAssetInfo(assetName).HasValue;
            }

            /// <summary>
            /// 设置对象池管理器
            /// </summary>
            /// <param name="objectPoolManager">对象池管理器</param>
            public void SetObjectPoolManager(IObjectPoolManager objectPoolManager){
                _AssetPool = objectPoolManager.CreateMultiSpawnObjectPool<AssetObject>("Asset Pool");
                _ResourcesPool = objectPoolManager.CreateMultiSpawnObjectPool<ResourcesObject>("Resource Pool");
            }

            /// <summary>
            /// 增加加载资源代理辅助器
            /// </summary>
            /// <param name="loadResourcesAgentHelper">加载资源代理辅助器</param>
            /// <param name="resourcesHelper">资源辅助器</param>
            /// <param name="readOnlyPath">只读路径</param>
            /// <param name="readWritePath">读写路径</param>
            /// <param name="decryptResourcesCallback">解密资源回调</param>
            public void AddLoadResourcesAgentHelper(ILoadResourcesAgentHelper loadResourcesAgentHelper,IResourcesHelper resourcesHelper,string readOnlyPath,string readWritePath,DecryptResourcesCallback decryptResourcesCallback){
                if(_TaskPool==null||_ResourcesPool==null){
                    throw new FrameworkException(" You must set object pool manager first ");
                }
                LoadResourcesAgent agent=new LoadResourcesAgent(loadResourcesAgentHelper,resourcesHelper,_AssetPool,_ResourcesPool,this,readOnlyPath,readWritePath,decryptResourcesCallback);
                _TaskPool.AddAgent(agent);
            }

            /// <summary>
            /// 关闭并清理资源加载器
            /// </summary>
            public void Shutdown(){
                _TaskPool.Shutdown();
                _AssetDependencyCount.Clear();
                _ResourcesDependencyCount.Clear();
                _SceneToAssetMap.Clear();
            }

            /// <summary>
            /// 资源加载器轮询
            /// </summary>
            /// <param name="elapseSeconds"></param>
            /// <param name="realElapseSeconds"></param>
            public void Update(float elapseSeconds, float realElapseSeconds){
                _TaskPool.Update(elapseSeconds,realElapseSeconds);
            }
        }
    }
}
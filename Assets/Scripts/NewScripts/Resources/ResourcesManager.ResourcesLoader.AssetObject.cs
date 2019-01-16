using System.Collections.Generic;
using PJW.ObjectPool;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        private partial class ResourcesLoader
        {
            /// <summary>
            /// 资源对象
            /// </summary>
            private sealed class AssetObject : ObjectBase
            {
                private readonly object[] _DependencyAssets;
                private readonly object _Resources;
                private readonly IObjectPool<AssetObject> _AssetPool;
                private readonly IObjectPool<ResourcesObject> _ResourcesPool;
                private readonly IResourcesHelper _ResourcesHelper;
                private readonly Dictionary<object, int> _AssetDependencyCount;

                
                /// <summary>
                /// 资源对象实例
                /// </summary>
                /// <param name="name">对象名</param>
                /// <param name="target">对象</param>
                /// <param name="dependencyAssets">依赖资源</param>
                /// <param name="resources">资源</param>
                /// <param name="assetPool">asset资源对象池</param>
                /// <param name="resourcesPool">resource资源对象池</param>
                /// <param name="resourcesHelper">资源辅助器</param>
                /// <param name="assetDependencyCount">依赖资源数量</param>
                public AssetObject(string name, object target, object[] dependencyAssets, object resources, 
                IObjectPool<AssetObject> assetPool, IObjectPool<ResourcesObject> resourcesPool, IResourcesHelper resourcesHelper, Dictionary<object, int> assetDependencyCount)
                    : base(name, target)
                {
                    if (dependencyAssets == null)
                    {
                        throw new FrameworkException("Dependency assets is invalid.");
                    }

                    if (resources == null)
                    {
                        throw new FrameworkException("Resource is invalid.");
                    }

                    if (assetPool == null)
                    {
                        throw new FrameworkException("Asset pool is invalid.");
                    }

                    if (resourcesPool == null)
                    {
                        throw new FrameworkException("Resource pool is invalid.");
                    }

                    if (resourcesHelper == null)
                    {
                        throw new FrameworkException("Resource helper is invalid.");
                    }

                    if (assetDependencyCount == null)
                    {
                        throw new FrameworkException("Asset dependency count is invalid.");
                    }

                    _DependencyAssets = dependencyAssets;
                    _Resources = resources;
                    _AssetPool = assetPool;
                    _ResourcesPool = resourcesPool;
                    _ResourcesHelper = resourcesHelper;
                    _AssetDependencyCount = assetDependencyCount;

                    foreach (object dependencyAsset in _DependencyAssets)
                    {
                        int referenceCount = 0;
                        if (_AssetDependencyCount.TryGetValue(dependencyAsset, out referenceCount))
                        {
                            _AssetDependencyCount[dependencyAsset] = referenceCount + 1;
                        }
                        else
                        {
                            _AssetDependencyCount.Add(dependencyAsset, 1);
                        }
                    }
                }

                /// <summary>
                /// 获取自定义释放检查标记
                /// </summary>
                /// <value></value>
                public override bool CustomCanReleaseFlag{
                    get{
                        int targetReferenceCount=0;
                        _AssetDependencyCount.TryGetValue(GetTarget,out targetReferenceCount);
                        return base.CustomCanReleaseFlag&&targetReferenceCount<=0;
                    }
                }

                /// <summary>
                /// 释放资源
                /// </summary>
                protected internal override void UnSpawn(){
                    base.UnSpawn();
                    foreach (object item in _DependencyAssets)
                    {
                        _AssetPool.Unspawn(item);
                    }
                }
                /// <summary>
                /// 释放对象
                /// </summary>
                /// <param name="isShutdown">是否是关闭对象时释放</param>
                protected internal override void Release(bool isShutdown)
                {
                    if(!isShutdown){
                        int targetReferenceCount=0;
                        if(_AssetDependencyCount.TryGetValue(GetTarget,out targetReferenceCount)&&targetReferenceCount>0){
                            throw new FrameworkException(Utility.Text.Format(" Resources object {0} reference count {1} larger than 0 ",GetName,targetReferenceCount));
                        }
                        foreach (object item in _DependencyAssets)
                        {
                            int referenceCount=0;
                            if(_AssetDependencyCount.TryGetValue(GetTarget,out referenceCount)){
                                _AssetDependencyCount[GetTarget]=referenceCount-1;
                            }
                            else{
                                throw new FrameworkException(Utility.Text.Format(" Resources target {0} dependency reference count is invalid "));
                            }
                        }
                        _AssetPool.Unspawn(_Resources);
                    }
                    _AssetDependencyCount.Remove(GetTarget);
                    _ResourcesHelper.Release(GetTarget);
                }
            }
        }
    }
}
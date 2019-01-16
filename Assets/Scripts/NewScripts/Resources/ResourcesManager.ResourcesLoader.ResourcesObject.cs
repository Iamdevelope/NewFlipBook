using PJW.ObjectPool;
using System.Collections.Generic;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        private partial class ResourcesLoader
        {
            /// <summary>
            /// 资源对象
            /// </summary>
            private sealed class ResourcesObject : ObjectBase
            {
                private readonly List<object> _DependencyResources;
                private readonly IResourcesHelper _ResourcesHelper;
                private readonly Dictionary<object,int> _DependencyResourcesCount;
                public ResourcesObject(string name,object target,IResourcesHelper resourcesHelper,Dictionary<object,int> dependencyResourcesCount)
                :base(name,target)
                {
                    if(resourcesHelper==null)
                    {
                        throw new FrameworkException(" Resources helper is invalid ");
                    }
                    if(dependencyResourcesCount==null)
                    {
                        throw new FrameworkException(" Resources dependency count is invalid ");
                    }
                    _DependencyResources=new List<object>();
                    _ResourcesHelper=resourcesHelper;
                    _DependencyResourcesCount=dependencyResourcesCount;
                }

                /// <summary>
                /// 获取自定义释放检查标记
                /// </summary>
                /// <value></value>
                public override bool CustomCanReleaseFlag
                {
                    get
                    {
                        int targetReferenceCount=0;
                        _DependencyResourcesCount.TryGetValue(base.GetTarget,out targetReferenceCount);
                        return base.CustomCanReleaseFlag&&targetReferenceCount<=0;
                    }
                }

                /// <summary>
                /// 新增依赖资源
                /// </summary>
                /// <param name="dependencyResources"></param>
                public void AddDependencyResources(object dependencyResources){
                    if(_DependencyResources.Contains(dependencyResources)){
                        return;
                    }
                    _DependencyResources.Add(dependencyResources);
                    int dependencyResourcesCount=0;
                    if(_DependencyResourcesCount.TryGetValue(base.GetTarget,out dependencyResourcesCount)){
                        _DependencyResourcesCount[base.GetTarget]=dependencyResourcesCount+1;
                    }
                    else{
                        _DependencyResourcesCount.Add(base.GetTarget,1);
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
                        if(_DependencyResourcesCount.TryGetValue(GetTarget,out targetReferenceCount)&&targetReferenceCount>0){
                            throw new FrameworkException(Utility.Text.Format(" Resources object {0} reference count {1} larger than 0 ",GetName,targetReferenceCount));
                        }
                        foreach (object item in _DependencyResources)
                        {
                            int reference=0;
                            if(_DependencyResourcesCount.TryGetValue(item,out reference)){
                                _DependencyResourcesCount[item]=reference-1;
                            }
                            else{
                                throw new FrameworkException(Utility.Text.Format(" Resources target {0} dependency reference count is invalid "));
                            }
                        }
                    }
                    _DependencyResourcesCount.Remove(GetTarget);
                    _ResourcesHelper.Release(GetTarget);
                }
            }
        }
    }
}
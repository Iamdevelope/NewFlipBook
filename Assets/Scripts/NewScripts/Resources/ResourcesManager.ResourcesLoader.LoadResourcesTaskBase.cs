using System;
using System.Collections.Generic;
using PJW.Task;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        private partial class ResourcesLoader
        {
            /// <summary>
            /// 加载资源任务基类
            /// </summary>
            private abstract class LoadResourcesTaskBase : ITask
            {
                private static int _Serial=0;
                private readonly int _Priority;
                private readonly int _SerialId;
                private readonly string _AssetName;
                private readonly Type _AssetType;
                private readonly ResourcesInfo _ResourcesInfo;
                private readonly string _ResourcesChildName;
                private readonly string[] _DependencyAssetNames;
                private readonly string[] _ScatteredDependencyAssetNames;
                private readonly object _UserData;
                private readonly List<object> _DependencyAssets;
                private readonly List<object> _DependencyResources;
                private ResourcesObject _ResourcesObject;
                private DateTime _StartTime;
                private int _TotalDependencyAssetCount;
                private bool _Done;

                /// <summary>
                /// 加载资源任务基类
                /// </summary>
                /// <param name="assetName">资源名</param>
                /// <param name="assetType">资源类型</param>
                /// <param name="priority">资源优先级</param>
                /// <param name="resourcesInfo">资源信息</param>
                /// <param name="resourcesChildName">资源子资源名</param>
                /// <param name="dependencyAssetNames">依赖资源集合</param>
                /// <param name="scatteredDependencyAssetsNames">依赖资源集合</param>
                public LoadResourcesTaskBase(string assetName,Type assetType,int priority,ResourcesInfo resourcesInfo,string resourcesChildName,
                string[] dependencyAssetNames,string[] scatteredDependencyAssetsNames,object userData){
                    _SerialId=_Serial++;
                    _Priority=priority;
                    _AssetName=assetName;
                    _AssetType=assetType;
                    _ResourcesInfo=resourcesInfo;
                    _ResourcesChildName=resourcesChildName;
                    _DependencyAssetNames=dependencyAssetNames;
                    _ScatteredDependencyAssetNames=scatteredDependencyAssetsNames;
                    _UserData=userData;
                    _DependencyAssets=new List<object>();
                    _DependencyResources=new List<object>();
                    _ResourcesObject=null;
                    _StartTime=default(DateTime);
                    _TotalDependencyAssetCount=0;
                    _Done=false;
                }
                
                /// <summary>
                /// 获取任务序列号
                /// </summary>
                /// <value></value>
                public int GetSerialId
                {
                    get
                    {
                        return _SerialId;
                    }
                }

                /// <summary>
                /// 获取任务优先级
                /// </summary>
                /// <value></value>
                public int GetPriority{
                    get{
                        return _Priority;
                    }
                }

                /// <summary>
                /// 获取任务是否完成
                /// </summary>
                /// <value></value>
                public bool Done{
                    get{
                        return _Done;
                    }
                    set{
                        _Done=value;
                    }
                }
                public string GetAssetName{
                    get{
                        return _AssetName;
                    }
                }
                public Type GetAssetType{
                    get{
                        return _AssetType;
                    }
                }
                public ResourcesInfo GetResourcesInfo{
                    get{
                        return _ResourcesInfo;
                    }
                }
                public string GetResourcesChildName{
                    get{
                        return _ResourcesChildName;
                    }
                }
                public object GetUserData{
                    get{
                        return _UserData;
                    }
                }
                public ResourcesObject GetResourcesObject{
                    get{
                        return _ResourcesObject;
                    }
                }
                public abstract bool GetIsScene{
                    get;
                }
                public DateTime StartTime{
                    get{
                        return _StartTime;
                    }
                    set{
                        _StartTime=value;
                    }
                }
                public int GetLoadedDependencyAssetCount{
                    get{
                        return _DependencyAssets.Count;
                    }
                }
                public int TotalDependencyAssetCount{
                    get{
                        return _TotalDependencyAssetCount;
                    }
                    set{
                        _TotalDependencyAssetCount=value;
                    }
                }
                public string[] GetDependencyAssetNames(){
                    return _DependencyAssetNames;
                }
                public string[] GetScatteredDependencyAssetNames(){
                    return _ScatteredDependencyAssetNames;
                }
                public object[] GetDependencyAssets(){
                    return _DependencyAssets.ToArray();
                }
                public List<object> GetDependencyResources(){
                    return _DependencyResources;
                }

                public void LoadMain(LoadResourcesAgent agent,ResourcesObject resourcesObject)
                {
                    _ResourcesObject=resourcesObject;
                    agent.GetHelper.LoadAsset(resourcesObject.GetTarget,GetResourcesChildName,GetAssetType,GetIsScene);
                }
                public virtual void OnLoadAssetSuccess(LoadResourcesAgent agent,object asset,float duration){

                }
                public virtual void OnLoadAssetFailure(LoadResourcesAgent agent,LoadResourceStatus status,string errorMessage){

                }
                public virtual void OnLoadAssetUpdate(LoadResourcesAgent agent,LoadResourcesProgressType type,float progress){

                }
                public virtual void OnLoadAssetDependency(LoadResourcesAgent agent,string dependencyAssetName,object dependencyAsset,object dependencyResource){
                    _DependencyAssets.Add(dependencyAsset);
                    if(dependencyResource!=null&&!_DependencyResources.Contains(dependencyResource)){
                        _DependencyResources.Add(dependencyResource);
                    }
                }
            }
        }
    }
}
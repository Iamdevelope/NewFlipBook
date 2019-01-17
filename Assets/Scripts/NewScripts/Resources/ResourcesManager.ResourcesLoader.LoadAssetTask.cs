using System;

namespace PJW.Resources
{
    internal partial class ResourcesManager{
        private partial class ResourcesLoader{
            /// <summary>
            /// 加载资源任务
            /// </summary>
            private sealed class LoadAssetTask : LoadResourcesTaskBase
            {
                public override bool GetIsScene
                {
                    get
                    {
                        return false;
                    }
                }
                private readonly LoadAssetCallbacks m_LoadAssetCallbacks;

                public LoadAssetTask(string assetName, Type assetType, int priority, ResourcesInfo resourceInfo, string resourceChildName, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, LoadAssetCallbacks loadAssetCallbacks, object userData)
                    : base(assetName, assetType, priority, resourceInfo, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, userData)
                {
                    m_LoadAssetCallbacks = loadAssetCallbacks;
                }

                public override void OnLoadAssetSuccess(LoadResourcesAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    if (m_LoadAssetCallbacks.GetLoadAssetSuccessCallback != null)
                    {
                        m_LoadAssetCallbacks.GetLoadAssetSuccessCallback(GetAssetName, asset, duration, GetUserData);
                    }
                }

                public override void OnLoadAssetFailure(LoadResourcesAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    if (m_LoadAssetCallbacks.GetLoadAssetFailureCallback != null)
                    {
                        m_LoadAssetCallbacks.GetLoadAssetFailureCallback(GetAssetName, status, errorMessage, GetUserData);
                    }
                }

                public override void OnLoadAssetUpdate(LoadResourcesAgent agent, LoadResourcesProgressType type, float progress)
                {
                    base.OnLoadAssetUpdate(agent, type, progress);
                    if (type == LoadResourcesProgressType.LoadAsset)
                    {
                        if (m_LoadAssetCallbacks.GetLoadAssetUpdateCallback != null)
                        {
                            m_LoadAssetCallbacks.GetLoadAssetUpdateCallback(GetAssetName, progress, GetUserData);
                        }
                    }
                }

                public override void OnLoadAssetDependency(LoadResourcesAgent agent, string dependencyAssetName, object dependencyAsset, object dependencyResource)
                {
                    base.OnLoadAssetDependency(agent, dependencyAssetName, dependencyAsset, dependencyResource);
                    if (m_LoadAssetCallbacks.GetLoadAssetDependencyCallback != null)
                    {
                        m_LoadAssetCallbacks.GetLoadAssetDependencyCallback(GetAssetName, dependencyAssetName, GetLoadedDependencyAssetCount, TotalDependencyAssetCount, GetUserData);
                    }
                }
            }
        }
    }
}
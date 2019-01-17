namespace PJW.Resources
{
    internal partial class ResourcesManager{
        private partial class ResourcesLoader{
            /// <summary>
            /// 加载场景任务
            /// </summary>
            private sealed class LoadSceneTask : LoadResourcesTaskBase
            {
                private readonly LoadSceneCallbacks m_LoadSceneCallbacks;

                public LoadSceneTask(string sceneAssetName, int priority, ResourcesInfo resourceInfo, string resourceChildName, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, LoadSceneCallbacks loadSceneCallbacks, object userData)
                    : base(sceneAssetName, null, priority, resourceInfo, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, userData)
                {
                    m_LoadSceneCallbacks = loadSceneCallbacks;
                }

                public override bool GetIsScene
                {
                    get
                    {
                        return true;
                    }
                }

                public override void OnLoadAssetSuccess(LoadResourcesAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    if (m_LoadSceneCallbacks.GetLoadSceneSuccessCallback != null)
                    {
                        m_LoadSceneCallbacks.GetLoadSceneSuccessCallback(GetAssetName, duration, GetUserData);
                    }
                }

                public override void OnLoadAssetFailure(LoadResourcesAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    if (m_LoadSceneCallbacks.GetLoadSceneFailureCallback != null)
                    {
                        m_LoadSceneCallbacks.GetLoadSceneFailureCallback(GetAssetName, status, errorMessage, GetUserData);
                    }
                }

                public override void OnLoadAssetUpdate(LoadResourcesAgent agent, LoadResourcesProgressType type, float progress)
                {
                    base.OnLoadAssetUpdate(agent, type, progress);
                    if (type == LoadResourcesProgressType.LoadScene)
                    {
                        if (m_LoadSceneCallbacks.GetLoadSceneUpdateCallback != null)
                        {
                            m_LoadSceneCallbacks.GetLoadSceneUpdateCallback(GetAssetName, progress, GetUserData);
                        }
                    }
                }

                public override void OnLoadAssetDependency(LoadResourcesAgent agent, string dependencyAssetName, object dependencyAsset, object dependencyResource)
                {
                    base.OnLoadAssetDependency(agent, dependencyAssetName, dependencyAsset, dependencyResource);
                    if (m_LoadSceneCallbacks.GetLoadSceneDependencyCallback != null)
                    {
                        m_LoadSceneCallbacks.GetLoadSceneDependencyCallback(GetAssetName, dependencyAssetName, GetLoadedDependencyAssetCount, TotalDependencyAssetCount, GetUserData);
                    }
                }
            }
        }
    }
}
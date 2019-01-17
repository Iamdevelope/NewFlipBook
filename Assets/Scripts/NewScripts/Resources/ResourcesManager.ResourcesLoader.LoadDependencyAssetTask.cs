namespace PJW.Resources
{
    internal partial class ResourcesManager{
        private partial class ResourcesLoader{
            /// <summary>
            /// 加载依赖资源任务
            /// </summary>
            private sealed class LoadDependencyAssetTask:LoadResourcesTaskBase
            {
                private readonly LoadResourcesTaskBase m_MainTask;

                public LoadDependencyAssetTask(string assetName, int priority, ResourcesInfo resourceInfo, string resourceChildName, string[] dependencyAssetNames, string[] scatteredDependencyAssetNames, LoadResourcesTaskBase mainTask, object userData)
                    : base(assetName, null, priority, resourceInfo, resourceChildName, dependencyAssetNames, scatteredDependencyAssetNames, userData)
                {
                    m_MainTask = mainTask;
                    m_MainTask.TotalDependencyAssetCount++;
                }

                public override bool GetIsScene
                {
                    get
                    {
                        return false;
                    }
                }

                public override void OnLoadAssetSuccess(LoadResourcesAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    m_MainTask.OnLoadAssetDependency(agent, GetAssetName, asset, GetResourcesObject != null ? GetResourcesObject.GetTarget : null);
                }

                public override void OnLoadAssetFailure(LoadResourcesAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    m_MainTask.OnLoadAssetFailure(agent, LoadResourceStatus.DependencyAssetError, Utility.Text.Format("Can not load dependency asset '{0}', internal status '{1}', internal error message '{2}'.", GetAssetName, status.ToString(), errorMessage));
                }
            }
        }
    }
}
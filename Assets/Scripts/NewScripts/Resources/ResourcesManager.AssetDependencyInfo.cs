namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源依赖信息
        /// </summary>
        private struct AssetDependencyInfo
        {
            private readonly string _AssetName;
            private readonly string[] _DependencyAssetNames;
            private readonly string[] _ScatteredDependencyAssetNames;
            
            /// <summary>
            /// 资源依赖信息实例
            /// </summary>
            /// <param name="assetName">资源名称</param>
            /// <param name="dependencyAssetNames">依赖资源名</param>
            /// <param name="scatteredDependencyAssetNames">依赖零散资源名称</param>
            public AssetDependencyInfo(string assetName,string[] dependencyAssetNames,string[] scatteredDependencyAssetNames){
                _AssetName=assetName;
                _DependencyAssetNames=dependencyAssetNames;
                _ScatteredDependencyAssetNames=scatteredDependencyAssetNames;
            }
            public string GetAssetName{
                get{
                    return _AssetName;
                }
            }
            public string[] GetDependencyAssetNames(){
                return _DependencyAssetNames;
            }
            public string[] GetScatteredDependencyAssetNames(){
                return _ScatteredDependencyAssetNames;
            }
        }
    }
}
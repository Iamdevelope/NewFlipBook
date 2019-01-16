namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        private struct AssetInfo
        {
            private readonly string _AssetName;
            private readonly ResourcesName _ResourcesName;
            private readonly string _ChildResourceName;
            
            /// <summary>
            /// 资源信息
            /// </summary>
            /// <param name="assetName">资源名称</param>
            /// <param name="resourcesName">所在资源名</param>
            /// <param name="childResourceName">子资源名</param>
            public AssetInfo(string assetName,ResourcesName resourcesName,string childResourceName){
                _AssetName=assetName;
                _ResourcesName=resourcesName;
                _ChildResourceName=childResourceName;
            }
            public string GetAssetName{
                get{
                    return _AssetName;
                }
            }
            public ResourcesName GetResourcesName{
                get{
                    return _ResourcesName;
                }
            }
            public string GetChildResourcesName{
                get{
                    return _ChildResourceName;
                }
            }
        }
    }
}
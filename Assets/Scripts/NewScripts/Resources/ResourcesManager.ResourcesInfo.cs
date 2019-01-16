namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        private struct ResourcesInfo
        {
            private readonly ResourcesName _ResourcesName;
            private readonly LoadType _LoadType;
            private readonly int _Length;
            private readonly int _HashCode;
            private readonly bool _StorageInReadOnly;

            public ResourcesName GetResourcesName{
                get{
                    return _ResourcesName;
                }
            }
            public LoadType GetLoadType{
                get{
                    return _LoadType;
                }
            }
            public int GetLength{
                get{
                    return _Length;
                }
            }
            public int GetHashCode{
                get{
                    return _HashCode;
                }
            }
            public bool GetStorageInReadOnly{
                get{
                    return _StorageInReadOnly;
                }
            }

            /// <summary>
            /// 资源信息
            /// </summary>
            /// <param name="resourcesName">资源名称</param>
            /// <param name="loadType">资源类型</param>
            /// <param name="length">资源大小</param>
            /// <param name="hashCode">资源哈希值</param>
            /// <param name="storageInReadOnly">资源是否是只读</param>
            public ResourcesInfo(ResourcesName resourcesName,LoadType loadType,int length,int hashCode,bool storageInReadOnly){
                _ResourcesName=resourcesName;
                _LoadType=loadType;
                _Length=length;
                _LoadType=loadType;
                _HashCode=hashCode;
                _StorageInReadOnly=storageInReadOnly;
            }
        }
    }
}
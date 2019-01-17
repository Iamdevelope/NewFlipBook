using System.Collections.Generic;

namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源组
        /// </summary>
        private sealed class ResourcesGroup
        {
            private readonly Dictionary<ResourcesName,ResourcesInfo> _ResourcesInfos;
            private readonly List<ResourcesName> _ResourcesNames;
            private int _TotalLength;
            
            /// <summary>
            /// 资源组实例
            /// </summary>
            /// <param name="resourcesInfo">资源信息引用</param>
            public ResourcesGroup(Dictionary<ResourcesName,ResourcesInfo> resourcesInfo){
                if(resourcesInfo==null){
                    throw new FrameworkException("Resources info is invalid ");
                }
                _ResourcesInfos=resourcesInfo;
                _ResourcesNames=new List<ResourcesName>();
                _TotalLength=0;
            }

            /// <summary>
            /// 获取资源组是否准备好
            /// </summary>
            /// <value></value>
            public bool GetReady
            {
                get{
                    return GetReadyResourcesCount >= GetResourcesCount;
                }
            }
            
            /// <summary>
            /// 获取资源组资源数量
            /// </summary>
            /// <value></value>
            public int GetResourcesCount{
                get{
                    return _ResourcesNames.Count;
                }
            }

            /// <summary>
            /// 获取资源组已准备完成的资源数量
            /// </summary>
            /// <value></value>
            public int GetReadyResourcesCount 
            { 
                get{
                    int readyResourcesCount=0;
                    foreach (ResourcesName item in _ResourcesNames)
                    {
                        if(_ResourcesInfos.ContainsKey(item)){
                            readyResourcesCount++;
                        }
                    }
                    return readyResourcesCount;
                }
            }

            /// <summary>
            /// 获取资源组总大小
            /// </summary>
            /// <value></value>
            public int GetTotalLength{
                get{
                    return _TotalLength;
                }
            }
            public int GetTotalReadyLength{
                get{
                    int totalReadyLength=0;
                    foreach (ResourcesName item in _ResourcesNames)
                    {
                        ResourcesInfo resourcesInfo=default(ResourcesInfo);
                        if(_ResourcesInfos.TryGetValue(item,out resourcesInfo)){
                            totalReadyLength+=resourcesInfo.GetLength;
                        }
                    }
                    return totalReadyLength;
                }
            }

            /// <summary>
            /// 资源组准备进度
            /// </summary>
            /// <value></value>
            public float Progress{
                get{
                    return GetTotalLength>0?(float)GetTotalReadyLength/GetTotalLength:1f;
                }
            }

            /// <summary>
            /// 向资源组中增加资源
            /// </summary>
            /// <param name="name">资源名</param>
            /// <param name="variant">变体名</param>
            /// <param name="length">资源大小</param>
            public void AddResource(string name,string variant,int length){
                _ResourcesNames.Add(new ResourcesName(name,variant));
                _TotalLength+=length;
            }
        }
    }
}
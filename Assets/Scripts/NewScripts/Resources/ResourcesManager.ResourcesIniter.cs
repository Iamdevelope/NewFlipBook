using System;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        /// <summary>
        /// 资源初始化
        /// </summary>
        private sealed class ResourcesIniter
        {
            private readonly ResourcesManager _ResourcesManager;
            private string _CurrentVariant;
            public FrameworkAction ResourceInitComplete;

            /// <summary>
            /// 初始化资源初始化器
            /// </summary>
            /// <param name="resourcesManager">资源管理器</param>
            public ResourcesIniter(ResourcesManager resourcesManager){
                _ResourcesManager=resourcesManager;
                _CurrentVariant=null;
                ResourceInitComplete=null;
            }

            /// <summary>
            /// 清理并关闭资源初始化器
            /// </summary>
            public void Shutdown(){

            }

            /// <summary>
            /// 初始化资源初始器
            /// </summary>
            /// <param name="currentVariant"></param>
            public void InitResources(string currentVariant){
                _CurrentVariant=currentVariant;
                if(_ResourcesManager._ResourcesHelper==null){
                    throw new FrameworkException("Resource helper is invalid ");
                }
                _ResourcesManager._ResourcesHelper.LoadBytes(Utility.Path.GetRemotePath(_ResourcesManager._ReadOnlyPath,Utility.Path.GetResourceNameWithSuffix(VersionListFileName)),ParsePackageList);
            }

            /// <summary>
            /// 解析资源包资源列表
            /// </summary>
            /// <param name="filePath">版本资源列表文件路径</param>
            /// <param name="bytes">要解析的数据</param>
            /// <param name="errorMessage">错误信息</param>
            private void ParsePackageList(string filePath, byte[] bytes, string errorMessage)
            {
                if(bytes==null||bytes.Length<=0){
                    throw new FrameworkException(Utility.Text.Format("Package list {0} is invalid, error message is {1} ",filePath,string.IsNullOrEmpty(errorMessage) ? "Empty" : errorMessage));
                }
            }
        }
    }
}
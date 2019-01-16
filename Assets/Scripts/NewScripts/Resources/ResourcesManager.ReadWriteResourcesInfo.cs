namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 读取资源信息
        /// </summary>
        public class ReadWriteResourcesInfo
        {
            private readonly LoadType _LoadType;
            private readonly int _Length;
            private readonly int _HashCode;
            public ReadWriteResourcesInfo(LoadType loadType,int length,int hashCode){
                _LoadType=loadType;
                _Length=length;
                _HashCode=hashCode;
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
        }
    }
}
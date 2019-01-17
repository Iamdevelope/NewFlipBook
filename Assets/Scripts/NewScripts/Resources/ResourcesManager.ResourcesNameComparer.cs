using System.Collections.Generic;

namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源名称比较器
        /// </summary>
        private sealed class ResourcesNameComparer : IComparer<ResourcesName>,IEqualityComparer<ResourcesName>
        {
            public int Compare(ResourcesName x, ResourcesName y)
            {
                return x.CompareTo(y);
            }

            public bool Equals(ResourcesName x, ResourcesName y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(ResourcesName obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
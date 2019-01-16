using System;

namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源名
        /// </summary>
        private sealed class ResourcesName : IComparable, IComparable<ResourcesName>, IEquatable<ResourcesName>
        {
            private readonly string _Name;
            private readonly string _Variant;

            /// <summary>
            /// 初始化资源名称
            /// </summary>
            /// <param name="name">资源名</param>
            /// <param name="variant">变体名</param>
            public ResourcesName(string name,string variant)
            {
                _Name=name;
                _Variant=variant;
            }

            public string GetName{
                get{return _Name;}
            }
            public string GetVariant{
                get{
                    return _Variant;
                }
            }
            public bool IsVariant{
                get{
                    return _Variant!=null;
                }
            }
            public string FullName{
                get{
                    return IsVariant?Utility.Text.Format("{0}.{1}",GetName,GetVariant):GetName;
                }
            }
            public override string ToString()
            {
                return FullName;
            }
            public override int GetHashCode()
            {
                if(GetVariant==null){
                    return GetName.GetHashCode();
                }
                return (GetName.GetHashCode()^GetVariant.GetHashCode());
            }
            public override bool Equals(object obj)
            {
                return (obj is ResourcesName)&&(this==(ResourcesName)obj);
            }
            public bool Equals(ResourcesName resourcesName)
            {
                return this==resourcesName;
            }
            public static bool operator ==(ResourcesName r1,ResourcesName r2)
            {
                return r1.CompareTo(r2)==0;
            }
            public static bool operator !=(ResourcesName r1,ResourcesName r2)
            {
                return r1.CompareTo(r2)!=0;
            }
            public int CompareTo(object obj)
            {
                if(obj==null)
                {
                    return 1;
                }
                if(!(obj is ResourcesName))
                {
                    throw new FrameworkException(" Type of obj is invalid ");
                }
                return CompareTo((ResourcesName)obj);
            }

            public int CompareTo(ResourcesName other)
            {
                int result=string.Compare(GetName,other.GetName);
                if(result!=0)
                {
                    return result;
                }
                return string.Compare(GetVariant,other.GetVariant);
            }
        }
    }
}
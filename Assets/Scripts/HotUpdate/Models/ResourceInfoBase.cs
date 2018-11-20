using UnityEngine;

namespace PJW.HotUpdate
{
    [SerializeField]
    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResourceInfoBase
    {
        /// <summary>
        /// 资源状态
        /// 0表示不改变，1表示增加，2表示修改，3表示删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Hashcode { get; set; }
        /// <summary>
        /// 自身版本
        /// </summary>
        public System.Version SelfVersion { get; set; }
        /// <summary>
        /// 资源大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 资源名字
        /// </summary>
        public string AssetName { get; set; }
    }
}
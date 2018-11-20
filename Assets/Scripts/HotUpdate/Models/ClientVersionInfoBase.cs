using UnityEngine;

namespace PJW.HotUpdate
{
    [SerializeField]
    /// <summary>
    /// 客户端版本信息
    /// </summary>
    public class ClientVersionInfoBase
    {
        /// <summary>
        /// 当前客户端版本
        /// </summary>
        public System.Version CurretVersion { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// MD5值
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 补丁包大小（压缩前）
        /// </summary>
        public long PackageSize { get; set; }
        /// <summary>
        /// 资源大小（压缩后）
        /// </summary>
        public long ResourceSize { get; set; }
        /// <summary>
        /// 渠道标示
        /// </summary>
        public string Channel { get; set; }
    }
}
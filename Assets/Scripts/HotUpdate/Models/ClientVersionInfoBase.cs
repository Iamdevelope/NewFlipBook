using UnityEngine;

namespace PJW.HotUpdate
{
    [SerializeField]
    /// <summary>
    /// �ͻ��˰汾��Ϣ
    /// </summary>
    public class ClientVersionInfoBase
    {
        /// <summary>
        /// ��ǰ�ͻ��˰汾
        /// </summary>
        public System.Version CurretVersion { get; set; }
        /// <summary>
        /// url��ַ
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// MD5ֵ
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// ��������С��ѹ��ǰ��
        /// </summary>
        public long PackageSize { get; set; }
        /// <summary>
        /// ��Դ��С��ѹ����
        /// </summary>
        public long ResourceSize { get; set; }
        /// <summary>
        /// ������ʾ
        /// </summary>
        public string Channel { get; set; }
    }
}
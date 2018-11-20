using UnityEngine;

namespace PJW.HotUpdate
{
    [SerializeField]
    /// <summary>
    /// ��Դ��Ϣ
    /// </summary>
    public class ResourceInfoBase
    {
        /// <summary>
        /// ��Դ״̬
        /// 0��ʾ���ı䣬1��ʾ���ӣ�2��ʾ�޸ģ�3��ʾɾ��
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// Ψһ��ʶ
        /// </summary>
        public string Hashcode { get; set; }
        /// <summary>
        /// ����汾
        /// </summary>
        public System.Version SelfVersion { get; set; }
        /// <summary>
        /// ��Դ��С
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// ��Դ����
        /// </summary>
        public string AssetName { get; set; }
    }
}
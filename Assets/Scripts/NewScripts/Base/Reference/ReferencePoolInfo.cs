
namespace PJW
{
    /// <summary>
    /// 引用信息
    /// </summary>
    public sealed class ReferencePoolInfo
    {
        private readonly string _TypeName;
        private readonly int _UnusedReferenceCount;
        private readonly int _UsingReferenceCount;
        private readonly int _AcquireReferenceCount;
        private readonly int _ReleaseReferenceCount;
        private readonly int _AddReferenceCount;
        private readonly int _RemoveReferenceCount;
        /// <summary>
        /// 初始化引用池信息
        /// </summary>
        /// <param name="typeName">引用池类型名称</param>
        /// <param name="unusedReferencedCount">未被使用的引用数量</param>
        /// <param name="usingReferenceCount">正在使用的引用数量</param>
        /// <param name="acquireReferenceCount">获取引用数量</param>
        /// <param name="releaseReferenceCount">归还引用数量</param>
        /// <param name="addReferenceCount">增加引用数量</param>
        /// <param name="removeReferenceCount">移除引用数量</param>
        public ReferencePoolInfo(string typeName,int unusedReferencedCount,int usingReferenceCount,int acquireReferenceCount,int releaseReferenceCount,
            int addReferenceCount,int removeReferenceCount)
        {
            _TypeName = typeName;
            _UnusedReferenceCount = unusedReferencedCount;
            _UsingReferenceCount = usingReferenceCount;
            _AcquireReferenceCount = acquireReferenceCount;
            _ReleaseReferenceCount = releaseReferenceCount;
            _AddReferenceCount = addReferenceCount;
            _RemoveReferenceCount = removeReferenceCount;
        }
        /// <summary>
        /// 获取类型名
        /// </summary>
        public string GetTypeName
        {
            get { return _TypeName; }
        }
        /// <summary>
        /// 获取未被使用的引用数量
        /// </summary>
        public int GetUnusedReferenceCount
        {
            get { return _UnusedReferenceCount; }
        }
        /// <summary>
        /// 获取正在被使用的引用数量
        /// </summary>
        public int GetUsingReferenceCount
        {
            get { return _UsingReferenceCount; }
        }
        /// <summary>
        /// 获取引用数量
        /// </summary>
        public int GetAcquireReferenceCount
        {
            get { return _AcquireReferenceCount; }
        }
        /// <summary>
        /// 获取被释放的引用数量
        /// </summary>
        public int GetReleaseReferenceCount
        {
            get { return _ReleaseReferenceCount; }
        }
        /// <summary>
        /// 获取增加的引用数量
        /// </summary>
        public int GetAddReferenceCount
        {
            get { return _AddReferenceCount; }
        }
        /// <summary>
        /// 获取移除的引用数量
        /// </summary>
        public int GetRemoveReferenceCount
        {
            get { return _RemoveReferenceCount; }
        }
    }
}
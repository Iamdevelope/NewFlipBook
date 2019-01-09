
namespace PJW
{
    /// <summary>
    /// 数据片段
    /// </summary>
    /// <typeparam name="T">数据源类型</typeparam>
    public struct FrameworkSegment<T> where T : class
    {
        private readonly T _Source;
        private readonly int _Offset;
        private readonly int _Length;

        /// <summary>
        /// 数据源构造函数
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="offset">偏移</param>
        /// <param name="length">长度</param>
        public FrameworkSegment(T source,int offset,int length)
        {
            if (source == null)
            {
                throw new FrameworkException("Source is invalid.");
            }

            if (offset < 0)
            {
                throw new FrameworkException("Offset is invalid.");
            }

            if (length <= 0)
            {
                throw new FrameworkException("Length is invalid.");
            }

            _Source = source;
            _Offset = offset;
            _Length = length;
        }
        public T GetSource
        {
            get { return _Source; }
        }
        public int GetOffset
        {
            get { return _Offset; }
        }
        public int GetLength
        {
            get { return _Length; }
        }
        /// <summary>
        /// 获取对象的哈希值
        /// </summary>
        /// <returns>对象的哈希值</returns>
        public override int GetHashCode()
        {
            return _Source.GetHashCode() ^ _Offset ^ _Length;
        }
        /// <summary>
        /// 比较对象是否与自身相等
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is FrameworkSegment<T> && Equals((FrameworkSegment<T>)obj);
        }
        /// <summary>
        /// 比较对象是否与自身相等
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns></returns>
        public bool Equals(FrameworkSegment<T> obj)
        {
            return obj.GetLength == GetLength && obj.GetOffset == GetOffset && obj.GetSource == GetSource;
        }
        /// <summary>
        /// 判断两个对象是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(FrameworkSegment<T> a,FrameworkSegment<T> b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// 判断两个对象是否不相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(FrameworkSegment<T> a, FrameworkSegment<T> b)
        {
            return !(a == b);
        }
    }
}

namespace PJW
{
    /// <summary>
    /// ����Ƭ��
    /// </summary>
    /// <typeparam name="T">����Դ����</typeparam>
    public struct FrameworkSegment<T> where T : class
    {
        private readonly T _Source;
        private readonly int _Offset;
        private readonly int _Length;

        /// <summary>
        /// ����Դ���캯��
        /// </summary>
        /// <param name="source">����Դ</param>
        /// <param name="offset">ƫ��</param>
        /// <param name="length">����</param>
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
        /// ��ȡ����Ĺ�ϣֵ
        /// </summary>
        /// <returns>����Ĺ�ϣֵ</returns>
        public override int GetHashCode()
        {
            return _Source.GetHashCode() ^ _Offset ^ _Length;
        }
        /// <summary>
        /// �Ƚ϶����Ƿ����������
        /// </summary>
        /// <param name="obj">��Ҫ�ȽϵĶ���</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is FrameworkSegment<T> && Equals((FrameworkSegment<T>)obj);
        }
        /// <summary>
        /// �Ƚ϶����Ƿ����������
        /// </summary>
        /// <param name="obj">��Ҫ�ȽϵĶ���</param>
        /// <returns></returns>
        public bool Equals(FrameworkSegment<T> obj)
        {
            return obj.GetLength == GetLength && obj.GetOffset == GetOffset && obj.GetSource == GetSource;
        }
        /// <summary>
        /// �ж����������Ƿ����
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(FrameworkSegment<T> a,FrameworkSegment<T> b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// �ж����������Ƿ����
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
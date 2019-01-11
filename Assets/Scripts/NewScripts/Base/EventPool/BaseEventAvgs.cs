namespace PJW
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseEventAvgs : FrameworkEventAvgs, IReference
    {
        public abstract int Id { get; }
        public abstract void Clear();
    }
}
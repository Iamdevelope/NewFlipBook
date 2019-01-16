namespace PJW
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseEventArgs : FrameworkEventArgs, IReference
    {
        public abstract int Id { get; }
        public abstract void Clear();
    }
}
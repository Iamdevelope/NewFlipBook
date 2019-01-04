namespace PJW
{
    /// <summary>
    /// 框架模块抽象类
    /// </summary>
    internal abstract class FrameworkModule
    {
        /// <summary>
        /// 模块优先级
        /// </summary>
        internal virtual int Priority
        {
            get { return 0; }
        }
        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);
        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        internal abstract void Shutdown();
    }
}

namespace PJW.Task
{
    /// <summary>
    /// 任务代理器接口
    /// </summary>
    /// <typeparam name="T">任务类型</typeparam>
    public interface ITaskAgent<T> where T : ITask
    {
        /// <summary>
        /// 获取当前任务
        /// </summary>
        T GetTask
        {
            get;
        }
        /// <summary>
        /// 初始化任务代理
        /// </summary>
        void Init();
        /// <summary>
        /// 任务代理轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑上流逝的时间</param>
        /// <param name="realElapseSeconds">实际上流逝的时间</param>
        void Update(float elapseSeconds, float realElapseSeconds);
        /// <summary>
        /// 开启任务处理
        /// </summary>
        /// <param name="task">任务</param>
        void Start(T task);
        /// <summary>
        /// 关闭当前任务代理
        /// </summary>
        void Shutdown();
        /// <summary>
        /// 停止正在处理的任务并重置任务代理
        /// </summary>
        void Reset();
    }
}
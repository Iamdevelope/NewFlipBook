
namespace PJW.Task
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 任务序列号
        /// </summary>
        int GetSerialId
        {
            get;
        }
        /// <summary>
        /// 任务优先级
        /// </summary>
        int GetPriority
        {
            get;
        }
        /// <summary>
        /// 任务是否完成
        /// </summary>
        bool Done
        {
            get;
        }
    }
}
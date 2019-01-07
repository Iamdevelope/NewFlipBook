
using System;

namespace PJW.FSM
{
    /// <summary>
    /// 有限状态机基类
    /// </summary>
    public abstract class FsmBase
    {
        private readonly string _Name;

        public FsmBase() : this(null) { }
        public FsmBase(string name)
        {
            _Name = name ?? string.Empty;
        }
        /// <summary>
        /// 状态机名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }
        /// <summary>
        /// 状态机持有者类型
        /// </summary>
        public abstract Type OwnerType
        {
            get;
        }
        /// <summary>
        /// 状态机的状态个数
        /// </summary>
        public abstract int FsmStateCount
        {
            get;
        }
        /// <summary>
        /// 状态机是否正在运行
        /// </summary>
        public abstract bool IsRunning
        {
            get;
        }
        /// <summary>
        /// 状态机是否被销毁
        /// </summary>
        public abstract bool IsDestroyed
        {
            get;
        }
        /// <summary>
        /// 当前状态名称
        /// </summary>
        public abstract string CurrentStateName
        {
            get;
        }
        /// <summary>
        /// 当前状态机持续时间
        /// </summary>
        public abstract float CurrentStateTime
        {
            get;
        }
        /// <summary>
        /// 状态机轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public abstract void Update(float elapseSeconds, float realElapseSeconds);
        /// <summary>
        /// 关闭并清理状态机
        /// </summary>
        public abstract void ShutDown();
    }
}
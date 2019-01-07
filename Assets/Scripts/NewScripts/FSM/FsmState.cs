using System;
using System.Collections.Generic;

namespace PJW.FSM
{
    /// <summary>
    /// 有限状态机状态基类
    /// </summary>
    /// <typeparam name="T">状态机持有者类型</typeparam>
    public abstract class FsmState<T> where T : class
    {
        private readonly Dictionary<int, FsmEventHandler<T>> _EventHandler;

        public FsmState()
        {
            _EventHandler = new Dictionary<int, FsmEventHandler<T>>();
        }
        /// <summary>
        /// 状态初始化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fsm">当前有限状态机</param>
        protected internal virtual void OnInit(IFsm<T> fsm) { }
        /// <summary>
        /// 状态进入时
        /// </summary>
        /// <param name="fsm">当前有限状态机</param>
        protected internal virtual void OnEnter(IFsm<T> fsm) { }
        /// <summary>
        /// 该状态下一直调用
        /// </summary>
        /// <param name="fsm">当前有限状态机</param>
        /// <param name="elapseSeconds">理论所需消耗时长</param>
        /// <param name="realElapseSeconds">真实消耗时长</param>
        protected internal virtual void OnUpdate(IFsm<T> fsm,float elapseSeconds,float realElapseSeconds) { }
        /// <summary>
        /// 状态离开时调用
        /// </summary>
        /// <param name="fsm">当前有限状态机</param>
        /// <param name="isShutDown">是否关闭有限状态机时调用</param>
        protected internal virtual void OnExit(IFsm<T> fsm,bool isShutDown) { }
        /// <summary>
        /// 状态被销毁时调用
        /// </summary>
        /// <param name="fsm">当前有限状态机</param>
        protected internal virtual void OnDestroy(IFsm<T> fsm) {
            _EventHandler.Clear();
        }
        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="eventId">事件编号</param>
        /// <param name="eventHandler">响应事件</param>
        protected void AddListernEvent(int eventId,FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new FrameworkException(" FsmEventHandler is invalid ");
            }
            if (!_EventHandler.ContainsKey(eventId))
            {
                _EventHandler[eventId] = eventHandler;
            }
            else
            {
                _EventHandler[eventId] += eventHandler;
            }
        }
        /// <summary>
        /// 移除监听事件
        /// </summary>
        /// <param name="eventId">事件编号</param>
        /// <param name="eventHandler">响应事件</param>
        protected void RemoveListernEvent(int eventId,FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new FrameworkException(" FsmEventHandler is invalid ");
            }
            if (_EventHandler.ContainsKey(eventId))
            {
                _EventHandler[eventId] -= eventHandler;
            }
        }
        /// <summary>
        /// 切换当前状态时
        /// </summary>
        /// <typeparam name="IState">切换到目标状态的状态类型</typeparam>
        /// <param name="fsm">有限状态机</param>
        protected void ChangeState<IState>(IFsm<T> fsm) where IState : FsmState<T>
        {
            Fsm<T> temp = (Fsm<T>)fsm;
            if (temp == null)
            {
                throw new FrameworkException(" Fsm is invalid ");
            }
            temp.ChangeState<IState>();
        }
        /// <summary>
        /// 切换当前状态
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <param name="stateType">要切换到的状态类型</param>
        protected void ChangeState(IFsm<T> fsm,Type stateType)
        {
            Fsm<T> temp = (Fsm<T>)fsm;
            if (temp == null)
            {
                throw new FrameworkException(" Fsm is invalid ");
            }
            if (stateType == null)
            {
                throw new FrameworkException(" Fsm of type is invalid ");
            }
            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new FrameworkException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }
            temp.ChangeState(stateType);
        }
        /// <summary>
        /// 响应事件
        /// </summary>
        /// <param name="fsm">有限状态机</param>
        /// <param name="sender">发送者</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="userData">用户自定义数据</param>
        internal void OnEvent(IFsm<T> fsm,object sender,int eventId,object userData)
        {
            FsmEventHandler<T> eventHandler = null;
            if(_EventHandler.TryGetValue(eventId,out eventHandler))
            {
                if (eventHandler != null)
                {
                    eventHandler(fsm, sender, userData);
                }
            }
        }
    }
}
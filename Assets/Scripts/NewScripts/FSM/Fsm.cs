
using System;
using System.Collections.Generic;

namespace PJW.FSM
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    /// <typeparam name="T">状态机持有者类型</typeparam>
    public sealed class Fsm<T> : FsmBase, IFsm<T> where T : class
    {
        private readonly T _Owner;
        private readonly Dictionary<string, FsmState<T>> _State;
        private readonly Dictionary<string, Variable.Variable> _Data;
        private FsmState<T> _CurrentState;
        private float _CurrentStateTime;
        private bool _IsDestroyed;
        /// <summary>
        /// 状态机实例
        /// </summary>
        /// <param name="name">状态机名</param>
        /// <param name="owner">状态机持有者</param>
        /// <param name="states">状态机包含状态</param>
        public Fsm(string name, T owner, params FsmState<T>[] states) : base(name)
        {
            if (owner == null)
            {
                throw new FrameworkException(" Fsm owner is invalid ");
            }
            if(states==null || states.Length < 1)
            {
                throw new FrameworkException(" Fsm states is invalid ");
            }
            _Owner = owner;
            _State = new Dictionary<string, FsmState<T>>();
            _Data = new Dictionary<string, Variable.Variable>();
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new FrameworkException(" Fsm states is invalid ");
                }
                string stateName = state.GetType().FullName;
                if (_State.ContainsKey(stateName))
                {
                    throw new FrameworkException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", Utility.Text.GetFullName<T>(name), stateName));
                }
                _State.Add(stateName, state);
                state.OnInit(this);
            }
            _CurrentState = null;
            _CurrentStateTime = 0;
            _IsDestroyed = false;
        }
        /// <summary>
        /// 返回状态机持有者
        /// </summary>
        public T Owner
        {
            get
            {
                return _Owner;
            }
        }
        /// <summary>
        /// 获取状态机是否被销毁
        /// </summary>
        public bool IsDestroy
        {
            get { return _IsDestroyed; }
        }
        /// <summary>
        /// 获取当前状态机
        /// </summary>
        public FsmState<T> CurretState
        {
            get { return _CurrentState; }
        }
        /// <summary>
        /// 获取当前状态机持有者类型
        /// </summary>
        public override Type OwnerType
        {
            get
            {
                return typeof(T);
            }
        }
        /// <summary>
        /// 获取状态机包含状态个数
        /// </summary>
        public override int FsmStateCount
        {
            get { return _State.Count; }
        }
        /// <summary>
        /// 获取状态机是否正在运行
        /// </summary>
        public override bool IsRunning
        {
            get { return _CurrentState != null; }
        }
        /// <summary>
        /// 获取当前状态机是否被销毁
        /// </summary>
        public override bool IsDestroyed
        {
            get { return _IsDestroyed; }
        }
        /// <summary>
        /// 当前状态名
        /// </summary>
        public override string CurrentStateName
        {
            get { return _CurrentState != null ? _CurrentState.GetType().FullName : null; }
        }
        /// <summary>
        /// 当前状态持续时长
        /// </summary>
        public override float CurrentStateTime
        {
            get { return _CurrentStateTime; }
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }
        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        internal void ChangeState(Type stateType)
        {
            if (_CurrentState == null)
            {
                throw new FrameworkException("Current state is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new FrameworkException(Utility.Text.Format("FSM '{0}' can not change state to '{1}' which is not exist.", Utility.Text.GetFullName<T>(Name), stateType.FullName));
            }

            _CurrentState.OnExit(this, false);
            _CurrentStateTime = 0f;
            _CurrentState = state;
            _CurrentState.OnEnter(this);
        }
        /// <summary>
        /// 响应事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="eventId">事件ID</param>
        public void FireEvent(object sender, int eventId)
        {
            if (_CurrentState == null)
            {
                throw new FrameworkException(" current state is invalid ");
            }
            _CurrentState.OnEvent(this, sender, eventId, null);
        }
        /// <summary>
        /// 响应事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="userData">用户自定义数据</param>
        public void FireEvent(object sender, int eventId, object userData)
        {
            if (_CurrentState == null)
            {
                throw new FrameworkException(" current state is invalid ");
            }
            _CurrentState.OnEvent(this, sender, eventId, userData);
        }
        /// <summary>
        /// 获取所有状态
        /// </summary>
        /// <returns>有限状态机所有状态</returns>
        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] fsmStates = new FsmState<T>[_State.Count];
            foreach (KeyValuePair<string,FsmState<T>> state in _State)
            {
                fsmStates[index++] = state.Value;
            }
            return fsmStates;
        }
        /// <summary>
        /// 获取所有状态
        /// </summary>
        /// <param name="results">有限状态机所有状态</param>
        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new FrameworkException(" results is invalid ");
            }
            results.Clear();
            foreach (KeyValuePair<string,FsmState<T>> item in _State)
            {
                results.Add(item.Value);
            }
        }
        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <typeparam name="TData">要获取的有限状态机的数据类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>有限状态机数据</returns>
        public TData GetData<TData>(string name) where TData : Variable.Variable
        {
            return (TData)GetData(name);
        }
        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <returns>有限状态机数据</returns>
        public Variable.Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FrameworkException(" Data name is invalid ");
            }
            Variable.Variable data = null;
            if(_Data.TryGetValue(name,out data))
            {
                return data;
            }
            return null;
        }
        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <typeparam name="TState">状态类型</typeparam>
        /// <returns>有限状态机状态</returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (_State.TryGetValue(typeof(TState).FullName, out state))
            {
                return (TState)state;
            }

            return null;
        }
        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <param name="stateType">状态类型</param>
        /// <returns>有限状态机状态</returns>
        public FsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new FrameworkException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new FrameworkException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = null;
            if (_State.TryGetValue(stateType.FullName, out state))
            {
                return state;
            }

            return null;
        }
        /// <summary>
        /// 判断是否存在指定有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在指定有限状态机数据</returns>
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FrameworkException(" Data name is invalid ");
            }
            return _Data.ContainsKey(name);
        }
        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <typeparam name="TState">要检查的状态</typeparam>
        /// <returns>是否存在有限状态机状态</returns>
        public bool HasState<TState>() where TState : FsmState<T>
        {
            return _State.ContainsKey(typeof(TState).FullName);
        }
        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <typeparam name="stateType">要检查的状态</typeparam>
        /// <returns>是否存在有限状态机状态</returns>
        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new FrameworkException(" State is invalid ");
            }
            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new FrameworkException(Utility.Text.Format(" state type {0} is invalid ", stateType.FullName));
            }
            return _State.ContainsKey(stateType.FullName);
        }
        /// <summary>
        /// 移除有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FrameworkException(" Data name is invalid ");
            }
            return _Data.Remove(name);
        }
        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <typeparam name="TData">要设置的数据类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <param name="data">有限状态机数据</param>
        public void SetData<TData>(string name, TData data) where TData : Variable.Variable
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FrameworkException(" Data name is invalid ");
            }
            _Data[name] = data;
        }
        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="data">有限状态机数据</param>
        public void SetData(string name, Variable.Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FrameworkException(" Data name is invalid ");
            }
            _Data[name] = data;
        }

        public override void ShutDown()
        {
            if (_CurrentState != null)
            {
                _CurrentState.OnExit(this, true);
                _CurrentState = null;
                _CurrentStateTime = 0f;
            }

            foreach (KeyValuePair<string, FsmState<T>> state in _State)
            {
                state.Value.OnDestroy(this);
            }

            _State.Clear();
            _Data.Clear();

            _IsDestroyed = true;
        }
        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机的状态</typeparam>
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new FrameworkException(" fsm is running ");
            }
            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new FrameworkException(Utility.Text.Format("FSM '{0}' can not start state '{1}' which is not exist.", Utility.Text.GetFullName<T>(Name), typeof(TState).FullName));
            }
            _CurrentState = state;
            _CurrentStateTime = 0;
            _CurrentState.OnEnter(this);
        }
        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <typeparam name="stateType">要开始的有限状态机的状态类型</typeparam>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new FrameworkException(" fsm is running ");
            }
            if (stateType == null)
            {
                throw new FrameworkException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new FrameworkException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new FrameworkException(Utility.Text.Format("FSM '{0}' can not start state '{1}' which is not exist.", Utility.Text.GetFullName<T>(Name), typeof(T).FullName));
            }
            _CurrentState = state;
            _CurrentStateTime = 0;
            _CurrentState.OnEnter(this);
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (_CurrentState == null)
            {
                return;
            }
            _CurrentStateTime += elapseSeconds;
            _CurrentState.OnUpdate(this,elapseSeconds, realElapseSeconds);
        }
    }
}
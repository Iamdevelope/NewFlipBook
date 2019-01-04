using System;
using System.Collections.Generic;

namespace PJW.FSM
{
    /// <summary>
    /// 有限状态机接口
    /// </summary>
    /// <typeparam name="T">状态机类型</typeparam>
    public interface IFsm<T> where T : class
    {
        /// <summary>
        /// 获取状态机名字
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// 获取状态机持有者
        /// </summary>
        T Owner
        {
            get;
        }
        /// <summary>
        /// 获取状态机数量
        /// </summary>
        int FsmStateCount
        {
            get;
        }
        /// <summary>
        /// 获取该状态机是否正在运行
        /// </summary>
        bool IsRunning
        {
            get;
        }
        /// <summary>
        /// 获取该状态机是否被销毁了
        /// </summary>
        bool IsDestory
        {
            get;
        }
        /// <summary>
        /// 当前状态机状态
        /// </summary>
        FsmState<T> CurretState
        {
            get;
        }
        /// <summary>
        /// 当前状态机持续时间
        /// </summary>
        float CurrentStateTime
        {
            get;
        }
        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机的类型</typeparam>
        void Start<TState>() where TState : FsmState<TState>;
        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <param name="stateType">要开始的有限状态机的类型</param>
        void Start(Type stateType);
        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <typeparam name="TState">要检查的有限状态机的状态类型</typeparam>
        /// <returns></returns>
        bool HasState<TState>() where TState : FsmState<TState>;
        /// <summary>
        /// 是否存在有限状态机的状态
        /// </summary>
        /// <param name="stateType">要检查的有限状态机的状态类型</param>
        /// <returns></returns>
        bool HasState(Type stateType);
        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机的状态类型</typeparam>
        /// <returns></returns>
        TState GetState<TState>() where TState : FsmState<TState>;
        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <param name="stateType">要获取的有限状态机的状态类型</param>
        /// <returns></returns>
        FsmState<T> GetState(Type stateType);
        /// <summary>
        /// 得到有限状态机的所有状态
        /// </summary>
        /// <returns></returns>
        FsmState<T>[] GetAllStates();
        /// <summary>
        /// 得到有限状态机的所有状态
        /// </summary>
        /// <param name="results"></param>
        void GetAllStates(out List<FsmState<T>> results);
        /// <summary>
        /// 执行有限状态机事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="eventId">事件编号</param>
        void FireEvent(object sender, int eventId);
        /// <summary>
        /// 执行有限状态机事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="eventId">事件编号</param>
        /// <param name="userData">用户自定义数据</param>
        void FireEvent(object sender, int eventId, object userData);
        /// <summary>
        /// 判断是否存在指定有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <returns></returns>
        bool HasData(string name);
        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <typeparam name="TData">要获取的有限状态机的数据类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns></returns>
        TData GetData<TData>(string name) where TData : Variable.Variable;
        /// <summary>
        /// 获取有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <returns></returns>
        Variable.Variable GetData(string name);
        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <typeparam name="TData">要设置的数据类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <param name="data">有限状态机数据</param>
        void SetData<TData>(string name, TData data) where TData : Variable.Variable;
        /// <summary>
        /// 设置有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="data">有限状态机数据</param>
        void SetData(string name, Variable.Variable data);
        /// <summary>
        /// 移除有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否移除成功</returns>
        bool RemoveData(string name);
    }
}
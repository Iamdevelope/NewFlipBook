
using System;
using System.Collections.Generic;

namespace PJW.FSM
{
    /// <summary>
    /// 状态机管理接口
    /// </summary>
    public interface IFsmManager
    {
        /// <summary>
        /// 有限状态机数量
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否存在有限状态机</returns>
        bool HasFsm<T>() where T : class;
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>是否存在有限状态机</returns>
        bool HasFsm(Type ownerType);
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        bool HasFsm<T>(string name) where T : class;
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        bool HasFsm(Type ownerType, string name);
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>需要获取的有限状态机</returns>
        IFsm<T> GetFsm<T>() where T : class;
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>需要获取的有限状态机</returns>
        FsmBase GetFsm(Type ownerType);
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>有限状态机</returns>
        IFsm<T> GetFsm<T>(string name) where T : class;
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>需要获取的有限状态机</returns>
        FsmBase GetFsm(Type ownerType, string name);
        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <returns>所有有限状态机</returns>
        FsmBase[] GetAllFsm();
        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <param name="results">所有有限状态机</param>
        void GetAllFsm(List<FsmBase> results);
        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机包含的状态机</param>
        /// <returns>有限状态机</returns>
        IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class;
        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机包含的状态机</param>
        /// <returns>有限状态机</returns>
        IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class;
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        bool DestroyFsm<T>() where T : class;
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>是否销毁有限状态机成功</returns>
        bool DestroyFsm(Type ownerType);
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否销毁有限状态机成功</returns>
        bool DestroyFsm<T>(string name) where T : class;
        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">要销毁的有限状态机名称。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        bool DestroyFsm(Type ownerType, string name);
        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        bool DestroyFsm<T>(IFsm<T> fsm) where T : class;
        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        bool DestroyFsm(FsmBase fsm);
    }
}
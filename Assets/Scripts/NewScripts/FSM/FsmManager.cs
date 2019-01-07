
using System;
using System.Collections.Generic;

namespace PJW.FSM
{
    /// <summary>
    /// 有限状态机管理器
    /// </summary>
    public class FsmManager : FrameworkModule, IFsmManager
    {
        private readonly Dictionary<string, FsmBase> _Fsm;
        private readonly List<FsmBase> _TempFsm;

        public FsmManager()
        {
            _Fsm = new Dictionary<string, FsmBase>();
            _TempFsm = new List<FsmBase>();
        }
        /// <summary>
        /// 获取有限状态机模块优先级
        /// </summary>
        public override int Priority
        {
            get
            {
                return 60;
            }
        }
        /// <summary>
        /// 获取有限状态机数量
        /// </summary>
        public int Count
        {
            get
            {
                return _Fsm.Count;
            }
        }
        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机包含的状态机</param>
        /// <returns>有限状态机</returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }
        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机包含的状态机</param>
        /// <returns>有限状态机</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            if (HasFsm<T>(name))
            {
                throw new FrameworkException(Utility.Text.Format(" the {0} already exit ", Utility.Text.GetFullName<T>(name)));
            }
            Fsm<T> fsm = new Fsm<T>(name, owner, states);
            _Fsm.Add(Utility.Text.GetFullName<T>(name), fsm);
            return fsm;
        }
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>() where T : class
        {
            return InternalDestroyFsm(Utility.Text.GetFullName<T>(string.Empty));
        }
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new FrameworkException(" owner type is invalid ");
            }
            return InternalDestroyFsm(Utility.Text.GetFullName(ownerType, string.Empty));
        }
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否销毁有限状态机成功</returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            return InternalDestroyFsm(Utility.Text.GetFullName<T>(name));
        }
        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">要销毁的有限状态机名称。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new FrameworkException(" owner type is invalid ");
            }
            return InternalDestroyFsm(Utility.Text.GetFullName(ownerType, name));
        }
        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            if (fsm == null)
            {
                throw new FrameworkException(" fsm is invalid ");
            }
            return InternalDestroyFsm(Utility.Text.GetFullName<T>(fsm.Name));
        }
        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(FsmBase fsm)
        {
            if (fsm == null)
            {
                throw new FrameworkException(" fsm is invalid ");
            }
            return InternalDestroyFsm(Utility.Text.GetFullName(fsm.OwnerType,fsm.Name));
        }
        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <returns>所有有限状态机</returns>
        public FsmBase[] GetAllFsm()
        {
            int index = 0;
            FsmBase[] results = new FsmBase[_Fsm.Count];
            foreach (KeyValuePair<string,FsmBase> item in _Fsm)
            {
                results[index++] = item.Value;
            }
            return results;
        }
        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <param name="results">所有有限状态机</param>
        public void GetAllFsm(List<FsmBase> results)
        {
            if (results == null)
            {
                throw new FrameworkException(" results is invalid ");
            }
            results.Clear();
            foreach (KeyValuePair<string, FsmBase> item in _Fsm)
            {
                results.Add(item.Value);
            }
        }
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>需要获取的有限状态机</returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return (IFsm<T>)InternalGetFsm(Utility.Text.GetFullName<T>(string.Empty));
        }
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>需要获取的有限状态机</returns>
        public FsmBase GetFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new FrameworkException(" owner type is invalid ");
            }
            return InternalGetFsm(Utility.Text.GetFullName(ownerType, string.Empty));
        }
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>有限状态机</returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return (IFsm<T>)InternalGetFsm(Utility.Text.GetFullName<T>(name));
        }
        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>需要获取的有限状态机</returns>
        public FsmBase GetFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new FrameworkException(" owner type is invalid ");
            }
            return InternalGetFsm(Utility.Text.GetFullName(ownerType, name));
        }
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(Utility.Text.GetFullName<T>(string.Empty));
        }
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new FrameworkException(" owner type is invalid ");
            }
            return InternalHasFsm(Utility.Text.GetFullName(ownerType, string.Empty));
        }
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return InternalHasFsm(Utility.Text.GetFullName<T>(name));
        }
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        public bool HasFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new FrameworkException(" owner type is invalid ");
            }
            return InternalHasFsm(Utility.Text.GetFullName(ownerType, name));
        }
        /// <summary>
        /// 关闭并清理管理器
        /// </summary>
        public override void Shutdown()
        {
            foreach (KeyValuePair<string,FsmBase> item in _Fsm)
            {
                item.Value.ShutDown();
            }
            _Fsm.Clear();
            _TempFsm.Clear();
        }
        /// <summary>
        /// 管理器轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            _TempFsm.Clear();
            if(_Fsm.Count<=0)
            {
                return;
            }
            foreach (KeyValuePair<string,FsmBase> item in _Fsm)
            {
                _TempFsm.Add(item.Value);
            }
            foreach (FsmBase item in _TempFsm)
            {
                if (item.IsDestroyed)
                {
                    continue;
                }
                item.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="name">需要销毁的有限状态机名称</param>
        /// <returns>是否销毁成功</returns>
        private bool InternalDestroyFsm(string name)
        {
            FsmBase fsm = null;
            if (_Fsm.TryGetValue(name, out fsm))
            {
                fsm.ShutDown();
                return _Fsm.Remove(name);
            }
            return false;
        }
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="name">状态机名称</param>
        /// <returns>是否存在有限状态机</returns>
        private bool InternalHasFsm(string name)
        {
            return _Fsm.ContainsKey(name);
        }
        /// <summary>
        /// 获得有限状态机
        /// </summary>
        /// <param name="name">状态机名称</param>
        /// <returns>状态机</returns>
        private FsmBase InternalGetFsm(string name)
        {
            FsmBase fsm = null;
            if(_Fsm.TryGetValue(name,out fsm))
            {
                return fsm;
            }
            return null;
        }
    }
}
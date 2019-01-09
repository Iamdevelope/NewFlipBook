
using System;

namespace PJW.ObjectPool
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IObjectPool<T> where T : ObjectBase
    {
        /// <summary>
        /// 获取对象池名
        /// </summary>
        string GetName
        {
            get;
        }
        /// <summary>
        /// 获取对象池完整名
        /// </summary>
        string GetFullName
        {
            get;
        }
        /// <summary>
        /// 获取对象池中对象的个数
        /// </summary>
        int GetObjectCount
        {
            get;
        }
        /// <summary>
        /// 获取对象类型
        /// </summary>
        Type GetObjectType
        {
            get;
        }
        /// <summary>
        /// 获取对象池中可以被释放的对象个数
        /// </summary>
        int GetCanReleaseCount
        {
            get;
        }
        /// <summary>
        /// 获取对象是否允许被多次获取
        /// </summary>
        bool GetAllowMultiSpawn
        {
            get;
        }
        /// <summary>
        /// 获取或设置对象池自动释放可释放对象的间隔时间
        /// </summary>
        float AutoReleaseInterval
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置对象池容量
        /// </summary>
        int Capacity
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置对象池对象过期时间
        /// </summary>
        float ExpireTime
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置对象池优先级
        /// </summary>
        int Priority
        {
            get;
            set;
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="spawned">对象是否已经被获取</param>
        void CreateObject(T obj, bool spawned);
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <returns></returns>
        bool CanSpawn();
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="name">对象名</param>
        /// <returns></returns>
        bool CanSpawn(string name);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        T Spawn();
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="name">对象名</param>
        /// <returns></returns>
        T Spawn(string name);
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj">需要回收的内部对象</param>
        void Unspawn(T obj);
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="target">需要回收的对象</param>
        void Unspawn(object target);
        /// <summary>
        /// 设置获取对象优先级
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="priority">优先级</param>
        void SetPriority(T obj, int priority);
        /// <summary>
        /// 设置获取对象优先级
        /// </summary>
        /// <param name="target">对象</param>
        /// <param name="priority">优先级</param>
        void SetPriority(object target, int priority);
        /// <summary>
        /// 设置对象是否加锁
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="isLock">是否加锁</param>
        void SetLock(T obj, bool isLock);
        /// <summary>
        /// 设置对象是否加锁
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="isLock">是否加锁</param>
        void SetLock(object target, bool isLock);
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        void Release();
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        /// <param name="releaseCount">尝试释放的个数</param>
        void Release(int releaseCount);
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        /// <param name="releaseObjectFilterCallback">释放对象筛选函数</param>
        void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        /// <param name="releaseCount">尝试释放的个数</param>
        /// <param name="releaseObjectFilterCallback">释放对象筛选函数</param>
        void Release(int releaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);
        /// <summary>
        /// 释放所有未被使用的对象
        /// </summary>
        void ReleaseAllUnused();
    }
}
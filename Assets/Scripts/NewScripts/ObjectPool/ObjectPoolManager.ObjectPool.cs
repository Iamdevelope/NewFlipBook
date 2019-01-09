
using System;
using System.Collections.Generic;

namespace PJW.ObjectPool
{
    internal partial class ObjectPoolManager
    {
        /// <summary>
        /// 对象池
        /// </summary>
        /// <typeparam name="T">对象池类型</typeparam>
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private readonly LinkedList<Object<T>> _Objects;
            private readonly List<T> _CachedCanReleaseObjects;
            private readonly List<T> _CachedToReleaseObjects;
            private readonly bool _AllowMultiSpawn;
            private float _AutoReleaseInterval;
            private int _Capacity;
            private float _ExpireTime;
            private int _Priority;
            private float _AutoReleaseTime;

            /// <summary>
            /// 对象池实例
            /// </summary>
            /// <param name="name">对象池名</param>
            /// <param name="allowMultiSpawn">允许被多次获取</param>
            /// <param name="autoReleaseInterval">自动释放可释放对象的间隔时间</param>
            /// <param name="expireTime">对象池对象过期时间</param>
            /// <param name="priority">优先级</param>
            /// <param name="capacity">对象池容量</param>
            public ObjectPool(string name, bool allowMultiSpawn, float autoReleaseInterval, float expireTime, int priority, int capacity) : base(name)
            {
                _Objects = new LinkedList<Object<T>>();
                _CachedCanReleaseObjects = new List<T>();
                _CachedToReleaseObjects = new List<T>();
                _AllowMultiSpawn = allowMultiSpawn;
                _AutoReleaseInterval = autoReleaseInterval;
                _ExpireTime = expireTime;
                _Priority = priority;
                _Capacity = capacity;
                _AutoReleaseTime = 0f;
            }

            /// <summary>
            /// 获取对象池中对象的个数
            /// </summary>
            public override int GetObjectCount
            {
                get
                {
                    return _Objects.Count;
                }
            }
            /// <summary>
            /// 获取对象类型
            /// </summary>
            public override Type GetObjectType
            {
                get { return typeof(T); }
            }
            /// <summary>
            /// 获取对象池中可以被释放的对象个数
            /// </summary>
            public override int GetCanReleaseCount
            {
                get { return _CachedCanReleaseObjects.Count; }
            }
            /// <summary>
            /// 获取对象是否允许被多次获取
            /// </summary>
            public override bool GetAllowMultiSpawn
            {
                get { return _AllowMultiSpawn; }
            }
            /// <summary>
            /// 获取或设置对象池自动释放可释放对象的间隔时间
            /// </summary>
            public override float AutoReleaseInterval
            {
                get { return _AutoReleaseInterval; }
                set { _AutoReleaseInterval = value; }
            }
            /// <summary>
            /// 获取或设置对象池容量
            /// </summary>
            public override int Capacity
            {
                get { return _Capacity; }
                set
                {
                    if (value < 0)
                    {
                        throw new FrameworkException(" set capacity of value is invalid ");
                    }
                    if (_Capacity == value) { return; }
                    FrameworkLog.Debug(" object pool {0} of capacity form {1} to {2} ", Utility.Text.GetFullName<T>(GetName), _Capacity, value.ToString());
                    _Capacity = value;
                    Release();
                }
            }
            /// <summary>
            /// 获取或设置对象池对象过期时间
            /// </summary>
            public override float ExpireTime
            {
                get { throw new NotImplementedException(); }
                set
                {
                    if (value < 0)
                    {
                        throw new FrameworkException(" set expireTime of value is invalid ");
                    }
                    if (_ExpireTime == value) { return; }
                    FrameworkLog.Debug(" object pool {0} of expire time form {1} to {2} ", Utility.Text.GetFullName<T>(GetName), _ExpireTime, value.ToString());
                    _ExpireTime = value;
                    Release();
                }
            }
            /// <summary>
            /// 获取或设置对象池优先级
            /// </summary>
            public override int Priority
            {
                get { return _Priority; }
                set { _Priority = value; }
            }

            /// <summary>
            /// 判断对象是否存在
            /// </summary>
            /// <returns></returns>
            public bool CanSpawn()
            {
                return CanSpawn(string.Empty);
            }
            /// <summary>
            /// 判断对象是否存在
            /// </summary>
            /// <param name="name">对象名</param>
            /// <returns></returns>
            public bool CanSpawn(string name)
            {
                foreach (Object<T> item in _Objects)
                {
                    if (item.GetName != name)
                    {
                        continue;
                    }
                    if (!item.IsUsing || GetAllowMultiSpawn)
                    {
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// 创建对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="spawned">对象是否已经被获取</param>
            public void CreateObject(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new FrameworkException(" the object is invalid ");
                }
                FrameworkLog.Debug(spawned ? " object pool {0} is create and spawn {1} " : " object pool {0} is create {1} ", Utility.Text.GetFullName<T>(GetName), obj.GetName);
                _Objects.AddLast(new Object<T>(obj, spawned));
                Release();
            }
            /// <summary>
            /// 获取所有对象信息
            /// </summary>
            /// <returns></returns>
            public override ObjectInfo[] GetAllObjectInfos()
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            public override void Release()
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            /// <param name="releaseCount">尝试释放的个数</param>
            public override void Release(int releaseCount)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            /// <param name="releaseObjectFilterCallback">释放对象筛选函数</param>
            public void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            /// <param name="releaseCount">尝试释放的个数</param>
            /// <param name="releaseObjectFilterCallback">释放对象筛选函数</param>
            public void Release(int releaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 释放所有未被使用的对象
            /// </summary>
            public override void ReleaseAllUnused()
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 设置对象是否加锁
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="isLock">是否加锁</param>
            public void SetLock(T obj, bool isLock)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 设置对象是否加锁
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="isLock">是否加锁</param>
            public void SetLock(object target, bool isLock)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 设置获取对象优先级
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="priority">优先级</param>
            public void SetPriority(T obj, int priority)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 设置获取对象优先级
            /// </summary>
            /// <param name="target">对象</param>
            /// <param name="priority">优先级</param>
            public void SetPriority(object target, int priority)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <returns></returns>
            public T Spawn()
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <param name="name">对象名</param>
            /// <returns></returns>
            public T Spawn(string name)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="obj">需要回收的内部对象</param>
            public void Unspawn(T obj)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="target">需要回收的对象</param>
            public void Unspawn(object target)
            {
                throw new NotImplementedException();
            }

            internal override void Shutdown()
            {
                throw new NotImplementedException();
            }

            internal override void Update(float elapseSeconds, float realElapseSeconds)
            {
                throw new NotImplementedException();
            }
        }
    }
}
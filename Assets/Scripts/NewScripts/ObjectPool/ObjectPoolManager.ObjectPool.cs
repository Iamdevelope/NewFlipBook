
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
                get { return _ExpireTime; }
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
                FrameworkLog.Debug(spawned ? " object pool '{0}' is create and spawn '{1}' " : " object pool '{0}' is create '{1}' ", Utility.Text.GetFullName<T>(GetName), obj.GetName);
                _Objects.AddLast(new Object<T>(obj, spawned));
                Release();
            }
            /// <summary>
            /// 获取所有对象信息
            /// </summary>
            /// <returns></returns>
            public override ObjectInfo[] GetAllObjectInfos()
            {
                int index = 0;
                ObjectInfo[] objectInfos = new ObjectInfo[_Objects.Count];
                foreach (var item in _Objects)
                {
                    objectInfos[index++] = new ObjectInfo(item.GetName, item.Priority, item.Locked, item.CustomCanReleaseFlag, item.LastUseTime, item.SpawnCount);
                }
                return objectInfos;
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            public override void Release()
            {
                Release(_Objects.Count - _Capacity, DefaultReleaseObjectFilterCallback);
            }

            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            /// <param name="releaseCount">尝试释放的个数</param>
            public override void Release(int releaseCount)
            {
                Release(releaseCount, DefaultReleaseObjectFilterCallback);
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            /// <param name="releaseObjectFilterCallback">释放对象筛选函数</param>
            public void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                Release(_Objects.Count - _Capacity, releaseObjectFilterCallback);
            }
            /// <summary>
            /// 释放对象池中可释放对象
            /// </summary>
            /// <param name="releaseCount">尝试释放的个数</param>
            /// <param name="releaseObjectFilterCallback">释放对象筛选函数</param>
            public void Release(int releaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                if (releaseObjectFilterCallback == null)
                {
                    throw new FrameworkException(" release object filter call back is invalid ");
                }
                FrameworkLog.Debug(" object is released ");
                _AutoReleaseTime = 0;
                if (releaseCount < 0)
                {
                    releaseCount = 0;
                }
                DateTime expireTime = DateTime.MinValue;
                if (_ExpireTime < float.MaxValue)
                {
                    expireTime = DateTime.Now.AddSeconds(-_ExpireTime);
                }
                GetCanReleaseObjects(_CachedCanReleaseObjects);
                List<T> toReleaseObjects = releaseObjectFilterCallback(_CachedCanReleaseObjects, releaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count <= 0)
                {
                    return;
                }
                foreach (ObjectBase toReleaseObject in toReleaseObjects)
                {
                    if (toReleaseObject == null)
                    {
                        throw new FrameworkException(" release object is not found ");
                    }
                    bool found = false;
                    foreach (Object<T> obj in _Objects)
                    {
                        if (obj.Peek() != toReleaseObject)
                        {
                            continue;
                        }
                        _Objects.Remove(obj);
                        obj.Release(false);
                        FrameworkLog.Debug(" Object pool '{0}' of object '{1}' release ", GetFullName, obj.GetName);
                        found = true;
                        break;
                    }
                    if (!found)
                    {
                        throw new FrameworkException(" can not release because not found ");
                    }
                }
            }
            /// <summary>
            /// 释放所有未被使用的对象
            /// </summary>
            public override void ReleaseAllUnused()
            {
                LinkedListNode<Object<T>> current = _Objects.First;
                while (current != null)
                {
                    if (current.Value.IsUsing || current.Value.Locked || !current.Value.CustomCanReleaseFlag)
                    {
                        current = current.Next;
                        continue;
                    }
                    LinkedListNode<Object<T>> next = current.Next;
                    _Objects.Remove(current);
                    current.Value.Release(false);
                    FrameworkLog.Debug(" Object pool '{0}' of object '{1}' release ", Utility.Text.GetFullName<T>(GetName), current.Value.GetName);
                    current = next;
                }
            }
            /// <summary>
            /// 设置对象是否加锁
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="isLock">是否加锁</param>
            public void SetLock(T obj, bool isLock)
            {
                if (obj == null)
                {
                    throw new FrameworkException(" object is invalid ");
                }
                SetLock(obj.GetTarget, isLock);
            }
            /// <summary>
            /// 设置对象是否加锁
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="isLock">是否加锁</param>
            public void SetLock(object target, bool isLock)
            {
                if (target == null)
                {
                    throw new FrameworkException(" The target is invalid in set lock ");
                }
                foreach (var item in _Objects)
                {
                    if (item.Peek().GetTarget == target)
                    {
                        item.Locked = isLock;
                        FrameworkLog.Debug(" Object pool {0} set lock {1} to {2} ", GetFullName, item.GetName, isLock);
                        return;
                    }
                }
                throw new FrameworkException(" not found target in object pool " + GetFullName);
            }
            /// <summary>
            /// 设置获取对象优先级
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="priority">优先级</param>
            public void SetPriority(T obj, int priority)
            {
                if (obj == null)
                {
                    throw new FrameworkException(" object is invalid ");
                }
                SetPriority(obj.GetTarget, priority);
            }
            /// <summary>
            /// 设置获取对象优先级
            /// </summary>
            /// <param name="target">对象</param>
            /// <param name="priority">优先级</param>
            public void SetPriority(object target, int priority)
            {
                if (target == null)
                {
                    throw new FrameworkException(" The target is invalid in set priority ");
                }
                foreach (var item in _Objects)
                {
                    if (item.Peek().GetTarget == target)
                    {
                        item.Peek().Priority = priority;
                        FrameworkLog.Debug(" object pool {0} set priority {1} to {2} ", Utility.Text.GetFullName<T>(GetName), item.GetName, priority.ToString());
                        return;
                    }
                }
                throw new FrameworkException(Utility.Text.Format(" not found target in object pool {0}", Utility.Text.GetFullName<T>(GetName)));
            }
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <returns></returns>
            public T Spawn()
            {
                return Spawn(string.Empty);
            }
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <param name="name">对象名</param>
            /// <returns></returns>
            public T Spawn(string name)
            {
                foreach (Object<T> item in _Objects)
                {
                    if (item.GetName != name)
                    {
                        continue;
                    }
                    if (!item.IsUsing || GetAllowMultiSpawn)
                    {
                        FrameworkLog.Debug(" object pool '{0}' spawn '{1}' ", Utility.Text.GetFullName<T>(name), item.Peek().GetName);
                        return item.Spawn();
                    }
                }
                return null;
            }
            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="obj">需要回收的内部对象</param>
            public void Unspawn(T obj)
            {
                if (obj == null)
                {
                    throw new FrameworkException(" Object is invalid ");
                }
                Unspawn(obj.GetTarget);
            }
            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="target">需要回收的对象</param>
            public void Unspawn(object target)
            {
                if (target == null)
                {
                    throw new FrameworkException(" The target is invalid ");
                }
                foreach (Object<T> item in _Objects)
                {
                    if (item.Peek().GetTarget == target)
                    {
                        FrameworkLog.Debug(" Object pool '{0}' Unspawn '{1}' ", Utility.Text.GetFullName<T>(GetName), item.Peek().GetName);
                        item.Unspawn();
                        Release();
                        return;
                    }
                }
                throw new FrameworkException(Utility.Text.Format(" the target '{0} ' can not find in object pool ", target.ToString()));
            }
            /// <summary>
            /// 关闭并清理对象池
            /// </summary>
            internal override void Shutdown()
            {
                LinkedListNode<Object<T>> current = _Objects.First;
                while (current != null)
                {
                    LinkedListNode<Object<T>> next = current.Next;
                    _Objects.Remove(current);
                    current.Value.Release(true);
                    FrameworkLog.Debug(" Object pool '{0}' of object '{1}' release ", Utility.Text.GetFullName<T>(GetName), current.Value.GetName);
                    current = next;
                }
            }

            internal override void Update(float elapseSeconds, float realElapseSeconds)
            {
                _AutoReleaseTime += realElapseSeconds;
                if (_AutoReleaseTime < _AutoReleaseInterval)
                {
                    return;
                }
                FrameworkLog.Debug(" Object pool '{0}' auto release start ", Utility.Text.GetFullName<T>(GetName));
                Release();
                FrameworkLog.Debug(" Object pool '{0}' auto release complete ", Utility.Text.GetFullName<T>(GetName));
            }

            /// <summary>
            /// 获取可以被释放的对象集合
            /// </summary>
            /// <param name="results"></param>
            private void GetCanReleaseObjects(List<T> results)
            {
                if (results == null)
                {
                    throw new FrameworkException(" the results is invalid ");
                }
                results.Clear();
                foreach (Object<T> item in _Objects)
                {
                    if (item.IsUsing || item.Locked || !item.CustomCanReleaseFlag)
                    {
                        continue;
                    }
                    results.Add(item.Peek());
                }
            }

            /// <summary>
            /// 默认释放对象筛选函数，当没有给定筛选时使用
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="candidateObjects">要筛选的对象集合</param>
            /// <param name="toReleaseCount">需要释放的对象个数</param>
            /// <param name="expireTime">对象过期参考时间</param>
            /// <returns>筛选对象集合</returns>
            private List<T> DefaultReleaseObjectFilterCallback(List<T> candidateObjects, int toReleaseCount, DateTime expireTime)
            {
                _CachedToReleaseObjects.Clear();
                if (expireTime > DateTime.MinValue)
                {
                    for (int i = candidateObjects.Count - 1; i >= 0; i--)
                    {
                        if (candidateObjects[i].LastUsedTime < expireTime)
                        {
                            _CachedToReleaseObjects.Add(candidateObjects[i]);
                            candidateObjects.RemoveAt(i);
                        }
                    }
                    toReleaseCount -= _CachedToReleaseObjects.Count;
                }
                //如果需要释放的对象个数还大于0，再对需要筛选的集合进行重排序，将优先级越低且越久没有被使用的对象也放置到筛选对象集合中
                for (int i = 0; toReleaseCount > 0 && i < candidateObjects.Count; i++)
                {
                    for (int j = i + 1; j < candidateObjects.Count; j++)
                    {
                        if (candidateObjects[i].Priority >= candidateObjects[j].Priority && candidateObjects[i].LastUsedTime > candidateObjects[j].LastUsedTime)
                        {
                            T temp = candidateObjects[i];
                            candidateObjects[i] = candidateObjects[j];
                            candidateObjects[j] = temp;
                        }
                    }
                    _CachedToReleaseObjects.Add(candidateObjects[i]);
                    toReleaseCount--;
                }
                return _CachedToReleaseObjects;
            }
        }
    }
}
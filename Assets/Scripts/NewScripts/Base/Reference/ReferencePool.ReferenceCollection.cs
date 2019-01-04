using System;
using System.Collections.Generic;

namespace PJW
{
    public static partial class ReferencePool
    {
        /// <summary>
        /// 引用容器
        /// </summary>
        public sealed class ReferenceCollection
        {
            private readonly Queue<IReference> _References;
            private readonly Type _ReferenceType;
            private int _UsingReferenceCount;
            private int _AcquireReferenceCount;
            private int _ReleaseReferenceCount;
            private int _AddReferenceCount;
            private int _RemoveReferenceCount;

            public ReferenceCollection(Type referenceType)
            {
                _References = new Queue<IReference>();
                _ReferenceType = referenceType;
                _UsingReferenceCount = 0;
                _AcquireReferenceCount = 0;
                _AddReferenceCount = 0;
                _ReleaseReferenceCount = 0;
                _RemoveReferenceCount = 0;
            }
            /// <summary>
            /// 获取引用类型
            /// </summary>
            public Type GetReferenceType
            {
                get { return _ReferenceType; }
            }
            /// <summary>
            /// 获取还未被使用的引用数量
            /// </summary>
            public int GetUnusedReferenceCount
            {
                get { return _References.Count; }
            }
            /// <summary>
            /// 获取正在使用的引用数量
            /// </summary>
            public int GetUsingReferenceCount
            {
                get { return _UsingReferenceCount; }
            }
            /// <summary>
            /// 获取引用数量
            /// </summary>
            public int GetAcquireReferenceCount
            {
                get { return _AcquireReferenceCount; }
            }
            /// <summary>
            /// 获取被释放的引用数量
            /// </summary>
            public int GetReleaseReferenceCount
            {
                get { return _ReleaseReferenceCount; }
            }
            /// <summary>
            /// 获取需要添加的引用数量
            /// </summary>
            public int GetAddReferenceCount
            {
                get { return _AddReferenceCount; }
            }
            /// <summary>
            /// 获取移除的引用数量
            /// </summary>
            public int GetRemoveReferenceCount
            {
                get { return _RemoveReferenceCount; }
            }
            /// <summary>
            /// 获取指定类型的引用
            /// </summary>
            /// <typeparam name="T">对应类型</typeparam>
            /// <returns></returns>
            public T Acquire<T>() where T : class, IReference, new()
            {
                if (typeof(T) != _ReferenceType)
                {
                    throw new FrameworkException(" Type is invalid ");
                }
                _UsingReferenceCount++;
                _AcquireReferenceCount++;
                lock (_References)
                {
                    if (_References.Count > 0)
                    {
                        return (T)_References.Dequeue();
                    }
                }
                _AddReferenceCount++;
                return new T();
            }
            /// <summary>
            /// 获取引用
            /// </summary>
            /// <returns></returns>
            public IReference Acquire()
            {
                _UsingReferenceCount++;
                _AcquireReferenceCount++;
                lock (_References)
                {
                    if (_References.Count > 0)
                    {
                        return _References.Dequeue();
                    }
                }
                _AddReferenceCount++;
                return (IReference)Activator.CreateInstance(_ReferenceType);
            }
            /// <summary>
            /// 释放引用
            /// </summary>
            /// <param name="reference">需要释放的引用</param>
            public void Release(IReference reference)
            {
                reference.Clear();
                lock (_References)
                {
                    if (_References.Contains(reference))
                    {
                        throw new FrameworkException(" the reference has been released ");
                    }
                    _References.Enqueue(reference);
                }
                _ReleaseReferenceCount++;
                _UsingReferenceCount--;
            }
            /// <summary>
            /// 添加引用
            /// </summary>
            /// <typeparam name="T">需要添加的引用类型</typeparam>
            /// <param name="count">需要添加的引用个数</param>
            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != _ReferenceType)
                {
                    throw new FrameworkException(" type is invalid ");
                }
                lock (_References)
                {
                    _AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        _References.Enqueue(new T());
                    }
                }
            }
            /// <summary>
            /// 添加引用
            /// </summary>
            /// <param name="count">需要添加的个数</param>
            public void Add(int count)
            {
                lock (_References)
                {
                    _AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        _References.Enqueue((IReference)Activator.CreateInstance(_ReferenceType));
                    }
                }
            }
            /// <summary>
            /// 移除引用
            /// </summary>
            /// <param name="count">移除多少个引用</param>
            public void Remove(int count)
            {
                lock (_References)
                {
                    if (count > _References.Count)
                    {
                        count = _References.Count;
                    }
                    _RemoveReferenceCount += count;
                    while (count-- > 0)
                    {
                        _References.Dequeue();
                    }
                }
            }
            /// <summary>
            /// 移除全部引用
            /// </summary>
            public void RemoveAll()
            {
                lock (_References)
                {
                    _RemoveReferenceCount += _References.Count;
                    _References.Clear();
                }
            }
        }
    }
}
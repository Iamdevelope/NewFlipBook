
using System;
using System.Collections.Generic;

namespace PJW
{
    /// <summary>
    /// 引用池
    /// </summary>
    public static partial class ReferencePool
    {
        public static Dictionary<string, ReferenceCollection> _References = new Dictionary<string, ReferenceCollection>();
        public static int GetReferenceCounts
        {
            get { return _References.Count; }
        }
        /// <summary>
        /// 获取所有引用池的信息
        /// </summary>
        /// <returns></returns>
        public static ReferencePoolInfo[] GetAllReferencePoolInfo()
        {
            int index = 0;
            ReferencePoolInfo[] referencePoolInfos = null;
            lock (_References)
            {
                referencePoolInfos = new ReferencePoolInfo[_References.Count];
                foreach (KeyValuePair<string,ReferenceCollection> item in _References)
                {
                    referencePoolInfos[index++] = new ReferencePoolInfo(item.Key, item.Value.GetUnusedReferenceCount, item.Value.GetUsingReferenceCount,
                        item.Value.GetAcquireReferenceCount, item.Value.GetReleaseReferenceCount, item.Value.GetAddReferenceCount, item.Value.GetRemoveReferenceCount);
                }
            }
            return referencePoolInfos;
        }
        /// <summary>
        /// 清除所有引用池
        /// </summary>
        public static void ClearAll()
        {
            lock (_References)
            {
                foreach (KeyValuePair<string, ReferenceCollection> item in _References)
                {
                    item.Value.RemoveAll();
                }
                _References.Clear();
            }
        }
        /// <summary>
        /// 从引用池中获取引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <returns></returns>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }
        /// <summary>
        /// 从引用池中获取引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <returns></returns>
        public static IReference Acquire(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            return GetReferenceCollection(referenceType).Acquire();
        }
        /// <summary>
        /// 将引用归还引用池
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="reference">引用</param>
        public static void Release<T>(T reference) where T : class, IReference
        {
            if (reference == null)
            {
                throw new FrameworkException(" reference is invalid ");
            }
            GetReferenceCollection(typeof(T)).Release(reference);
        }
        /// <summary>
        /// 将引用归还引用池
        /// </summary>
        /// <param name="reference">引用</param>
        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                throw new FrameworkException(" reference is invalid ");
            }
            Type referenceType = reference.GetType();
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Release(reference);
        }
        /// <summary>
        /// 向引用池中增加指定数量的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="count">增加的数量</param>
        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }
        /// <summary>
        /// 向引用池中增加指定数量的引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <param name="count">增加数量</param>
        public static void Add(Type referenceType,int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Add(count);
        }
        /// <summary>
        /// 向引用池中移除指定数量的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="count">需要移除的引用数量</param>
        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }
        /// <summary>
        /// 向引用池中移除指定数量的引用
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <param name="count">需要移除的引用数量</param>
        public static void Remove(Type referenceType,int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Remove(count);
        }
        /// <summary>
        /// 移除引用池中所有的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }
        /// <summary>
        /// 移除引用池中的所有引用
        /// </summary>
        /// <param name="referenceType"></param>
        public static void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).RemoveAll();
        }
        /// <summary>
        /// 检查引用类型是否正确
        /// </summary>
        /// <param name="referenceType"></param>
        private static void InternalCheckReferenceType(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new FrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new FrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new FrameworkException(Utility.Text.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }
        }
        /// <summary>
        /// 通过引用类型得到引用容器
        /// </summary>
        /// <param name="referenceType">引用类型</param>
        /// <returns>得到的引用容器</returns>
        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new FrameworkException(" Reference is invalid ");
            }
            string fullName = referenceType.FullName;
            ReferenceCollection referenceCollection = null;
            lock (_References)
            {
                if(!_References.TryGetValue(fullName,out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    _References.Add(fullName, referenceCollection);
                }
            }
            return referenceCollection;
        }
    }
}
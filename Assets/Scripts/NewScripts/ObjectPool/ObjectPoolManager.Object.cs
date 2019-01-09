
using System;

namespace PJW.ObjectPool
{
    internal partial class ObjectPoolManager
    {
        /// <summary>
        /// 内部对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        private sealed class Object<T> where T : ObjectBase
        {
            private readonly T _Object;
            private int _SpawnCount;

            /// <summary>
            /// 初始化对象实例
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="spwaned">对象是否已经被获取</param>
            public Object(T obj,bool spwaned)
            {
                if (obj == null)
                {
                    throw new FrameworkException(" object is invalid ");
                }
                _Object = obj;
                _SpawnCount = spwaned ? 1 : 0;
                if (spwaned)
                {
                    _Object.OnSpawn();
                }
            }
            /// <summary>
            /// 对象的名字
            /// </summary>
            public string GetName
            {
                get { return _Object.GetName; }
            }
            /// <summary>
            /// 对象是否被加锁
            /// </summary>
            public bool Locked
            {
                get { return _Object.IsLock; }
                internal set { _Object.IsLock = value; }
            }
            /// <summary>
            /// 对象的优先级
            /// </summary>
            public int Priority
            {
                get { return _Object.Priority; }
                set { _Object.Priority = value; }
            }
            /// <summary>
            /// 对象最后一次被使用的时间
            /// </summary>
            public DateTime LastUseTime
            {
                get { return _Object.LastUsedTime; }
                set { _Object.LastUsedTime = value; }
            }
            /// <summary>
            /// 获取自定义释放检查标记
            /// </summary>
            public bool CustomCanReleaseFlag
            {
                get { return _Object.CustomCanReleaseFlag; }
            }
            /// <summary>
            /// 对象是否正在使用
            /// </summary>
            public bool IsUsing
            {
                get { return _SpawnCount > 0; }
            }
            /// <summary>
            /// 获取对象被使用次数
            /// </summary>
            public int SpawnCount
            {
                get { return _SpawnCount; }
            }
            /// <summary>
            /// 查看对象
            /// </summary>
            public T Peek()
            {
                return _Object;
            }
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <returns></returns>
            public T Spawn()
            {
                _SpawnCount++;
                _Object.LastUsedTime = DateTime.Now;
                _Object.OnSpawn();
                return _Object;
            }
            /// <summary>
            /// 回收对象
            /// </summary>
            public void Unspawn()
            {
                _Object.UnSpawn();
                _Object.LastUsedTime = DateTime.Now;
                _SpawnCount--;
                if (_SpawnCount < 0)
                {
                    throw new FrameworkException(" the object has not reference ");
                }
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="isShutdown">是否是关闭对象时释放</param>
            public void Release(bool isShutdown)
            {
                _Object.Release(isShutdown);
            }
        }
    }
}
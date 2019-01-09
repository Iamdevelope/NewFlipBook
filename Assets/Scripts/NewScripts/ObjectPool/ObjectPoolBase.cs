
using System;

namespace PJW.ObjectPool
{
    /// <summary>
    /// 对象池基类
    /// </summary>
    public abstract class ObjectPoolBase
    {
        private readonly string _Name;
        /// <summary>
        /// 获取对象池名
        /// </summary>
        public string GetName
        {
            get { return _Name; }
        }
        /// <summary>
        /// 获取对象池完整名
        /// </summary>
        public string GetFullName
        {
            get { return Utility.Text.GetFullName(GetObjectType, GetName); }
        }
        /// <summary>
        /// 获取对象池中对象的个数
        /// </summary>
        public abstract int GetObjectCount
        {
            get;
        }
        /// <summary>
        /// 获取对象类型
        /// </summary>
        public abstract Type GetObjectType
        {
            get;
        }
        /// <summary>
        /// 获取对象池中可以被释放的对象个数
        /// </summary>
        public abstract int GetCanReleaseCount
        {
            get;
        }
        /// <summary>
        /// 获取对象是否允许被多次获取
        /// </summary>
        public abstract bool GetAllowMultiSpawn
        {
            get;
        }
        /// <summary>
        /// 获取或设置对象池自动释放可释放对象的间隔时间
        /// </summary>
        public abstract float AutoReleaseInterval
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置对象池容量
        /// </summary>
        public abstract int Capacity
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置对象池对象过期时间
        /// </summary>
        public abstract float ExpireTime
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置对象池优先级
        /// </summary>
        public abstract int Priority
        {
            get;
            set;
        }

        public ObjectPoolBase() : this(null) { }
        public ObjectPoolBase(string name)
        {
            _Name = name ?? string.Empty;
        }

        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        public abstract void Release();
        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        /// <param name="releaseCount">尝试释放的个数</param>
        public abstract void Release(int releaseCount);
        /// <summary>
        /// 释放所有未被使用的对象
        /// </summary>
        public abstract void ReleaseAllUnused();
        /// <summary>
        /// 获取所有对象信息
        /// </summary>
        /// <returns></returns>
        public abstract ObjectInfo[] GetAllObjectInfos();
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);
        internal abstract void Shutdown();
    }
}
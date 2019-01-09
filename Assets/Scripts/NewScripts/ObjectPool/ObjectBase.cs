
using System;

namespace PJW.ObjectPool
{
    /// <summary>
    /// 对象基类
    /// </summary>
    public abstract class ObjectBase
    {
        private readonly string _Name;
        private readonly object _Target;
        private int _Priority;
        private bool _IsLock;
        private DateTime _LastTime;

        /// <summary>
        /// 对象基类构造函数
        /// </summary>
        /// <param name="target">对象</param>
        public ObjectBase(object target) : this(null,target,0,false)
        {

        }
        /// <summary>
        /// 对象基类构造函数
        /// </summary>
        /// <param name="name">对象的名字</param>
        /// <param name="target">对象</param>
        public ObjectBase(string name,object target) : this(name, target, 0, false)
        {

        }
        /// <summary>
        /// 对象基类构造函数
        /// </summary>
        /// <param name="name">对象的名字</param>
        /// <param name="target">对象</param>
        /// <param name="priority">优先级</param>
        public ObjectBase(string name,object target,int priority) : this(name, target, priority, false)
        {

        }
        /// <summary>
        /// 对象基类构造函数
        /// </summary>
        /// <param name="name">对象的名字</param>
        /// <param name="target">对象</param>
        /// <param name="isLock">是否被加锁</param>
        public ObjectBase(string name,object target,bool isLock) : this(name, target, 0, isLock)
        {

        }
        /// <summary>
        /// 对象基类构造函数
        /// </summary>
        /// <param name="name">对象的名字</param>
        /// <param name="target">对象</param>
        /// <param name="priority">优先级</param>
        /// <param name="isLock">是否被加锁</param>
        public ObjectBase(string name,object target,int priority,bool isLock)
        {
            if (target == null)
            {
                throw new FrameworkException(" Target relence is null ");
            }
            _Name = name;
            _Target = target;
            _Priority = priority;
            _IsLock = isLock;
            _LastTime = DateTime.Now;
        }

        /// <summary>
        /// 获取对象名
        /// </summary>
        public string GetName
        {
            get { return _Name; }
        }
        /// <summary>
        /// 获取对象引用
        /// </summary>
        public object GetTarget
        {
            get { return _Target; }
        }
        /// <summary>
        /// 获取对象优先级
        /// </summary>
        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }
        /// <summary>
        /// 获取对象是否被加锁
        /// </summary>
        public bool IsLock
        {
            get { return _IsLock; }
            set { _IsLock = value; }
        }
        /// <summary>
        /// 获取对象最后一次被使用时间
        /// </summary>
        public DateTime LastUsedTime
        {
            get { return _LastTime; }
            internal set { _LastTime = value; }
        }
        /// <summary>
        /// 获取自定义释放检查标记。
        /// </summary>
        public virtual bool CustomCanReleaseFlag
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 对象被创建时
        /// </summary>
        protected internal virtual void OnSpawn() { }
        /// <summary>
        /// 对象被回收时
        /// </summary>
        protected internal virtual void UnSpawn() { }
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="isShutdown">是否是关闭对象时释放</param>
        protected internal abstract void Release(bool isShutdown);
    }
}
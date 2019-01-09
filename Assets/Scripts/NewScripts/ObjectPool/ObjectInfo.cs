
using System;

namespace PJW.ObjectPool
{
    /// <summary>
    /// 对象信息
    /// </summary>
    public class ObjectInfo
    {
        private readonly string _Name;
        private readonly int _Priority;
        private readonly bool _IsLock;
        private readonly bool _CustomCanReleaseFlag;
        private readonly DateTime _LastTime;
        private readonly int _SpawnCount;

        /// <summary>
        /// 初始化对象信息
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="priority">对象优先级</param>
        /// <param name="isLock">对象是否被加锁</param>
        /// <param name="customCanReleaseFlag">对象自定义释放检查标记</param>
        /// <param name="lastUseTime">对象上次使用时间</param>
        /// <param name="spawnCount">对象获取次数</param>
        public ObjectInfo(string name,int priority,bool isLock,bool customCanReleaseFlag,DateTime lastUseTime,int spawnCount)
        {
            _Name = name;
            _Priority = priority;
            _IsLock = isLock;
            _CustomCanReleaseFlag = customCanReleaseFlag;
            _LastTime = lastUseTime;
            _SpawnCount = spawnCount;
        }
        /// <summary>
        /// 获取对象名
        /// </summary>
        public string GetName
        {
            get { return _Name; }
        }
        /// <summary>
        /// 获取对象优先级
        /// </summary>
        public int GetPriority
        {
            get { return _Priority; }
        }
        /// <summary>
        /// 获取对象是否被加锁
        /// </summary>
        public bool GetIsLock
        {
            get { return _IsLock; }
        }
        /// <summary>
        /// 获取对象自定义释放检查标记
        /// </summary>
        public bool GetCustomCanReleaseFlag
        {
            get { return _CustomCanReleaseFlag; }
        }
        /// <summary>
        /// 获取对象上一次使用时间
        /// </summary>
        public DateTime GetLastUseTime
        {
            get { return _LastTime; }
        }
        /// <summary>
        /// 获取对象是否正在被使用
        /// </summary>
        public bool GetIsUsing
        {
            get { return _SpawnCount > 0; }
        }
        /// <summary>
        /// 获取对象被获取次数
        /// </summary>
        public int GetSpawnCount
        {
            get { return _SpawnCount; }
        }
    }
}
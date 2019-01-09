
using System;
using System.Collections.Generic;

namespace PJW.ObjectPool
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    internal sealed partial class ObjectPoolManager : FrameworkModule, IObjectPoolManager
    {
        private const int _DefaultCapacity = int.MaxValue;
        private const float _DefaultExpireTime = float.MaxValue;
        private const int _DefaultPriority = 0;

        private Dictionary<string, ObjectPoolBase> _ObjectPools;

        public ObjectPoolManager()
        {
            _ObjectPools = new Dictionary<string, ObjectPoolBase>();
        }

        public override void Shutdown()
        {
            foreach (KeyValuePair<string, ObjectPoolBase> item in _ObjectPools)
            {
                item.Value.Shutdown();
            }
            _ObjectPools.Clear();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<string, ObjectPoolBase> item in _ObjectPools)
            {
                item.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        public override int Priority
        {
            get
            {
                return 90;
            }
        }

        /// <summary>
        /// 获取对象池数量
        /// </summary>
        public int GetObjectPoolCount
        {
            get { return _ObjectPools.Count; }
        }

        /// <summary>
        /// 判断是否存在对象池
        /// </summary>
        /// <typeparam name="T">需要检查的对象类型</typeparam>
        /// <returns>是否存在对象池</returns>
        public bool IsExitObjectPool<T>() where T : ObjectBase
        {
            return IsExitObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 判断是否存在对象池
        /// </summary>
        /// <param name="objectType">需要检查的对象类型</param>
        /// <returns>是否存在对象池</returns>
        public bool IsExitObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new FrameworkException(" the object type is invalid ");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new FrameworkException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return IsExitObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }

        /// <summary>
        /// 判断是否存在对象池
        /// </summary>
        /// <typeparam name="T">需要检查的对象类型</typeparam>
        /// <param name="name">需要检查的对象池名</param>
        /// <returns>是否存在对象池</returns>
        public bool IsExitObjectPool<T>(string name) where T : ObjectBase
        {
            return IsExitObjectPool(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 判断是否存在对象池
        /// </summary>
        /// <param name="objectType">需要检查的对象类型</param>
        /// <param name="name">需要检查的对象池名</param>
        /// <returns>是否存在对象池</returns>
        public bool IsExitObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new FrameworkException(" the object type is invalid ");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new FrameworkException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return IsExitObjectPool(Utility.Text.GetFullName(objectType, name));
        }

        /// <summary>
        /// 判断是否存在对象池
        /// </summary>
        /// <param name="fullName">需要检查的对象池全名</param>
        /// <returns>是否存在对象池</returns>
        public bool IsExitObjectPool(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new FrameworkException(" the objectpool of name is invalid ");
            }
            return _ObjectPools.ContainsKey(fullName);
        }

        /// <summary>
        /// /判断是否存在对象池
        /// </summary>
        /// <param name="condition">要检查的条件</param>
        /// <returns>是否存在对象池</returns>
        public bool IsExitObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new FrameworkException(" the condition is invalid ");
            }
            foreach (KeyValuePair<string,ObjectPoolBase> item in _ObjectPools)
            {
                if (condition(item.Value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象池</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return (IObjectPool<T>)GetObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>对象池</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new FrameworkException(" the object type is invalid ");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new FrameworkException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return GetObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象池名</param>
        /// <returns>对象池</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return (IObjectPool<T>)GetObjectPool(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名</param>
        /// <returns>对象池</returns>
        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new FrameworkException(" the object type is invalid ");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new FrameworkException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return GetObjectPool(Utility.Text.GetFullName(objectType, name));
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="fullName">对象池全名</param>
        /// <returns>对象池</returns>
        public ObjectPoolBase GetObjectPool(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new FrameworkException(" the objectpool of name is invalid ");
            }
            ObjectPoolBase temp = null;
            if (IsExitObjectPool(fullName))
            {
                if(_ObjectPools.TryGetValue(fullName,out temp))
                {
                    return temp;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="condition">要检查的条件</param>
        /// <returns>对象池</returns>
        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new FrameworkException(" the condition is invalid ");
            }
            foreach (KeyValuePair<string, ObjectPoolBase> item in _ObjectPools)
            {
                if (condition(item.Value))
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取满足条件的所有对象池
        /// </summary>
        /// <param name="condition">要检查的条件</param>
        /// <returns>满足条件的对象池</returns>
        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new FrameworkException(" the condition is invalid ");
            }
            List<ObjectPoolBase> objectPoolBases = new List<ObjectPoolBase>();
            foreach (KeyValuePair<string,ObjectPoolBase> item in _ObjectPools)
            {
                if (condition(item.Value))
                {
                    objectPoolBases.Add(item.Value);
                }
            }
            return objectPoolBases.ToArray();
        }

        /// <summary>
        /// 获取满足条件的所有对象池
        /// </summary>
        /// <param name="condition">要检查的条件</param>
        /// <param name="results">满足条件的对象池</param>
        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            if (condition == null)
            {
                throw new FrameworkException(" the condition is invalid ");
            }
            results.Clear();
            foreach (KeyValuePair<string, ObjectPoolBase> item in _ObjectPools)
            {
                if (condition(item.Value))
                {
                    results.Add(item.Value);
                }
            }
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <returns></returns>
        public ObjectPoolBase[] GetObjectPools()
        {
            return GetObjectPools(Condition);
        }

        private bool Condition(ObjectPoolBase obj)
        {
            return true;
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="results">所有对象池</param>
        public void GetObjectPools(List<ObjectPoolBase> results)
        {
            GetObjectPools(Condition, results);
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="sort">是否根据对象池优先级获取</param>
        /// <returns>所有对象池</returns>
        public ObjectPoolBase[] GetObjectPools(bool sort)
        {
            if (sort)
            {
                List<ObjectPoolBase> objectPoolBases = new List<ObjectPoolBase>();
                GetObjectPools(objectPoolBases);
                objectPoolBases.Sort(ObjectPoolComparer);
                return objectPoolBases.ToArray();
            }
            else
            {
                return GetObjectPools();
            }
        }

        private int ObjectPoolComparer(ObjectPoolBase x, ObjectPoolBase y)
        {
            return x.Priority.CompareTo(y.Priority);
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="sort">是否根据对象池优先级获取</param>
        /// <param name="results">所有对象池</param>
        public void GetObjectPools(bool sort, List<ObjectPoolBase> results)
        {
            if (sort)
            {
                GetObjectPools(results);
                results.Sort(ObjectPoolComparer);
            }
            else
            {
                GetObjectPools(results);
            }
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="capacity">对象池的容量。</param>
        /// <param name="expireTime">对象池对象过期秒数。</param>
        /// <param name="priority">对象池的优先级。</param>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return InternalDestroyObjectPool(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象池名</param>
        /// <returns>是否销毁</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalDestroyObjectPool(Utility.Text.GetFullName<T>(name));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="objectPool">需要销毁的对象池</param>
        /// <returns>是否销毁</returns>
        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            if (objectPool == null)
            {
                throw new FrameworkException(" the object pool is invalid ");
            }
            return InternalDestroyObjectPool(objectPool.GetFullName);
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns>是否销毁</returns>
        public bool DestroyObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new FrameworkException(" the object type is invalid ");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new FrameworkException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return InternalDestroyObjectPool(Utility.Text.GetFullName(objectType, string.Empty));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名</param>
        /// <returns>是否销毁</returns>
        public bool DestroyObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new FrameworkException(" the object type is invalid ");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new FrameworkException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }
            return InternalDestroyObjectPool(Utility.Text.GetFullName(objectType, name));
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectPool">需要销毁的对象池</param>
        /// <returns>是否销毁</returns>
        public bool DestroyObjectPool(ObjectPoolBase objectPool)
        {
            if (objectPool == null)
            {
                throw new FrameworkException(" the object pool is invalid ");
            }
            return InternalDestroyObjectPool(objectPool.GetFullName);
        }

        private bool InternalDestroyObjectPool(string fullName)
        {
            if (!IsExitObjectPool(fullName))
            {
                throw new FrameworkException(" the object pool of type is invalid ");
            }
            ObjectPoolBase objectPool = null;
            if(_ObjectPools.TryGetValue(fullName,out objectPool))
            {
                objectPool.Shutdown();
                return _ObjectPools.Remove(fullName);
            }
            return false;
        }

        /// <summary>
        /// 释放对象池中可释放对象
        /// </summary>
        public void Release()
        {
            ObjectPoolBase[] objectPoolBases = GetObjectPools(true);
            foreach (ObjectPoolBase item in objectPoolBases)
            {
                item.Release();
            }
        }

        /// <summary>
        /// 释放所有未被使用的对象
        /// </summary>
        public void ReleaseAllUnUsedObjectPool()
        {
            ObjectPoolBase[] objectPoolBases = GetObjectPools(true);
            foreach (ObjectPoolBase item in objectPoolBases)
            {
                item.ReleaseAllUnused();
            }
        }
    }
}
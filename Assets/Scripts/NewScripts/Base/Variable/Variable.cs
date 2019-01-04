using System;

namespace PJW.Variable
{
    /// <summary>
    /// 变量
    /// </summary>
    public abstract class Variable
    {
        protected Variable()
        {

        }
        /// <summary>
        /// 获取变量类型
        /// </summary>
        public abstract Type GetType
        {
            get;
        }
        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetValue(object value);
        /// <summary>
        /// 得到变量值
        /// </summary>
        /// <returns></returns>
        public abstract object GetValue();
        /// <summary>
        /// 重置变量
        /// </summary>
        public abstract void Reset();
    }
}
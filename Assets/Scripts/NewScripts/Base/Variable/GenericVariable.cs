using System;

namespace PJW.Variable
{
    /// <summary>
    /// 变量
    /// </summary>
    public abstract class Variable<T> : Variable
    {
        private T _Value;
        protected Variable()
        {
            _Value = default(T);
        }
        protected Variable(T value)
        {
            _Value = value;
        }
        /// <summary>
        /// 得到变量的类型
        /// </summary>
        public override Type GetType
        {
            get
            {
                return typeof(T);
            }
        }
        /// <summary>
        /// 变量值的属性
        /// </summary>
        public T Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        /// <summary>
        /// 得到变量值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return _Value;
        }
        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object value)
        {
            _Value = (T)value;
        }
        /// <summary>
        /// 重置变量值
        /// </summary>
        public override void Reset()
        {
            _Value = default(T);
        }
        /// <summary>
        /// 获取变量字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (_Value != null) ? _Value.ToString() : "Null";
        }
    }
}
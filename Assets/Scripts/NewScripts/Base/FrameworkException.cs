
using System;
using System.Runtime.Serialization;

namespace PJW
{
    /// <summary>
    /// 框架异常处理机制
    /// </summary>
    public class FrameworkException : Exception
    {
        /// <summary>
        /// 初始化游戏框架异常类的构造函数
        /// </summary>
        public FrameworkException() : base()
        {

        }
        /// <summary>
        /// 使用指定错误信息初始化游戏框架异常类的构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        public FrameworkException(string message) : base(message)
        {

        }
        /// <summary>
        /// 使用指定错误信息并且对此异常原因的内部异常的引用的构造函数
        /// </summary>
        /// <param name="message">解释异常原因的错误信息</param>
        /// <param name="innerException">导致当前异常的异常</param>
        public FrameworkException(string message, Exception innerException) : base(message, innerException)
        {

        }
        /// <summary>
        /// 用序列化数据初始化框架异常类的构造函数
        /// </summary>
        /// <param name="info">有关所引发异常的序列化的对象数据</param>
        /// <param name="context">包含有关源或目标的上下文信息</param>
        public FrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
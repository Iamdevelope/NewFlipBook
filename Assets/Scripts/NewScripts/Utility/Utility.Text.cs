using System;
using System.Text;

namespace PJW
{
    public static partial class Utility
    {
        /// <summary>
        /// 字符相关的实用函数
        /// </summary>
        public static class Text
        {
            private static StringBuilder cacheString = new StringBuilder(1024);
            /// <summary>
            /// 获取格式化字符串
            /// </summary>
            /// <param name="format">字符串格式</param>
            /// <param name="arg0">字符串参数</param>
            /// <returns>格式化后的字符串</returns>
            public static string Format(string format,string arg0)
            {
                if (format == null)
                {
                    throw new FrameworkException(" Format is invialid ");
                }
                cacheString.Length = 0;
                cacheString.AppendFormat(format, arg0);
                return cacheString.ToString();
            }
            /// <summary>
            /// 获取格式化字符串
            /// </summary>
            /// <param name="format">字符串格式</param>
            /// <param name="arg0">字符串参数</param>
            /// <param name="arg1">字符串参数</param>
            /// <returns>格式化后的字符串</returns>
            public static string Format(string format, string arg0, string arg1)
            {
                if (format == null)
                {
                    throw new FrameworkException(" Format is invialid ");
                }
                cacheString.Length = 0;
                cacheString.AppendFormat(format, arg0, arg1);
                return cacheString.ToString();
            }
            /// <summary>
            /// 获取格式化字符串
            /// </summary>
            /// <param name="format">字符串格式</param>
            /// <param name="arg0">字符串参数</param>
            /// <param name="arg1">字符串参数</param>
            /// <param name="arg2">字符串参数</param>
            /// <returns>格式化后的字符串</returns>
            public static string Format(string format, string arg0, string arg1, string arg2)
            {
                if (format == null)
                {
                    throw new FrameworkException(" Format is invialid ");
                }
                cacheString.Length = 0;
                cacheString.AppendFormat(format, arg0, arg1, arg2);
                return cacheString.ToString();
            }
            /// <summary>
            /// 获取格式化字符串
            /// </summary>
            /// <param name="format">字符串格式</param>
            /// <param name="args">字符串参数</param>
            /// <returns>格式化后的字符串</returns>
            public static string Format(string format, string[] args)
            {
                if (format == null)
                {
                    throw new FrameworkException(" Format is invialid ");
                }
                if (args == null)
                {
                    throw new FrameworkException(" Args is invialid ");
                }
                cacheString.Length = 0;
                cacheString.AppendFormat(format, args);
                return cacheString.ToString();
            }
            /// <summary>
            /// 根据类型和名称获取完整名称
            /// </summary>
            /// <typeparam name="T">类型</typeparam>
            /// <param name="name">名称</param>
            /// <returns>完整名称</returns>
            public static string GetFullName<T>(string name)
            {
                return GetFullName(typeof(T), name);
            }
            /// <summary>
            /// 根据类型和名称获取完整名称
            /// </summary>
            /// <typeparam name="T">类型</typeparam>
            /// <param name="name">名称</param>
            /// <returns>完整名称</returns>
            public static string GetFullName(Type type,string name)
            {
                if (type == null)
                {
                    throw new FrameworkException(" Type is invialid ");
                }
                string typeName = type.FullName;
                return string.IsNullOrEmpty(name) ? typeName : Format("{0},{1}", typeName, name);
            }
        }
    }
}
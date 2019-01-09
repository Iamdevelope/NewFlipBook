
using System.IO;

namespace PJW.Localization
{
    /// <summary>
    /// 本地化管理器辅助器接口
    /// </summary>
    public interface ILocalizationHelper
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        Language SystemLanguage
        {
            get;
        }
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAsset">需要加载字典资源</param>
        /// <param name="loadType">字典加载类型</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否加载成功</returns>
        bool LoadDictionary(object dictionaryAsset, LoadType loadType, object userData);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">需要解析的文本</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(string text, object userData);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">需要解析的二进制数据</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(byte[] bytes, object userData);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">需要解析的流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(Stream stream, object userData);
        /// <summary>
        /// 释放字典
        /// </summary>
        /// <param name="dictionaryAsset">需要释放的字典资源</param>
        void ReleaseDictionaryAsset(object dictionaryAsset);
    }
}

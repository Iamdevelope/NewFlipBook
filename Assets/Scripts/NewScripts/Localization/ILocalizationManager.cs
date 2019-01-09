
using System;
using System.IO;

namespace PJW.Localization
{
    /// <summary>
    /// 本地化管理器接口
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// 获取或设置本地化语言
        /// </summary>
        Language Language
        {
            get;
            set;
        }
        /// <summary>
        /// 获取系统语言
        /// </summary>
        Language SystemLanguage
        {
            get;
        }
        /// <summary>
        /// 获取字典数量
        /// </summary>
        int GetDictionaryCount
        {
            get;
        }
        /// <summary>
        /// 加载字典依赖事件
        /// </summary>
        event EventHandler<LoadDictionaryDependencyEventAvgs> loadDictionaryDependencyEvent;
        /// <summary>
        /// 加载字典失败事件
        /// </summary>
        event EventHandler<LoadDictionaryFailureEventAvgs> loadDictionaryFailureEvent;
        /// <summary>
        /// 加载字典成功事件
        /// </summary>
        event EventHandler<LoadDictionarySuccessEventAvgs> loadDictionarySuccessEvent;
        /// <summary>
        /// 加载字典更新事件
        /// </summary>
        event EventHandler<LoadDictionaryUpdateEventAvgs> loadDictionaryUpdateEvent;

        //void SetResourceManager()
        /// <summary>
        /// 设置本地化辅助器
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器</param>
        void SetLocalizationHelper(ILocalizationHelper localizationHelper);
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        void LoadDictionary(string dictionaryAssetName, LoadType loadType);
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        /// <param name="priority">优先级</param>
        void LoadDictionary(string dictionaryAssetName, LoadType loadType, int priority);
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        /// <param name="userData">用户自定义数据</param>
        void LoadDictionary(string dictionaryAssetName, LoadType loadType, object userData);
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">用户自定义数据</param>
        void LoadDictionary(string dictionaryAssetName, LoadType loadType, int priority, object userData);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">要解析字典文本</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(string text);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">要解析字典文本</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(string text, object userData);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="bytes">要解析的二进制流</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(byte[] bytes);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="bytes">要解析的二进制流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(byte[] bytes, object userData);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="stream">要解析的流</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(Stream stream);
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="stream">要解析的流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        bool ParseDictionary(Stream stream, object userData);
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <returns>获取的字典内容字符串</returns>
        string GetString(string str);
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="arg0">字典参数 0</param>
        /// <returns>获取的字典内容字符串</returns>
        string GetString(string str, object arg0);
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="arg0">字典参数 0</param>
        /// <param name="arg1">字典参数 1</param>
        /// <returns>获取的字典内容字符串</returns>
        string GetString(string str, object arg0, object arg1);
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="arg0">字典参数 0</param>
        /// <param name="arg1">字典参数 1</param>
        /// <param name="arg2">字典参数 2</param>
        /// <returns>获取的字典内容字符串</returns>
        string GetString(string str, object arg0, object arg1, object arg2);
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="args">字典参数</param>
        /// <returns>获取的字典内容字符串</returns>
        string GetString(string str, params object[] args);
        /// <summary>
        /// 判断字典中是否存在该主键
        /// </summary>
        /// <param name="key">字典主键</param>
        /// <returns>是否存在</returns>
        bool IsExitDictionary(string key);
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="key">字典主键</param>
        /// <returns>字典值</returns>
        string GetDictionaryValue(string key);
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="key">字典主键</param>
        /// <param name="value">字典值</param>
        /// <returns>是否添加成功</returns>
        bool AddDictionary(string key, string value);
        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="key">需要移除的字典主键</param>
        /// <returns>是否移除成功</returns>
        bool RemoveDictionary(string key);
    }
}
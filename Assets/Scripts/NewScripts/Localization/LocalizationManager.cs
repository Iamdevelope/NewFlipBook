
using System;
using System.Collections.Generic;
using System.IO;

namespace PJW.Localization
{
    /// <summary>
    /// 本地化管理器
    /// </summary>
    internal partial class LocalizationManager : FrameworkModule, ILocalizationManager
    {
        private readonly Dictionary<string, string> _Dictionary;
        private ILocalizationHelper _LocalizationHelper;
        private Language _Language;
        private event EventHandler<LoadDictionaryDependencyEventAvgs> _LoadDictionaryDependencyEvent;
        private event EventHandler<LoadDictionaryFailureEventAvgs> _LoadDictionaryFailureEvent;
        private event EventHandler<LoadDictionarySuccessEventAvgs> _LoadDictionarySuccessEvent;
        private event EventHandler<LoadDictionaryUpdateEventAvgs> _LoadDictionaryUpdateEvent;

        public LocalizationManager()
        {
            _Dictionary = new Dictionary<string, string>();
            _LocalizationHelper = null;
            _Language = Language.Unspecified;
            _LoadDictionaryUpdateEvent = null;
            _LoadDictionarySuccessEvent = null;
            _LoadDictionaryFailureEvent = null;
            _LoadDictionaryDependencyEvent = null;
        }

        /// <summary>
        /// 获取或设置本地化语言
        /// </summary>
        public Language Language
        {
            get
            {
                if (_Language == Language.Unspecified)
                {
                    throw new FrameworkException(" the language is upspecified ");
                }
                return _Language;
            }
            set
            {
                _Language = value;
            }
        }
        /// <summary>
        /// 获取系统语言
        /// </summary>
        public Language SystemLanguage {
            get
            {
                if (_LocalizationHelper == null)
                {
                    throw new FrameworkException(" Localization Helper is invalid ");
                }
                return _LocalizationHelper.SystemLanguage;
            }
        }
        /// <summary>
        /// 获取字典数量
        /// </summary>
        public int GetDictionaryCount
        {
            get { return _Dictionary.Count; }
        }
        /// <summary>
        /// 加载字典依赖事件
        /// </summary>
        public event EventHandler<LoadDictionaryDependencyEventAvgs> loadDictionaryDependencyEvent
        {
            add { _LoadDictionaryDependencyEvent += value; }
            remove { _LoadDictionaryDependencyEvent -= value; }
        }
        /// <summary>
        /// 加载字典失败事件
        /// </summary>
        public event EventHandler<LoadDictionaryFailureEventAvgs> loadDictionaryFailureEvent
        {
            add { _LoadDictionaryFailureEvent += value; }
            remove { _LoadDictionaryFailureEvent -= value; }
        }
        /// <summary>
        /// 加载字典成功事件
        /// </summary>
        public event EventHandler<LoadDictionarySuccessEventAvgs> loadDictionarySuccessEvent
        {
            add { _LoadDictionarySuccessEvent += value; }
            remove { _LoadDictionarySuccessEvent -= value; }
        }
        /// <summary>
        /// 加载字典更新事件
        /// </summary>
        public event EventHandler<LoadDictionaryUpdateEventAvgs> loadDictionaryUpdateEvent
        {
            add { _LoadDictionaryUpdateEvent += value; }
            remove { _LoadDictionaryUpdateEvent -= value; }
        }
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="key">字典主键</param>
        /// <param name="value">字典值</param>
        /// <returns>是否添加成功</returns>
        public bool AddDictionary(string key, string value)
        {
            if (IsExitDictionary(key))
            {
                return false;
            }
            _Dictionary.Add(key, value);
            return true;
        }
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="key">字典主键</param>
        /// <returns>字典值</returns>
        public string GetDictionaryValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new FrameworkException(" the key is invalid ");
            }
            return _Dictionary.ContainsKey(key) ? _Dictionary[key] : null;
        }
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <returns>获取的字典内容字符串</returns>
        public string GetString(string str)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="arg0">字典参数 0</param>
        /// <returns>获取的字典内容字符串</returns>
        public string GetString(string str, object arg0)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="arg0">字典参数 0</param>
        /// <param name="arg1">字典参数 1</param>
        /// <returns>获取的字典内容字符串</returns>
        public string GetString(string str, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="arg0">字典参数 0</param>
        /// <param name="arg1">字典参数 1</param>
        /// <param name="arg2">字典参数 2</param>
        /// <returns>获取的字典内容字符串</returns>
        public string GetString(string str, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据字典键获取字典值
        /// </summary>
        /// <param name="str">字典键</param>
        /// <param name="args">字典参数</param>
        /// <returns>获取的字典内容字符串</returns>
        public string GetString(string str, params object[] args)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 判断字典中是否存在该主键
        /// </summary>
        /// <param name="key">字典主键</param>
        /// <returns>是否存在</returns>
        public bool IsExitDictionary(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new FrameworkException(" the key is invalid ");
            }
            return _Dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        public void LoadDictionary(string dictionaryAssetName, LoadType loadType)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        /// <param name="priority">优先级</param>
        public void LoadDictionary(string dictionaryAssetName, LoadType loadType, int priority)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadDictionary(string dictionaryAssetName, LoadType loadType, object userData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryAssetName">字典名</param>
        /// <param name="loadType">加载类型</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadDictionary(string dictionaryAssetName, LoadType loadType, int priority, object userData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">要解析字典文本</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDictionary(string text)
        {
            return ParseDictionary(text, null);
        }
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="text">要解析字典文本</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDictionary(string text, object userData)
        {
            if (_LocalizationHelper == null)
            {
                throw new FrameworkException(" you must set localization helper ");
            }
            try
            {
                return _LocalizationHelper.ParseDictionary(text, userData);
            }
            catch (Exception e)
            {
                if (e is FrameworkException)
                {
                    throw;
                }
                throw new FrameworkException(Utility.Text.Format(" can not parse dictionary , because {0} .excaption ", e.Message), e);
            }
        }
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="bytes">要解析的二进制流</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDictionary(byte[] bytes)
        {
            return ParseDictionary(bytes, null);
        }
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="bytes">要解析的二进制流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDictionary(byte[] bytes, object userData)
        {
            if (_LocalizationHelper == null)
            {
                throw new FrameworkException(" you must set localization helper ");
            }
            try
            {
                return _LocalizationHelper.ParseDictionary(bytes, userData);
            }
            catch (Exception e)
            {
                if (e is FrameworkException)
                {
                    throw;
                }
                throw new FrameworkException(Utility.Text.Format(" can not parse dictionary , because {0} .excaption ", e.Message), e);
            }
        }
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="stream">要解析的流</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDictionary(Stream stream)
        {
            return ParseDictionary(stream, null);
        }
        /// <summary>
        /// 解析字典
        /// </summary>
        /// <param name="stream">要解析的流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>是否解析成功</returns>
        public bool ParseDictionary(Stream stream, object userData)
        {
            if (_LocalizationHelper == null)
            {
                throw new FrameworkException(" you must set localization helper ");
            }
            try
            {
                return _LocalizationHelper.ParseDictionary(stream, userData);
            }
            catch (Exception e)
            {
                if(e is FrameworkException)
                {
                    throw;
                }
                throw new FrameworkException(Utility.Text.Format(" can not parse dictionary , because {0} .excaption ", e.Message), e);
            }
        }
        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="key">需要移除的字典主键</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveDictionary(string key)
        {
            if (!IsExitDictionary(key))
            {
                return false;
            }
            return _Dictionary.Remove(key);
        }
        /// <summary>
        /// 设置本地化辅助器
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器</param>
        public void SetLocalizationHelper(ILocalizationHelper localizationHelper)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.IO;
using PJW.Resources;
using System.Collections.Generic;

namespace PJW.Config
{
    /// <summary>
    /// 配置文件管理器
    /// </summary>
    internal sealed partial class ConfigManager : FrameworkModule,IConfigManager
    {
        private readonly Dictionary<string,ConfigData> _ConfigDatas;
        private readonly LoadAssetCallbacks _LoadAssetCallbacks;
        private IResourcesManager _ResourcesManager;
        private IConfigHelper _ConfigHelper;
        private EventHandler<LoadConfigDependencyEventAvgs> _LoadConfigDependencyHandler;
        private EventHandler<LoadConfigFailureEventAvgs> _LoadConfigFailureHandler;
        private EventHandler<LoadConfigSuccessEventAvgs> _LoadConfigSuccessHandler;
        private EventHandler<LoadConfigUpdateEventAvgs> _LoadConfigUpdateHandler;

        public ConfigManager(){
            _ConfigDatas=new Dictionary<string, ConfigData>();
            
        }

        /// <summary>
		/// 配置资源个数
		/// </summary>
		/// <value></value>
        public int GetConfigCount
        {
            get
            {
                return _ConfigDatas.Count;
            }
        }

        /// <summary>
		/// 配置加载依赖事件
		/// </summary>
        public event EventHandler<LoadConfigDependencyEventAvgs> _LoadConfigDependency{
            add{
                _LoadConfigDependencyHandler+=value;
            }
            remove{
                _LoadConfigDependencyHandler-=value;
            }
        }

        /// <summary>
		/// 配置加载失败事件
		/// </summary>
        public event EventHandler<LoadConfigFailureEventAvgs> _LoadConfigFailure{
            add{
                _LoadConfigFailureHandler+=value;
            }
            remove{
                _LoadConfigFailureHandler-=value;
            }
        }

        /// <summary>
		/// 配置加载成功事件
		/// </summary>
        public event EventHandler<LoadConfigSuccessEventAvgs> _LoadConfigSuccess{
            add{
                _LoadConfigSuccessHandler+=value;
            }
            remove{
                _LoadConfigSuccessHandler-=value;
            }
        }

        /// <summary>
		/// 配置加载更新事件
		/// </summary>
        public event EventHandler<LoadConfigUpdateEventAvgs> _LoadConfigUpdate{
            add{
                _LoadConfigUpdateHandler+=value;
            }
            remove{
                _LoadConfigUpdateHandler-=value;
            }
        }
        
        /// <summary>
		/// 设置资源管理器
		/// </summary>
		/// <param name="resourcesManager">资源管理器</param>
        public void SetResourceManager(IResourcesManager resourcesManager)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
		/// 设置配置辅助器
		/// </summary>
		/// <param name="configHelper">配置辅助器</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 判断是否存在指定配置
		/// </summary>
		/// <param name="configName">需要检查的配置名</param>
		/// <returns>是否存在指定配置</returns>
        public bool IsExitConfig(string configName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
        public void LoadConfig(string configAssetName, LoadType loadType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="priority">加载优先级</param>
        public void LoadConfig(string configAssetName, LoadType loadType, int priority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="userData">用户自定义数据</param>
        public void LoadConfig(string configAssetName, LoadType loadType, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="priority">加载优先级</param>
		/// <param name="userData">用户自定义数据</param>
        public void LoadConfig(string configAssetName, LoadType loadType, int priority, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="text">配置文本</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="text">配置文本</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(string text, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="bytes">需要解析的二进制流</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="bytes">需要解析的二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(byte[] bytes, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="stream">需要解析的二进制流</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="stream">需要解析的二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(Stream stream, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 添加指定配置
		/// </summary>
		/// <param name="configName">配置文件名</param>
		/// <param name="boolValue">配置文件的布尔值</param>
		/// <param name="intValue">配置文件的整数值</param>
		/// <param name="floatValue">配置文件的浮点值</param>
		/// <param name="stringValue">配置文件的字符值</param>
		/// <returns>是否添加成功</returns>
        public bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 移除指定配置
		/// </summary>
		/// <param name="configName">需要移除的配置名</param>
        public void RemoveConfig(string configName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 移除所有配置
		/// </summary>
        public void RemoveAllConfig()
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置中的布尔值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的布尔值</returns>
        public bool GetBool(string configName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置中的布尔值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定配置不存在时，返回此默认值</param>
		/// <returns>返回的布尔值</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置中的整数值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的整数值</returns>
        public int GetInt(string configName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置中的整数值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的整数值</returns>
        public int GetInt(string configName, int defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置的浮点值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的浮点值</returns>
        public float GetFloat(string configName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置的浮点值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的浮点值</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置的字符值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的字符值</returns>
        public string GetString(string configName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// 获取指定配置的字符值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的字符值</returns>
        public string GetString(string configName, string defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 配置管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 关闭并清理配置管理器。
        /// </summary>
        public override void Shutdown()
        {

        }
    }
}
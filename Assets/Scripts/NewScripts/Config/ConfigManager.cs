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
        private EventHandler<LoadConfigDependencyEventArgs> _LoadConfigDependencyHandler;
        private EventHandler<LoadConfigFailureEventArgs> _LoadConfigFailureHandler;
        private EventHandler<LoadConfigSuccessEventArgs> _LoadConfigSuccessHandler;
        private EventHandler<LoadConfigUpdateEventArgs> _LoadConfigUpdateHandler;

        public ConfigManager(){
            _ConfigDatas=new Dictionary<string, ConfigData>();
            _LoadAssetCallbacks=new LoadAssetCallbacks(OnLoadAssetSuccessCallback,OnLoadAssetDependencyCallback,OnLoadAssetFailureCallback,OnLoadAssetUpdateCallback);
            _ResourcesManager=null;
            _ConfigHelper=null;
            _LoadConfigDependencyHandler=null;
            _LoadConfigFailureHandler=null;
            _LoadConfigSuccessHandler=null;
            _LoadConfigUpdateHandler=null;
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
        public event EventHandler<LoadConfigDependencyEventArgs> _LoadConfigDependency{
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
        public event EventHandler<LoadConfigFailureEventArgs> _LoadConfigFailure{
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
        public event EventHandler<LoadConfigSuccessEventArgs> _LoadConfigSuccess{
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
        public event EventHandler<LoadConfigUpdateEventArgs> _LoadConfigUpdate{
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
            if(resourcesManager==null){
                throw new FrameworkException("Resource manager is invalid ");
            }
            _ResourcesManager=resourcesManager;
        }
        
        /// <summary>
		/// 设置配置辅助器
		/// </summary>
		/// <param name="configHelper">配置辅助器</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            if(configHelper==null){
                throw new FrameworkException("Config helper is invalid ");
            }
            _ConfigHelper=configHelper;
        }

        /// <summary>
		/// 判断是否存在指定配置
		/// </summary>
		/// <param name="configName">需要检查的配置名</param>
		/// <returns>是否存在指定配置</returns>
        public bool IsExitConfig(string configName)
        {
            return GetConfigData(configName).HasValue;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configAssetName">配置资源名</param>
        /// <param name="loadType">加载类型</param>
        public void LoadConfig(string configAssetName, LoadType loadType)
        {
            LoadConfig(configAssetName,loadType,Constant.DefaultPriority,null);
        }

        /// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="priority">加载优先级</param>
        public void LoadConfig(string configAssetName, LoadType loadType, int priority)
        {
            LoadConfig(configAssetName,loadType,priority,null);
        }

        /// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="userData">用户自定义数据</param>
        public void LoadConfig(string configAssetName, LoadType loadType, object userData)
        {
            LoadConfig(configAssetName,loadType,Constant.DefaultPriority,userData);
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
            if(configAssetName==null){
                throw new FrameworkException("Config asset name is invalid ");
            }
            if(_ResourcesManager==null){
                throw new FrameworkException("You must set resources manager first ");
            }
            _ResourcesManager.LoadAsset(configAssetName,priority,new LoadConfigInfo(loadType,userData),_LoadAssetCallbacks);
        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="text">配置文本</param>
        /// <returns>是否解析成功</returns>
        public bool ParseConfig(string text)
        {
            return ParseConfig(text,null);
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="text">配置文本</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(string text, object userData)
        {
            if(_ConfigHelper==null){
                throw new FrameworkException("You must set config helper first ");
            }
            try{
                return _ConfigHelper.ParseConfig(text,userData);
            }
            catch(Exception e){
                if(e is FrameworkException){
                    throw ;
                }
                throw new FrameworkException(Utility.Text.Format("Can not parse config error is : {0} ",e.Message),e);
            }
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="bytes">需要解析的二进制流</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(byte[] bytes)
        {
            return ParseConfig(bytes,null);
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="bytes">需要解析的二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(byte[] bytes, object userData)
        {
            if(_ConfigHelper==null){
                throw new FrameworkException("You must set config helper first ");
            }
            try{
                return _ConfigHelper.ParseConfig(bytes,userData);
            }
            catch(Exception e){
                if(e is FrameworkException){
                    throw ;
                }
                throw new FrameworkException(Utility.Text.Format("Can not parse config error is : {0} ",e.Message),e);
            }
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="stream">需要解析的二进制流</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(Stream stream)
        {
            return ParseConfig(stream,null);
        }

        /// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="stream">需要解析的二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
        public bool ParseConfig(Stream stream, object userData)
        {
            if(_ConfigHelper==null){
                throw new FrameworkException("You must set config helper first ");
            }
            try{
                return _ConfigHelper.ParseConfig(stream,userData);
            }
            catch(Exception e){
                if(e is FrameworkException){
                    throw ;
                }
                throw new FrameworkException(Utility.Text.Format("Can not parse config error is : {0} ",e.Message),e);
            }
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
            if(IsExitConfig(configName)){
                return false;
            }
            _ConfigDatas.Add(configName,new ConfigData(boolValue,intValue,floatValue,stringValue));
            return true;
        }

        /// <summary>
		/// 移除指定配置
		/// </summary>
		/// <param name="configName">需要移除的配置名</param>
        public void RemoveConfig(string configName)
        {
            _ConfigDatas.Remove(configName);
        }

        /// <summary>
		/// 移除所有配置
		/// </summary>
        public void RemoveAllConfig()
        {
            _ConfigDatas.Clear();
        }

        /// <summary>
		/// 获取指定配置中的布尔值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的布尔值</returns>
        public bool GetBool(string configName)
        {
            ConfigData? configData=GetConfigData(configName);
            if(configData==null){
                throw new FrameworkException(Utility.Text.Format("Config {0} not exist "+configName));
            }
            return configData.Value.GetBool;
        }

        /// <summary>
		/// 获取指定配置中的布尔值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定配置不存在时，返回此默认值</param>
		/// <returns>返回的布尔值</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            ConfigData? configData=GetConfigData(configName);
            return configData==null?defaultValue:configData.Value.GetBool;
        }

        /// <summary>
		/// 获取指定配置中的整数值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的整数值</returns>
        public int GetInt(string configName)
        {
            ConfigData? configData=GetConfigData(configName);
            if(configData==null){
                throw new FrameworkException(Utility.Text.Format("Config {0} not exist "+configName));
            }
            return configData.Value.GetInt;
        }

        /// <summary>
		/// 获取指定配置中的整数值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的整数值</returns>
        public int GetInt(string configName, int defaultValue)
        {
            ConfigData? configData=GetConfigData(configName);
            return configData==null?defaultValue:configData.Value.GetInt;
        }

        /// <summary>
		/// 获取指定配置的浮点值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的浮点值</returns>
        public float GetFloat(string configName)
        {
            ConfigData? configData=GetConfigData(configName);
            if(configData==null){
                throw new FrameworkException(Utility.Text.Format("Config {0} not exist "+configName));
            }
            return configData.Value.GetFloat;
        }

        /// <summary>
		/// 获取指定配置的浮点值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的浮点值</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            ConfigData? configData=GetConfigData(configName);
            return configData==null?defaultValue:configData.Value.GetFloat;
        }

        /// <summary>
		/// 获取指定配置的字符值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的字符值</returns>
        public string GetString(string configName)
        {
            ConfigData? configData=GetConfigData(configName);
            if(configData==null){
                throw new FrameworkException(Utility.Text.Format("Config {0} not exist "+configName));
            }
            return configData.Value.GetString;
        }

        /// <summary>
		/// 获取指定配置的字符值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的字符值</returns>
        public string GetString(string configName, string defaultValue)
        {
            ConfigData? configData=GetConfigData(configName);
            return configData==null?defaultValue:configData.Value.GetString;
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
        private ConfigData? GetConfigData(string configName)
        {
            if(configName==null){
                throw new FrameworkException("Config name is invalid ");
            }
            ConfigData configData=default(ConfigData);
            if(_ConfigDatas.TryGetValue(configName,out configData)){
                return configData;
            }
            return null;
        }
        
        /// <summary>
        /// 加载资源更新
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="progress">更新进度</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnLoadAssetUpdateCallback(string assetName, float progress, object userData)
        {
            LoadConfigInfo loadConfigInfo=default(LoadConfigInfo);
            if(loadConfigInfo==null){
                throw new FrameworkException("Load config info is invalid ");
            }
            if(_LoadConfigUpdateHandler!=null){
                _LoadConfigUpdateHandler(this,new LoadConfigUpdateEventArgs(assetName,progress,loadConfigInfo.GetUserData));
            }
        }

        /// <summary>
        /// 加载失败
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="loadResourceStatus">资源状态</param>
        /// <param name="errorMessage">错误原因</param>
        /// <param name="userData">用户自定义数据</param>
        private void OnLoadAssetFailureCallback(string assetName, LoadResourceStatus loadResourceStatus, string errorMessage, object userData)
        {
            LoadConfigInfo loadConfigInfo=default(LoadConfigInfo);
            if(loadConfigInfo==null){
                throw new FrameworkException("Load config info is invalid ");
            }
            string error=Utility.Text.Format("Load config failure, asset name {0} ,load resourcesStatus {1} , error message {2} ",assetName,loadResourceStatus.ToString(),errorMessage);
            if(_LoadConfigFailureHandler!=null){
                _LoadConfigFailureHandler(this,new LoadConfigFailureEventArgs(assetName,error,loadConfigInfo.GetUserData));
                return;
            }
            throw new FrameworkException(error);
        }

        /// <summary>
        /// 加载资源时加载依赖资源回调函数。
        /// </summary>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="dependencyName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        private void OnLoadAssetDependencyCallback(string assetName, string dependencyName, int loadedCount, int totalCount, object userData)
        {
            LoadConfigInfo loadConfigInfo=default(LoadConfigInfo);
            if(loadConfigInfo==null){
                throw new FrameworkException("Load config info is invalid ");
            }
            if(_LoadConfigDependencyHandler!=null){
                _LoadConfigDependencyHandler(this,new LoadConfigDependencyEventArgs(assetName,dependencyName,loadedCount,totalCount,loadConfigInfo.GetUserData));
            }
        }

        /// <summary>
        /// 加载资源成功
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="loadedAsset">已加载资源</param>
        /// <param name="duration">加载所用时长</param>
        /// <param name="userData">用户自定义</param>
        private void OnLoadAssetSuccessCallback(string assetName, object loadedAsset, float duration, object userData)
        {
            LoadConfigInfo loadConfigInfo=default(LoadConfigInfo);
            if(loadConfigInfo==null){
                throw new FrameworkException("Load config info is invalid ");
            }
            try{
                if(!_ConfigHelper.LoadConfig(loadedAsset,loadConfigInfo.GetLoadType,loadConfigInfo.GetUserData)){
                    throw new FrameworkException(Utility.Text.Format("Load config failure in helper , asset name {0} ",assetName));
                }
            }
            catch(Exception e){
                if(_LoadConfigFailureHandler!=null){
                    _LoadConfigFailureHandler(this,new LoadConfigFailureEventArgs(assetName,e.Message,loadConfigInfo.GetUserData));
                    return;
                }
                throw;
            }
            finally{
                _ConfigHelper.ReleaseConfigAsset(assetName);
            }
            if(_LoadConfigSuccessHandler!=null){
                _LoadConfigSuccessHandler(this,new LoadConfigSuccessEventArgs(assetName ,duration,loadConfigInfo.GetUserData));
            }
        }
    }
}
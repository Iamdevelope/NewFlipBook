using System;
using PJW.Resources;
using System.IO;

namespace PJW.Config
{
	/// <summary>
	/// 配置脚本管理接口
	/// </summary>
	public interface IConfigManager {
		/// <summary>
		/// 配置资源个数
		/// </summary>
		/// <value></value>
		int GetConfigCount{get;}

		/// <summary>
		/// 配置加载依赖事件
		/// </summary>
		event EventHandler<LoadConfigDependencyEventAvgs> _LoadConfigDependency;
		
		/// <summary>
		/// 配置加载失败事件
		/// </summary>
		event EventHandler<LoadConfigFailureEventAvgs> _LoadConfigFailure;
		
		/// <summary>
		/// 配置加载成功事件
		/// </summary>
		event EventHandler<LoadConfigSuccessEventAvgs> _LoadConfigSuccess;
		
		/// <summary>
		/// 配置加载更新事件
		/// </summary>
		event EventHandler<LoadConfigUpdateEventAvgs> _LoadConfigUpdate;

		/// <summary>
		/// 设置资源管理器
		/// </summary>
		/// <param name="resourcesManager">资源管理器</param>
		void SetResourceManager(IResourcesManager resourcesManager);

		/// <summary>
		/// 设置配置辅助器
		/// </summary>
		/// <param name="configHelper">配置辅助器</param>
		void SetConfigHelper(IConfigHelper configHelper);

		/// <summary>
		/// 判断是否存在指定配置
		/// </summary>
		/// <param name="configName">需要检查的配置名</param>
		/// <returns>是否存在指定配置</returns>
		bool IsExitConfig(string configName);

		/// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		void LoadConfig(string configAssetName,LoadType loadType);

		/// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="priority">加载优先级</param>
		void LoadConfig(string configAssetName,LoadType loadType,int priority);

		/// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="userData">用户自定义数据</param>
		void LoadConfig(string configAssetName,LoadType loadType,object userData);

		/// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAssetName">配置资源名</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="priority">加载优先级</param>
		/// <param name="userData">用户自定义数据</param>
		void LoadConfig(string configAssetName,LoadType loadType,int priority,object userData);

		/// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="text">配置文本</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(string text);

		/// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="text">配置文本</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(string text,object userData);

		/// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="bytes">需要解析的二进制流</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(byte[] bytes);
		
		/// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="bytes">需要解析的二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(byte[] bytes,object userData);
		
		/// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="stream">需要解析的二进制流</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(Stream stream);
		
		/// <summary>
		/// 解析配置
		/// </summary>
		/// <param name="stream">需要解析的二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(Stream stream,object userData);

		/// <summary>
		/// 添加指定配置
		/// </summary>
		/// <param name="configName">配置文件名</param>
		/// <param name="boolValue">配置文件的布尔值</param>
		/// <param name="intValue">配置文件的整数值</param>
		/// <param name="floatValue">配置文件的浮点值</param>
		/// <param name="stringValue">配置文件的字符值</param>
		/// <returns>是否添加成功</returns>
		bool AddConfig(string configName,bool boolValue,int intValue,float floatValue,string stringValue);
		
		/// <summary>
		/// 移除指定配置
		/// </summary>
		/// <param name="configName">需要移除的配置名</param>
		void RemoveConfig(string configName);

		/// <summary>
		/// 移除所有配置
		/// </summary>
		void RemoveAllConfig();

		/// <summary>
		/// 获取指定配置中的布尔值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的布尔值</returns>
		bool GetBool(string configName);
		
		/// <summary>
		/// 获取指定配置中的布尔值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定配置不存在时，返回此默认值</param>
		/// <returns>返回的布尔值</returns>
		bool GetBool(string configName,bool defaultValue);
		
		/// <summary>
		/// 获取指定配置中的整数值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的整数值</returns>
		int GetInt(string configName);
		
		/// <summary>
		/// 获取指定配置中的整数值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的整数值</returns>
		int GetInt(string configName,int defaultValue);
		
		/// <summary>
		/// 获取指定配置的浮点值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的浮点值</returns>
		float GetFloat(string configName);
		
		/// <summary>
		/// 获取指定配置的浮点值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的浮点值</returns>
		float GetFloat(string configName,float defaultValue);
		
		/// <summary>
		/// 获取指定配置的字符值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <returns>返回的字符值</returns>
		string GetString(string configName);
		
		/// <summary>
		/// 获取指定配置的字符值
		/// </summary>
		/// <param name="configName">指定的配置名</param>
		/// <param name="defaultValue">当指定的配置不存在时，返回此默认值</param>
		/// <returns>返回的字符值</returns>
		string GetString(string configName,string defaultValue);
	}
}

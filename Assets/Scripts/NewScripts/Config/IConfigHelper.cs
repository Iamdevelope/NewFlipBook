using System.IO;

namespace PJW.Config
{
	/// <summary>
	/// 配置文件帮助接口
	/// </summary>
	public interface IConfigHelper{
		/// <summary>
		/// 加载配置
		/// </summary>
		/// <param name="configAsset">配置资源</param>
		/// <param name="loadType">加载类型</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否加载成功</returns>
		bool LoadConfig(object configAsset,LoadType loadType,object userData);
		
		/// <summary>
		/// 解析配置文件
		/// </summary>
		/// <param name="text">要解析的配置文本</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(string text,object userData);
		
		/// <summary>
		/// 解析配置文件
		/// </summary>
		/// <param name="bytes">要解析的配置二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(byte[] bytes,object userData);
		
		/// <summary>
		/// 解析配置文件
		/// </summary>
		/// <param name="stream">要解析的配置二进制流</param>
		/// <param name="userData">用户自定义数据</param>
		/// <returns>是否解析成功</returns>
		bool ParseConfig(Stream stream,object userData);
		
		/// <summary>
		/// 释放配置资源
		/// </summary>
		/// <param name="configAsset">配置资源</param>
		void ReleaseConfigAsset(object configAsset);
	}
}
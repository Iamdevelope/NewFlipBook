using System;

namespace PJW.Resources
{
    /// <summary>
    /// 加载资源代理辅助器接口
    /// </summary>
    public interface ILoadResourcesAgentHelper
    {
        /// <summary>
        /// 加载资源代理辅助器错误事件
        /// </summary>
        event EventHandler<LoadResourcesAgentHelperErrorEventArgs> LoadResourcesAgentHelperErrorEventArgs;
        
        /// <summary>
        /// 加载资源代理辅助器异步加载完成事件
        /// </summary>
        event EventHandler<LoadResourcesAgentHelperLoadCompleteEventArgs> LoadResourcesAgentHelperLoadCompleteEventArgs;
        
        /// <summary>
        /// 加载资源代理辅助器异步解析二进制完成事件
        /// </summary>
        event EventHandler<LoadResourcesAgentHelperParseBytesCompleteEventArgs> LoadResourcesAgentHelperParseBytesCompleteEventArgs;
         
        /// <summary>
        /// 加载资源代理辅助器异步读取二进制流完成事件
        /// </summary>
        event EventHandler<LoadResourcesAgentHelperReadBytesCompleteEventArgs> LoadResourcesAgentHelperReadBytesCompleteEventArgs;
        
        /// <summary>
        /// 加载资源代理辅助器异步读取文件完成事件
        /// </summary>
        event EventHandler<LoadResourcesAgentHelperReadFileCompleteEventArgs> LoadResourcesAgentHelperReadFileCompleteEventArgs;
        
        /// <summary>
        /// 加载资源代理器异步加载资源更新事件
        /// </summary>
        event EventHandler<LoadResourcesAgentHelperUpdateEventArgs> LoadResourcesAgentHelperUpdateEventArgs;

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源
        /// </summary>
        /// <param name="fullPath">资源的完整路径</param>
        void ReadFile(string fullPath);

        /// <summary>
        /// 通过加载代理辅助器开始异步读取二进制流
        /// </summary>
        /// <param name="fullPath">资源完整路径</param>
        /// <param name="loadType">加载方式</param>
        void ReadBytes(string fullPath,LoadType loadType);
        
        /// <summary>
        /// 通过加载代理辅助器开始异步将资源二进制流转换为加载对象
        /// </summary>
        /// <param name="bytes">要加载的二进制流</param>
        void ParseBytes(byte[] bytes);

        /// <summary>
        /// 通过加载资源代理辅助器开始异步加载资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="resourceChildName">要加载的子资源名</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="isScene">要加载的资源是否是场景</param>
        void LoadAsset(object resource,string resourceChildName,Type assetType,bool isScene);

        /// <summary>
        /// 重置加载资源代理辅助器
        /// </summary>
        void Reset();
    }
}
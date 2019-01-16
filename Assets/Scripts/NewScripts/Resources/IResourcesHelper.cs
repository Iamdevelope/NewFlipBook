
namespace PJW.Resources
{
    /// <summary>
    /// 资源辅助器接口
    /// </summary>
    public interface IResourcesHelper
    {
        /// <summary>
        /// 直接从文件路径读取数据流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="loadBytesCallback">读取数据流的回调</param>
        void LoadBytes(string filePath,LoadBytesCallback loadBytesCallback);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调</param>
        /// <param name="userData">用户自定义数据</param>
        void UnloadScene(string sceneAssetName,UnloadSceneCallbacks unloadSceneCallbacks,object userData);
        
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="asset">需要释放的资源</param>
        void Release(object asset);
    }
}
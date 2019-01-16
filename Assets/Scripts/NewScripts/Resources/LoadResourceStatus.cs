namespace PJW.Resources
{
    /// <summary>
    /// 加载资源状态
    /// </summary>
    public enum LoadResourceStatus
    {
        /// <summary>
        /// 资源加载完成
        /// </summary>
        OK=0,
        /// <summary>
        /// 资源还未准备完成
        /// </summary>
        NoReady,
        /// <summary>
        /// 资源不存在
        /// </summary>
        NotExist,
        /// <summary>
        /// 依赖资源错误
        /// </summary>
        DependencyAssetError,
        /// <summary>
        /// 资源类型错误
        /// </summary>
        TypeError,
        /// <summary>
        /// 加载子资源错误
        /// </summary>
        ChildAssetError,
        /// <summary>
        /// 加载场景资源错误
        /// </summary>
        SceneAssetError
    }
}
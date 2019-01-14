namespace PJW.Resources
{
    /// <summary>
    /// 加载失败委托
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="loadResourceStatus">资源状态</param>
    /// <param name="errorMessage">错误原因</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void LoadAssetFailureCallback(string assetName,LoadResourceStatus loadResourceStatus,string errorMessage,object userData);
}
namespace PJW.Resources
{
    /// <summary>
    /// 加载数据流委托
    /// </summary>
    /// <param name="assetName">文件路径</param>
    /// <param name="bytes">数据流</param>
    /// <param name="errorMessage">错误信息</param>
    public delegate void LoadBytesCallback(string filePath,byte[] bytes,string errorMessage);
}
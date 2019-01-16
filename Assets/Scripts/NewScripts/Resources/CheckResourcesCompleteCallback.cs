namespace PJW.Resources
{
    /// <summary>
    /// 使用可更新模式并检查资源完成的回调函数
    /// </summary>
    /// <param name="needUpdateResources">是否需要进行资源更新</param>
    /// <param name="removeCount">已移除资源数量</param>
    /// <param name="updateCount">需要更新资源数量</param>
    /// <param name="updateTotalLength">需要更新的总资源数量</param>
    /// <param name="updatTotalZipLength">需要更新的总压缩包大小</param>
    public delegate void CheckResourcesCompleteCallback(bool needUpdateResources,int removeCount,int updateCount,int updateTotalLength,int updatTotalZipLength);
}
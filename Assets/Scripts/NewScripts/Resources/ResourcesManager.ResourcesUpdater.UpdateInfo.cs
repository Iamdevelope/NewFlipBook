namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        private sealed partial class ResourcesUpdater
        {
            /// <summary>
            /// 更新信息
            /// </summary>
            private sealed class UpdateInfo
            {
                private readonly ResourcesName _ResourcesName;
                private readonly LoadType _LoadType;
                private readonly int _Length;
                private readonly int _HashCode;
                private readonly int _ZipLength;
                private readonly int _ZipHashCode;
                private readonly string _SavePath;
                private readonly string _DownloadUrl;
                private readonly int _RetryCount;

                /// <summary>
                /// 初始化更新信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                /// <param name="zipLength">压缩包大小。</param>
                /// <param name="zipHashCode">压缩包哈希值。</param>
                /// <param name="savePath">资源更新下载后存放路径。</param>
                /// <param name="downloadUrl">资源更新下载地址。</param>
                /// <param name="retryCount">已重试次数。</param>
                public UpdateInfo(ResourcesName resourcesName, LoadType loadType, int length, int hashCode, 
                int zipLength, int zipHashCode, string savePath, string downloadUrl, int retryCount)
                {
                    _ResourcesName = resourcesName;
                    _LoadType = loadType;
                    _Length = length;
                    _HashCode = hashCode;
                    _ZipLength = zipLength;
                    _ZipHashCode = zipHashCode;
                    _SavePath = savePath;
                    _DownloadUrl = downloadUrl;
                    _RetryCount = retryCount;
                }

                public int GetRetryCount
                {
                    get
                    {
                        return _RetryCount;
                    }
                }

                public string GetDownloadUrl
                {
                    get
                    {
                        return _DownloadUrl;
                    }
                }

                public string GetSavePath
                {
                    get
                    {
                        return _SavePath;
                    }
                }

                public int GetZipHashCode
                {
                    get
                    {
                        return _ZipHashCode;
                    }
                }

                public int GetZipLength
                {
                    get
                    {
                        return _ZipLength;
                    }
                }

                public int GetHashCode
                {
                    get
                    {
                        return _HashCode;
                    }
                }

                public int GetLength
                {
                    get
                    {
                        return _Length;
                    }
                }

                public LoadType GetLoadType
                {
                    get
                    {
                        return _LoadType;
                    }
                }

                public ResourcesName GetResourcesName
                {
                    get
                    {
                        return _ResourcesName;
                    }
                }
            }
        }
    }
}
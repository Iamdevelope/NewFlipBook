namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        private partial class ResourcesChecker
        {
            /// <summary>
            /// 资源检查信息
            /// </summary>
            private sealed class CheckInfo
            {
                private readonly ResourcesName _ResourceName;
                private CheckStatus _Status;
                private bool _NeedRemove;
                private RemoteVersionInfo _VersionInfo;
                private LocalVersionInfo _ReadOnlyInfo;
                private LocalVersionInfo _ReadWriteInfo;

                /// <summary>
                /// 初始化资源检查信息的新实例。
                /// </summary>
                /// <param name="resourceName">资源名称。</param>
                public CheckInfo(ResourcesName resourceName)
                {
                    _ResourceName = resourceName;
                    _Status = CheckStatus.Unknown;
                    _NeedRemove = false;
                    _VersionInfo = default(RemoteVersionInfo);
                    _ReadOnlyInfo = default(LocalVersionInfo);
                    _ReadWriteInfo = default(LocalVersionInfo);
                }

                /// <summary>
                /// 获取资源名称。
                /// </summary>
                public ResourcesName ResourceName
                {
                    get
                    {
                        return _ResourceName;
                    }
                }

                /// <summary>
                /// 获取资源加载方式。
                /// </summary>
                public LoadType LoadType
                {
                    get
                    {
                        return _VersionInfo.LoadType;
                    }
                }

                /// <summary>
                /// 获取资源大小。
                /// </summary>
                public int Length
                {
                    get
                    {
                        return _VersionInfo.Length;
                    }
                }

                /// <summary>
                /// 获取资源哈希值。
                /// </summary>
                public int HashCode
                {
                    get
                    {
                        return _VersionInfo.HashCode;
                    }
                }

                /// <summary>
                /// 获取压缩包大小。
                /// </summary>
                public int ZipLength
                {
                    get
                    {
                        return _VersionInfo.ZipLength;
                    }
                }

                /// <summary>
                /// 获取压缩包哈希值。
                /// </summary>
                public int ZipHashCode
                {
                    get
                    {
                        return _VersionInfo.ZipHashCode;
                    }
                }

                /// <summary>
                /// 获取资源检查状态。
                /// </summary>
                public CheckStatus Status
                {
                    get
                    {
                        return _Status;
                    }
                }

                /// <summary>
                /// 获取资源是否可以从读写区移除。
                /// </summary>
                public bool NeedRemove
                {
                    get
                    {
                        return _NeedRemove;
                    }
                }

                /// <summary>
                /// 设置资源在版本中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                /// <param name="zipLength">压缩包大小。</param>
                /// <param name="zipHashCode">压缩包哈希值。</param>
                public void SetVersionInfo(LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode)
                {
                    if (_VersionInfo.Exist)
                    {
                        throw new FrameworkException(Utility.Text.Format("You must set version info of '{0}' only once.", _ResourceName.FullName));
                    }

                    _VersionInfo = new RemoteVersionInfo(loadType, length, hashCode, zipLength, zipHashCode);
                }

                /// <summary>
                /// 设置资源在只读区中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                public void SetReadOnlyInfo(LoadType loadType, int length, int hashCode)
                {
                    if (_ReadOnlyInfo.Exist)
                    {
                        throw new FrameworkException(Utility.Text.Format("You must set readonly info of '{0}' only once.", _ResourceName.FullName));
                    }

                    _ReadOnlyInfo = new LocalVersionInfo(loadType, length, hashCode);
                }

                /// <summary>
                /// 设置资源在读写区中的信息。
                /// </summary>
                /// <param name="loadType">资源加载方式。</param>
                /// <param name="length">资源大小。</param>
                /// <param name="hashCode">资源哈希值。</param>
                public void SetReadWriteInfo(LoadType loadType, int length, int hashCode)
                {
                    if (_ReadWriteInfo.Exist)
                    {
                        throw new FrameworkException(Utility.Text.Format("You must set read-write info of '{0}' only once.", _ResourceName.FullName));
                    }

                    _ReadWriteInfo = new LocalVersionInfo(loadType, length, hashCode);
                }

                /// <summary>
                /// 刷新资源信息状态。
                /// </summary>
                /// <param name="currentVariant">当前变体。</param>
                public void RefreshStatus(string currentVariant)
                {
                    if (!_VersionInfo.Exist)
                    {
                        _Status = CheckStatus.Disuse;
                        _NeedRemove = _ReadWriteInfo.Exist;
                        return;
                    }

                    if (_ResourceName.GetVariant == null || _ResourceName.GetVariant == currentVariant)
                    {
                        if (_ReadOnlyInfo.Exist && _ReadOnlyInfo.LoadType == _VersionInfo.LoadType && _ReadOnlyInfo.Length == _VersionInfo.Length && _ReadOnlyInfo.HashCode == _VersionInfo.HashCode)
                        {
                            _Status = CheckStatus.StorageInReadOnly;
                            _NeedRemove = _ReadWriteInfo.Exist;
                        }
                        else if (_ReadWriteInfo.Exist && _ReadWriteInfo.LoadType == _VersionInfo.LoadType && _ReadWriteInfo.Length == _VersionInfo.Length && _ReadWriteInfo.HashCode == _VersionInfo.HashCode)
                        {
                            _Status = CheckStatus.StorageInReadWrite;
                            _NeedRemove = false;
                        }
                        else
                        {
                            _Status = CheckStatus.NeedUpdate;
                            _NeedRemove = _ReadWriteInfo.Exist;
                        }
                    }
                    else
                    {
                        _Status = CheckStatus.Unavailable;
                        if (_ReadOnlyInfo.Exist && _ReadOnlyInfo.LoadType == _VersionInfo.LoadType && _ReadOnlyInfo.Length == _VersionInfo.Length && _ReadOnlyInfo.HashCode == _VersionInfo.HashCode)
                        {
                            _NeedRemove = _ReadWriteInfo.Exist;
                        }
                        else if (_ReadWriteInfo.Exist && _ReadWriteInfo.LoadType == _VersionInfo.LoadType && _ReadWriteInfo.Length == _VersionInfo.Length && _ReadWriteInfo.HashCode == _VersionInfo.HashCode)
                        {
                            _NeedRemove = false;
                        }
                        else
                        {
                            _NeedRemove = _ReadWriteInfo.Exist;
                        }
                    }
                }
            }

            /// <summary>
            /// 检查状态
            /// </summary>
            public enum CheckStatus
            {
                /// <summary>
                    /// 状态未知。
                    /// </summary>
                    Unknown = 0,

                    /// <summary>
                    /// 需要更新。
                    /// </summary>
                    NeedUpdate,

                    /// <summary>
                    /// 存在最新且已存放于只读区中。
                    /// </summary>
                    StorageInReadOnly,

                    /// <summary>
                    /// 存在最新且已存放于读写区中。
                    /// </summary>
                    StorageInReadWrite,

                    /// <summary>
                    /// 不适用于当前变体。
                    /// </summary>
                    Unavailable,

                    /// <summary>
                    /// 已废弃。
                    /// </summary>
                    Disuse
            }

            /// <summary>
            /// 远程资源信息
            /// </summary>
            private struct RemoteVersionInfo{
                private readonly bool _Exist;
                private readonly LoadType _LoadType;
                private readonly int _Length;
                private readonly int _HashCode;
                private readonly int _ZipLength;
                private readonly int _ZipHashCode;

                public RemoteVersionInfo(LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode)
                {
                    _Exist = true;
                    _LoadType = loadType;
                    _Length = length;
                    _HashCode = hashCode;
                    _ZipLength = zipLength;
                    _ZipHashCode = zipHashCode;
                }

                public bool Exist
                {
                    get
                    {
                        return _Exist;
                    }
                }

                public LoadType LoadType
                {
                    get
                    {
                        return _LoadType;
                    }
                }

                public int Length
                {
                    get
                    {
                        return _Length;
                    }
                }

                public int HashCode
                {
                    get
                    {
                        return _HashCode;
                    }
                }

                public int ZipLength
                {
                    get
                    {
                        return _ZipLength;
                    }
                }

                public int ZipHashCode
                {
                    get
                    {
                        return _ZipHashCode;
                    }
                }
            }

            /// <summary>
            /// 本地资源信息
            /// </summary>
            private struct LocalVersionInfo{
                private readonly bool _Exist;
                private readonly LoadType _LoadType;
                private readonly int _Length;
                private readonly int _HashCode;

                public LocalVersionInfo(LoadType loadType, int length, int hashCode)
                {
                    _Exist = true;
                    _LoadType = loadType;
                    _Length = length;
                    _HashCode = hashCode;
                }

                public bool Exist
                {
                    get
                    {
                        return _Exist;
                    }
                }

                public LoadType LoadType
                {
                    get
                    {
                        return _LoadType;
                    }
                }

                public int Length
                {
                    get
                    {
                        return _Length;
                    }
                }

                public int HashCode
                {
                    get
                    {
                        return _HashCode;
                    }
                }
            }
        }
    }
}
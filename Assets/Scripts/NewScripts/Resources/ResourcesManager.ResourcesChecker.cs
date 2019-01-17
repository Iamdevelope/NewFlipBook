using System;
using System.Collections.Generic;
using System.IO;

namespace PJW.Resources
{
    internal partial class ResourcesManager
    {
        /// <summary>
        /// 资源检查器
        /// </summary>
        private sealed partial class ResourcesChecker
        {
            private readonly ResourcesManager _ResourcesManager;
            private readonly Dictionary<ResourcesName, CheckInfo> _CheckInfos;
            private string _CurrentVariant;
            private bool _VersionListReady;
            private bool _ReadOnlyListReady;
            private bool _ReadWriteListReady;
            public FrameworkAction<ResourcesName,LoadType,int,int,int,int> ResourcesNeedUpdate;
            public FrameworkAction<int,int,int,int> ResourcesCheckComplete;

            public ResourcesChecker(ResourcesManager resourcesManager){
                _ResourcesManager=resourcesManager;
                _CheckInfos=new Dictionary<ResourcesName, CheckInfo>();
                _CurrentVariant=null;
                _VersionListReady=false;
                _ReadOnlyListReady=false;
                _ReadWriteListReady=false;

                ResourcesNeedUpdate=null;
                ResourcesCheckComplete=null;
            }
            /// <summary>
            /// 关闭并清理资源检查器。
            /// </summary>
            public void Shutdown()
            {
                _CheckInfos.Clear();
            }

            public void CheckResources(string currentVariant)
            {
                _CurrentVariant = currentVariant;

                TryRecoverReadWriteList();

                if (_ResourcesManager._ResourcesHelper == null)
                {
                    throw new FrameworkException("Resource helper is invalid ");
                }

                _ResourcesManager._ResourcesHelper.LoadBytes(Utility.Path.GetRemotePath(_ResourcesManager._ReadWritePath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParseVersionList);
                _ResourcesManager._ResourcesHelper.LoadBytes(Utility.Path.GetRemotePath(_ResourcesManager._ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName)), ParseReadOnlyList);
                _ResourcesManager._ResourcesHelper.LoadBytes(Utility.Path.GetRemotePath(_ResourcesManager._ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName)), ParseReadWriteList);
            }

            private void SetVersionInfo(ResourcesName resourceName, LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode)
            {
                GetOrAddCheckInfo(resourceName).SetVersionInfo(loadType, length, hashCode, zipLength, zipHashCode);
            }

            private void SetReadOnlyInfo(ResourcesName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadOnlyInfo(loadType, length, hashCode);
            }

            private void SetReadWriteInfo(ResourcesName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadWriteInfo(loadType, length, hashCode);
            }

            private CheckInfo GetOrAddCheckInfo(ResourcesName resourceName)
            {
                CheckInfo checkInfo = null;
                if (_CheckInfos.TryGetValue(resourceName, out checkInfo))
                {
                    return checkInfo;
                }

                checkInfo = new CheckInfo(resourceName);
                _CheckInfos.Add(checkInfo.ResourceName, checkInfo);

                return checkInfo;
            }

            private void RefreshCheckInfoStatus()
            {
                if (!_VersionListReady || !_ReadOnlyListReady || !_ReadWriteListReady)
                {
                    return;
                }

                int removedCount = 0;
                int updateCount = 0;
                int updateTotalLength = 0;
                int updateTotalZipLength = 0;
                foreach (KeyValuePair<ResourcesName, CheckInfo> checkInfo in _CheckInfos)
                {
                    CheckInfo ci = checkInfo.Value;
                    ci.RefreshStatus(_CurrentVariant);

                    if (ci.Status == CheckStatus.StorageInReadOnly)
                    {
                        ProcessResourceInfo(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode, true);
                    }
                    else if (ci.Status == CheckStatus.StorageInReadWrite)
                    {
                        ProcessResourceInfo(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode, false);
                    }
                    else if (ci.Status == CheckStatus.NeedUpdate)
                    {
                        updateCount++;
                        updateTotalLength += ci.Length;
                        updateTotalZipLength += ci.ZipLength;

                        ResourcesNeedUpdate(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode, ci.ZipLength, ci.ZipHashCode);
                    }
                    else if (ci.Status == CheckStatus.Disuse || ci.Status == CheckStatus.Unavailable)
                    {
                        // Do nothing.
                    }
                    else
                    {
                        throw new FrameworkException(Utility.Text.Format("Check resources {0} error with unknown status ", ci.ResourceName.FullName));
                    }

                    if (ci.NeedRemove)
                    {
                        removedCount++;

                        string path = Utility.Path.GetCombinePath(_ResourcesManager._ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ci.ResourceName.FullName));
                        File.Delete(path);

                        if (!_ResourcesManager._ReadWriteResourcesInfos.ContainsKey(ci.ResourceName))
                        {
                            throw new FrameworkException(Utility.Text.Format("Resource {0} is not exist in read-write list ", ci.ResourceName.FullName));
                        }

                        _ResourcesManager._ReadWriteResourcesInfos.Remove(ci.ResourceName);
                    }
                }

                ResourcesCheckComplete(removedCount, updateCount, updateTotalLength, updateTotalZipLength);
            }

            /// <summary>
            /// 尝试恢复读写区资源列表。
            /// </summary>
            /// <returns>是否恢复成功。</returns>
            private bool TryRecoverReadWriteList()
            {
                string file = Utility.Path.GetCombinePath(_ResourcesManager._ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName));
                string backupFile = file + BackupFileSuffixName;

                try
                {
                    if (!File.Exists(backupFile))
                    {
                        return false;
                    }

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    File.Move(backupFile, file);
                }
                catch
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 解析版本资源列表。
            /// </summary>
            /// <param name="fileUri">版本资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParseVersionList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (_VersionListReady)
                {
                    throw new FrameworkException("Version list has been parsed ");
                }

                if (bytes == null || bytes.Length <= 0)
                {
                    throw new FrameworkException(Utility.Text.Format("Version list {0} is invalid, error message is {1} ", fileUri, string.IsNullOrEmpty(errorMessage) ? "Empty" : errorMessage));
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        memoryStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != VersionListHeader[0] || header[1] != VersionListHeader[1] || header[2] != VersionListHeader[2])
                        {
                            throw new FrameworkException("Version list header is invalid ");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);
                            _ResourcesManager._ApplicableGameVersion = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                            _ResourcesManager._InternalResourcesVersion = binaryReader.ReadInt32();

                            int resourceCount = binaryReader.ReadInt32();
                            string[] names = new string[resourceCount];
                            string[] variants = new string[resourceCount];
                            int[] lengths = new int[resourceCount];
                            Dictionary<string, string[]> dependencyAssetNamesCollection = new Dictionary<string, string[]>();
                            for (int i = 0; i < resourceCount; i++)
                            {
                                names[i] = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                variants[i] = null;
                                byte variantLength = binaryReader.ReadByte();
                                if (variantLength > 0)
                                {
                                    variants[i] = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(variantLength), encryptBytes));
                                }

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                lengths[i] = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();
                                int zipLength = binaryReader.ReadInt32();
                                int zipHashCode = binaryReader.ReadInt32();

                                int assetNamesCount = binaryReader.ReadInt32();
                                string[] assetNames = new string[assetNamesCount];
                                for (int j = 0; j < assetNamesCount; j++)
                                {
                                    assetNames[j] = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));

                                    int dependencyAssetNamesCount = binaryReader.ReadInt32();
                                    string[] dependencyAssetNames = new string[dependencyAssetNamesCount];
                                    for (int k = 0; k < dependencyAssetNamesCount; k++)
                                    {
                                        dependencyAssetNames[k] = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));
                                    }

                                    if (variants[i] == null || variants[i] == _CurrentVariant)
                                    {
                                        dependencyAssetNamesCollection.Add(assetNames[j], dependencyAssetNames);
                                    }
                                }

                                ResourcesName resourceName = new ResourcesName(names[i], variants[i]);
                                SetVersionInfo(resourceName, loadType, lengths[i], hashCode, zipLength, zipHashCode);
                                if (variants[i] == null || variants[i] == _CurrentVariant)
                                {
                                    ProcessAssetInfo(resourceName, assetNames);
                                }
                            }

                            ProcessAssetDependencyInfo(dependencyAssetNamesCollection);

                            ResourcesGroup resourceGroupAll = _ResourcesManager.GetResourcesGroup(string.Empty);
                            for (int i = 0; i < resourceCount; i++)
                            {
                                resourceGroupAll.AddResource(names[i], variants[i], lengths[i]);
                            }

                            int resourceGroupCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceGroupCount; i++)
                            {
                                string groupName = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                                ResourcesGroup resourceGroup = _ResourcesManager.GetResourcesGroup(groupName);
                                int groupResourceCount = binaryReader.ReadInt32();
                                for (int j = 0; j < groupResourceCount; j++)
                                {
                                    ushort versionIndex = binaryReader.ReadUInt16();
                                    if (versionIndex >= resourceCount)
                                    {
                                        throw new FrameworkException(Utility.Text.Format("Version index {0} is invalid, resource count is {1} ", versionIndex, resourceCount));
                                    }

                                    resourceGroup.AddResource(names[versionIndex], variants[versionIndex], lengths[versionIndex]);
                                }
                            }
                        }
                        else
                        {
                            throw new FrameworkException("Version list version is invalid ");
                        }
                    }

                    _VersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is FrameworkException)
                    {
                        throw;
                    }

                    throw new FrameworkException(Utility.Text.Format("Parse version list exception {0} ", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            /// <summary>
            /// 解析只读区资源列表。
            /// </summary>
            /// <param name="fileUri">只读区资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParseReadOnlyList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (_ReadOnlyListReady)
                {
                    throw new FrameworkException("Readonly list has been parsed ");
                }

                if (bytes == null || bytes.Length <= 0)
                {
                    _ReadOnlyListReady = true;
                    RefreshCheckInfoStatus();
                    return;
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        memoryStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != ReadOnlyListHeader[0] || header[1] != ReadOnlyListHeader[1] || header[2] != ReadOnlyListHeader[2])
                        {
                            throw new FrameworkException("Readonly list header is invalid ");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            int resourceCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceCount; i++)
                            {
                                string name = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                string variant = null;
                                byte variantLength = binaryReader.ReadByte();
                                if (variantLength > 0)
                                {
                                    variant = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(variantLength), encryptBytes));
                                }

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                int length = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();

                                SetReadOnlyInfo(new ResourcesName(name, variant), loadType, length, hashCode);
                            }
                        }
                        else
                        {
                            throw new FrameworkException("Readonly list version is invalid ");
                        }
                    }

                    _ReadOnlyListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is FrameworkException)
                    {
                        throw;
                    }

                    throw new FrameworkException(Utility.Text.Format("Parse readonly list exception {0} ", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            /// <summary>
            /// 解析读写区资源列表。
            /// </summary>
            /// <param name="fileUri">读写区资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParseReadWriteList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (_ReadWriteListReady)
                {
                    throw new FrameworkException("Read-write list has been parsed ");
                }

                if (bytes == null || bytes.Length <= 0)
                {
                    _ReadWriteListReady = true;
                    RefreshCheckInfoStatus();
                    return;
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        memoryStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != ReadWriteListHeader[0] || header[1] != ReadWriteListHeader[1] || header[2] != ReadWriteListHeader[2])
                        {
                            throw new FrameworkException("Read-write list header is invalid ");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            int resourceCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceCount; i++)
                            {
                                string name = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                string variant = null;
                                byte variantLength = binaryReader.ReadByte();
                                if (variantLength > 0)
                                {
                                    variant = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(variantLength), encryptBytes));
                                }

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                int length = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();

                                SetReadWriteInfo(new ResourcesName(name, variant), loadType, length, hashCode);

                                ResourcesName resourceName = new ResourcesName(name, variant);
                                if (_ResourcesManager._ReadWriteResourcesInfos.ContainsKey(resourceName))
                                {
                                    throw new FrameworkException(Utility.Text.Format("Read-write resource info {0} is already exist ", resourceName.FullName));
                                }

                                _ResourcesManager._ReadWriteResourcesInfos.Add(resourceName, new ReadWriteResourcesInfo(loadType, length, hashCode));
                            }
                        }
                        else
                        {
                            throw new FrameworkException("Read-write list version is invalid ");
                        }
                    }

                    _ReadWriteListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is FrameworkException)
                    {
                        throw;
                    }

                    throw new FrameworkException(Utility.Text.Format("Parse read-write list exception {0} ", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void ProcessAssetInfo(ResourcesName resourceName, string[] assetNames)
            {
                foreach (string assetName in assetNames)
                {
                    int childNamePosition = assetName.LastIndexOf('/');
                    if (childNamePosition < 0 || childNamePosition + 1 >= assetName.Length)
                    {
                        throw new FrameworkException(Utility.Text.Format("Asset name {0} is invalid ", assetName));
                    }

                    _ResourcesManager._AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName, assetName.Substring(childNamePosition + 1)));
                }
            }

            private void ProcessAssetDependencyInfo(Dictionary<string, string[]> dependencyAssetNamesCollection)
            {
                foreach (KeyValuePair<string, string[]> dependencyAssetNamesCollectionItem in dependencyAssetNamesCollection)
                {
                    List<string> dependencyAssetNames = new List<string>();
                    List<string> scatteredDependencyAssetNames = new List<string>();
                    foreach (string dependencyAssetName in dependencyAssetNamesCollectionItem.Value)
                    {
                        AssetInfo? assetInfo = _ResourcesManager.GetAssetInfo(dependencyAssetName);
                        if (assetInfo.HasValue)
                        {
                            dependencyAssetNames.Add(dependencyAssetName);
                        }
                        else
                        {
                            scatteredDependencyAssetNames.Add(dependencyAssetName);
                        }
                    }

                    _ResourcesManager._AssetDependencyInfos.Add(dependencyAssetNamesCollectionItem.Key, new AssetDependencyInfo(dependencyAssetNamesCollectionItem.Key, dependencyAssetNames.ToArray(), scatteredDependencyAssetNames.ToArray()));
                }
            }

            private void ProcessResourceInfo(ResourcesName resourceName, LoadType loadType, int length, int hashCode, bool storageInReadOnly)
            {
                if (_ResourcesManager._ResourcesInfos.ContainsKey(resourceName))
                {
                    throw new FrameworkException(Utility.Text.Format("Resource info {0} is already exist ", resourceName.FullName));
                }

                _ResourcesManager._ResourcesInfos.Add(resourceName, new ResourcesInfo(resourceName, loadType, length, hashCode, storageInReadOnly));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using PJW.Download;
using System.IO;

namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 资源更新器
        /// </summary>
        private sealed partial class ResourcesUpdater
        {
            private readonly ResourcesManager _ResourcesManager;
            private readonly List<UpdateInfo> _UpdateWaitingInfo;
            private IDownLoadManager _DownloadManager;
            private bool _CheckResourcesComplete;
            private bool _UpdateAllowed;
            private bool _UpdateComplete;
            private int _RetryCount;
            private int _UpdatingCount;

            public FrameworkAction<ResourcesName, string, string, int, int, int> ResourcesUpdateStart;
            public FrameworkAction<ResourcesName, string, string, int, int> ResourcesUpdateChanged;
            public FrameworkAction<ResourcesName, string, string, int, int> ResourcesUpdateSuccess;
            public FrameworkAction<ResourcesName, string, int, int, string> ResourcesUpdateFailure;
            public FrameworkAction ResourcesUpdateAllComplete;

            /// <summary>
            /// 初始化资源更新器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourcesUpdater(ResourcesManager resourcesManager)
            {
                _ResourcesManager = resourcesManager;
                _UpdateWaitingInfo = new List<UpdateInfo>();
                _DownloadManager = null;
                _CheckResourcesComplete = false;
                _UpdateAllowed = false;
                _UpdateComplete = false;
                _RetryCount = 3;
                _UpdatingCount = 0;

                ResourcesUpdateStart = null;
                ResourcesUpdateChanged = null;
                ResourcesUpdateSuccess = null;
                ResourcesUpdateFailure = null;
                ResourcesUpdateAllComplete = null;
            }

            /// <summary>
            /// 获取或设置资源更新重试次数。
            /// </summary>
            /// <value></value>
            public int RetryCount
            {
                get
                {
                    return _RetryCount;
                }
                set
                {
                    _RetryCount=value;
                }
            }

            /// <summary>
            /// 获取等待更新队列大小
            /// </summary>
            /// <value></value>
            public int UpdateWaitintCount
            {
                get
                {
                    return _UpdateWaitingInfo.Count;
                }
            }

            /// <summary>
            /// 正在更新队列大小
            /// </summary>
            /// <value></value>
            public int UpdatingCount
            {
                get
                {
                    return _UpdatingCount;
                }
            }

            /// <summary>
            /// 资源更新器轮询
            /// </summary>
            /// <param name="elapseSeconds"></param>
            /// <param name="realElapseSeconds"></param>
            public void Update(float elapseSeconds,float realElapseSeconds)
            {
                if(_UpdateAllowed&&!_UpdateComplete)
                {
                    if(_UpdateWaitingInfo.Count>0)
                    {
                        if(_DownloadManager.CanuseAgentCount>0)
                        {
                            UpdateInfo updateInfo=_UpdateWaitingInfo[0];
                            _UpdateWaitingInfo.RemoveAt(0);
                            _DownloadManager.AddDownload(updateInfo.GetSavePath,updateInfo.GetDownloadUrl);
                            _UpdatingCount++;
                        }
                    }
                    else if(_UpdatingCount<=0)
                    {
                        _UpdateComplete=true;
                        Utility.Path.RemoveEmptyDirectory(_ResourcesManager._ReadWritePath);
                        if(ResourcesUpdateAllComplete!=null)
                        {
                            ResourcesUpdateAllComplete();
                        }
                    }
                }
            }

            /// <summary>
            /// 关闭并清理资源更新器
            /// </summary>
            public void Shutdown()
            {
                if(_DownloadManager!=null)
                {
                    _DownloadManager.DownLoadFailureHandler-=OnDownloadFailure;
                    _DownloadManager.DownLoadStartHandler-=OnDownloadStart;
                    _DownloadManager.DownLoadSuccessHandler-=OnDownloadSuccess;
                    _DownloadManager.DownLoadUpdateHandler-=OnDownloadUpdate;
                }
                _UpdateWaitingInfo.Clear();
            }

            /// <summary>
            /// 设置下载管理器
            /// </summary>
            /// <param name="downLoadManager"></param>
            public void SetDownloadManager(IDownLoadManager downLoadManager)
            {
                if(downLoadManager==null)
                {
                    throw new FrameworkException(" Download manager is invalid ");
                }
                _DownloadManager.DownLoadFailureHandler+=OnDownloadFailure;
                _DownloadManager.DownLoadStartHandler+=OnDownloadStart;
                _DownloadManager.DownLoadSuccessHandler+=OnDownloadSuccess;
                _DownloadManager.DownLoadUpdateHandler+=OnDownloadUpdate;
            }

            /// <summary>
            /// 增加资源更新
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
            public void AddResourcesUpdate(ResourcesName resourcesName, LoadType loadType, int length, int hashCode, 
            int zipLength, int zipHashCode, string savePath, string downloadUrl, int retryCount)
            {
                _UpdateWaitingInfo.Add(new UpdateInfo(resourcesName,loadType,length,hashCode,zipLength,zipHashCode,savePath,downloadUrl,retryCount));
            }

            /// <summary>
            /// 检查资源是否更新完成
            /// </summary>
            /// <param name="needGenerateReadWriteList">是否需要生成读写资源列表</param>
            public void CheckResourcesComplete(bool needGenerateReadWriteList)
            {
                _CheckResourcesComplete=true;
                if(needGenerateReadWriteList)
                {
                    GenerateReadWriteList();
                }
            }

            /// <summary>
            /// 更新资源
            /// </summary>
            public void UpdateResources()
            {
                if(_ResourcesManager==null)
                {
                    throw new FrameworkException(" You must set resources manager first ");
                }
                if(!_CheckResourcesComplete)
                {
                    throw new FrameworkException(" You must check resources complete first ");
                }
                _UpdateAllowed=true;
            }

            /// <summary>
            /// 生成读写资源列表
            /// </summary>
            private void GenerateReadWriteList()
            {
                string file=Utility.Path.GetCombinePath(_ResourcesManager._ReadWritePath,Utility.Path.GetResourceNameWithSuffix(ResourceListFileName));
                string backupFile=null;
                if(File.Exists(file))
                {
                    backupFile=file+BackupFileSuffixName;
                    if(File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                    File.Move(file,backupFile);
                }
                FileStream fileStream=null;
                try
                {
                    fileStream=new FileStream(file,FileMode.CreateNew,FileAccess.Write);
                    using(BinaryWriter binaryWriter=new BinaryWriter(fileStream))
                    {
                        fileStream=null;
                        byte[] encrytCode=new byte[4];
                        Utility.Random.GetRandomBytes(encrytCode);

                        binaryWriter.Write(ReadWriteListHeader);
                        binaryWriter.Write(ReadWriteListVersionHeader);
                        binaryWriter.Write(encrytCode);
                        binaryWriter.Write(_ResourcesManager._ReadWriteResourcesInfos.Count);
                        foreach (KeyValuePair<ResourcesName,ReadWriteResourcesInfo> item in _ResourcesManager._ReadWriteResourcesInfos)
                        {
                            byte[] nameBytes=Utility.Encryption.GetSelfXorBytes(Utility.Converter.GetBytes(item.Key.GetVariant),encrytCode);
                            binaryWriter.Write((byte)nameBytes.Length);
                            binaryWriter.Write(nameBytes);

                            if(item.Key.GetVariant==null)
                            {
                                binaryWriter.Write((byte)0);
                            }
                            else
                            {
                                byte[] variantBytes=Utility.Encryption.GetSelfXorBytes(Utility.Converter.GetBytes(item.Key.GetVariant),encrytCode);
                                binaryWriter.Write((byte)variantBytes.Length);
                                binaryWriter.Write(variantBytes);
                            }
                            binaryWriter.Write((byte)item.Value.GetLoadType);
                            binaryWriter.Write(item.Value.GetLength);
                            binaryWriter.Write(item.Value.GetHashCode);
                        }
                    }
                    if(!string.IsNullOrEmpty(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                }
                catch(Exception e)
                {
                    if(File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    if(!string.IsNullOrEmpty(backupFile))
                    {
                        File.Move(backupFile,file);
                    }
                    throw new FrameworkException(Utility.Text.Format(" Pack save exception {0} ",e.Message),e);
                }
                finally
                {
                    if(fileStream!=null)
                    {
                        fileStream.Dispose();
                        fileStream=null;
                    }
                }
            }

            /// <summary>
            /// 下载失败
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (File.Exists(e.DownloadPath))
                {
                    File.Delete(e.DownloadPath);
                }

                if (ResourcesUpdateFailure != null)
                {
                    ResourcesUpdateFailure(updateInfo.GetResourcesName, e.DownloadUrl, updateInfo.GetRetryCount, RetryCount, e.ErrorMessage);
                }

                if (updateInfo.GetRetryCount < _RetryCount)
                {
                    _UpdatingCount--;
                    UpdateInfo newUpdateInfo = new UpdateInfo(updateInfo.GetResourcesName, updateInfo.GetLoadType, updateInfo.GetLength, updateInfo.GetHashCode, updateInfo.GetZipLength, updateInfo.GetZipHashCode, updateInfo.GetSavePath, updateInfo.GetDownloadUrl, updateInfo.GetRetryCount + 1);
                    if (_UpdateAllowed)
                    {
                        _UpdateWaitingInfo.Add(newUpdateInfo);
                    }
                    else
                    {
                        throw new FrameworkException("Update state error.");
                    }
                }
            }

            /// <summary>
            /// 下载开始
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnDownloadStart(object sender, DownloadStartEventArgs e)
            {
                UpdateInfo updateInfo=e.UserData as UpdateInfo;
                if(updateInfo==null)
                {
                    return;
                }
                if(_DownloadManager==null)
                {
                    throw new FrameworkException(" You must set download manager first ");
                }
                if(e.CurrentLength>updateInfo.GetZipLength)
                {
                    _DownloadManager.RemoveDownload(e.SerialId);
                    string downloadFile=Utility.Text.Format("{0}.download",e.DownloadPath);
                    if(File.Exists(downloadFile)){
                        File.Delete(downloadFile);
                    }
                    string errorMessage=Utility.Text.Format(" When download start, downloaded length is larger than zip length, need {0}, current {1} .",updateInfo.GetZipLength.ToString(),e.CurrentLength.ToString());
                    OnDownloadFailure(this,new DownloadFailureEventArgs(e.SerialId,e.DownloadPath,e.DownloadUrl,errorMessage,e.UserData));
                    return;
                }
                if(ResourcesUpdateStart!=null){
                    ResourcesUpdateStart(updateInfo.GetResourcesName,e.DownloadPath,e.DownloadUrl,e.CurrentLength,updateInfo.GetZipLength,updateInfo.GetRetryCount);
                }
            }

            /// <summary>
            /// 下载成功
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                bool zip = (updateInfo.GetLength != updateInfo.GetZipLength || updateInfo.GetHashCode != updateInfo.GetZipHashCode);
                byte[] bytes = File.ReadAllBytes(e.DownloadPath);

                if (updateInfo.GetZipLength != bytes.Length)
                {
                    string errorMessage = Utility.Text.Format("Zip length error, need '{0}', downloaded '{1}'.", updateInfo.GetZipLength.ToString(), bytes.Length.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUrl, errorMessage, e.UserData));
                    return;
                }

                if (!zip)
                {
                    byte[] hashBytes = Utility.Converter.GetBytes(updateInfo.GetHashCode);
                    if (updateInfo.GetLoadType == LoadType.LoadFromMemoryAndQuickDecrypt)
                    {
                        Utility.Encryption.GetQuickSelfXorBytes(bytes, hashBytes);
                    }
                    else if (updateInfo.GetLoadType == LoadType.LoadFromMemoryAndDecrypt)
                    {
                        Utility.Encryption.GetSelfXorBytes(bytes, hashBytes);
                    }
                }

                int hashCode = Utility.Converter.GetInt32(Utility.Verifier.GetCrc32(bytes));
                if (updateInfo.GetZipHashCode != hashCode)
                {
                    string errorMessage = Utility.Text.Format("Zip hash code error, need '{0}', downloaded '{1}'.", updateInfo.GetZipHashCode.ToString("X8"), hashCode.ToString("X8"));
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUrl, errorMessage, e.UserData));
                    return;
                }

                if (zip)
                {
                    try
                    {
                        bytes = Utility.Zip.Decompress(bytes);
                    }
                    catch (Exception exception)
                    {
                        string errorMessage = Utility.Text.Format("Unable to decompress from file '{0}' with error message '{1}'.", e.DownloadPath, exception.Message);
                        OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUrl, errorMessage, e.UserData));
                        return;
                    }

                    if (bytes == null)
                    {
                        string errorMessage = Utility.Text.Format("Unable to decompress from file '{0}'.", e.DownloadPath);
                        OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUrl, errorMessage, e.UserData));
                        return;
                    }

                    if (updateInfo.GetLength != bytes.Length)
                    {
                        string errorMessage = Utility.Text.Format("Resource length error, need '{0}', downloaded '{1}'.", updateInfo.GetLength.ToString(), bytes.Length.ToString());
                        OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUrl, errorMessage, e.UserData));
                        return;
                    }

                    File.WriteAllBytes(e.DownloadPath, bytes);
                }

                _UpdatingCount--;

                if (_ResourcesManager._ResourcesInfos.ContainsKey(updateInfo.GetResourcesName))
                {
                    throw new FrameworkException(Utility.Text.Format("Resource info '{0}' is already exist.", updateInfo.GetResourcesName.FullName));
                }

                _ResourcesManager._ResourcesInfos.Add(updateInfo.GetResourcesName, new ResourcesInfo(updateInfo.GetResourcesName, updateInfo.GetLoadType, updateInfo.GetLength, updateInfo.GetHashCode, false));

                if (_ResourcesManager._ReadWriteResourcesInfos.ContainsKey(updateInfo.GetResourcesName))
                {
                    throw new FrameworkException(Utility.Text.Format("Read-write resource info '{0}' is already exist.", updateInfo.GetResourcesName.FullName));
                }

                _ResourcesManager._ReadWriteResourcesInfos.Add(updateInfo.GetResourcesName, new ReadWriteResourcesInfo(updateInfo.GetLoadType, updateInfo.GetLength, updateInfo.GetHashCode));

                GenerateReadWriteList();

                if (ResourcesUpdateSuccess != null)
                {
                    ResourcesUpdateSuccess(updateInfo.GetResourcesName, e.DownloadPath, e.DownloadUrl, updateInfo.GetLength, updateInfo.GetZipLength);
                }
            }

            /// <summary>
            /// 下载更新
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnDownloadUpdate(object sender, DownloadUpdateEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (_DownloadManager == null)
                {
                    throw new FrameworkException("You must set download manager first.");
                }

                if (e.CurrentLength > updateInfo.GetZipLength)
                {
                    _DownloadManager.RemoveDownload(e.SerialId);
                    string downloadFile = Utility.Text.Format("{0}.download", e.DownloadPath);
                    if (File.Exists(downloadFile))
                    {
                        File.Delete(downloadFile);
                    }

                    string errorMessage = Utility.Text.Format("When download update, downloaded length is larger than zip length, need '{0}', current '{1}'.", updateInfo.GetZipLength.ToString(), e.CurrentLength.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUrl, errorMessage, e.UserData));
                    return;
                }

                if (ResourcesUpdateChanged != null)
                {
                    ResourcesUpdateChanged(updateInfo.GetResourcesName, e.DownloadPath, e.DownloadUrl, e.CurrentLength, updateInfo.GetZipLength);
                }
            }
        }
    }
}
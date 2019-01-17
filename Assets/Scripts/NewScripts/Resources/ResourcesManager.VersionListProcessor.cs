using System;
using System.IO;
using PJW.Download;

namespace PJW.Resources
{
    internal sealed partial class ResourcesManager
    {
        /// <summary>
        /// 版本资源列表处理器
        /// </summary>
        private sealed class VersionListProcessor
        {
            private readonly ResourcesManager _ResourcesManager;
            private IDownLoadManager _DownloadManager;
            private int _VersionListLength;
            private int _VersionListHashCode;
            private int _VersionListZipLength;
            private int _VersionListZipHashCode;
            public FrameworkAction<string,string> VersionListUpdateSuccess;
            public FrameworkAction<string,string> VersionListUpdateFailure;
            public VersionListProcessor(ResourcesManager resourcesManager){
                _ResourcesManager=resourcesManager;
                _DownloadManager=null;
                _VersionListLength=0;
                _VersionListHashCode=0;
                _VersionListZipLength=0;
                _VersionListZipHashCode=0;
                VersionListUpdateFailure=null;
                VersionListUpdateSuccess=null;
            }

            /// <summary>
            /// 关闭并清理资源列表处理器
            /// </summary>
            public void Shutdown(){
                if(_DownloadManager!=null){
                    _DownloadManager.DownLoadFailureHandler-=OnDownloadFailure;
                    _DownloadManager.DownLoadSuccessHandler-=OnDownloadSuccess;
                }
            }

            /// <summary>
            /// 设置下载管理器
            /// </summary>
            /// <param name="downloadManager"></param>
            public void SetDownloadManager(IDownLoadManager downloadManager){
                if(downloadManager==null){
                    throw new FrameworkException(" Download manager is invalid ");
                }
                _DownloadManager=downloadManager;
                _DownloadManager.DownLoadFailureHandler+=OnDownloadFailure;
                _DownloadManager.DownLoadSuccessHandler+=OnDownloadSuccess;
            }

            /// <summary>
            /// 更新资源列表
            /// </summary>
            /// <param name="versionListLength">资源列表大小</param>
            /// <param name="versionListHashCode">版本资源列表哈希值</param>
            /// <param name="versionListZipLength">版本资源列表压缩后大小</param>
            /// <param name="versionListZipHashCode">版本资源列表压缩后哈希值</param>
            public void UpdateVersionList(int versionListLength,int versionListHashCode,int versionListZipLength,int versionListZipHashCode){
                if(_DownloadManager==null){
                    throw new FrameworkException(" You must set download manager first ");
                }
                _VersionListLength=versionListLength;
                _VersionListHashCode=versionListHashCode;
                _VersionListZipLength=versionListZipLength;
                _VersionListZipHashCode=versionListZipHashCode;
                string versionListFileName=Utility.Path.GetResourceNameWithSuffix(VersionListFileName);
                string latestVersionListFileName=Utility.Path.GetResourceNameWithCrc32AndSuffix(VersionListFileName,_VersionListHashCode);
                string localVersionListFilePath=Utility.Path.GetCombinePath(_ResourcesManager._ReadWritePath,versionListFileName);
                string latestVersionListFileUrl=Utility.Path.GetRemotePath(_ResourcesManager._UpdatePrefixUrl,latestVersionListFileName);
                _DownloadManager.AddDownload(localVersionListFilePath,latestVersionListFileUrl,this);
            }

            /// <summary>
            /// 检查版本资源
            /// </summary>
            /// <param name="latestInternalResourcesVersion">最新的内部资源版本号</param>
            /// <returns>检查版本资源列表结果</returns>
            public CheckVersionListResult CheckVersionList(int latestInternalResourcesVersion){
                string applicableGameVersion = null;
                int internalResourceVersion = 0;

                string versionListFileName = Utility.Path.GetCombinePath(_ResourcesManager._ReadWritePath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName));
                if (!File.Exists(versionListFileName))
                {
                    FrameworkLog.Debug("Latest internal resource version is '{0}', local resource version is not exist.", latestInternalResourcesVersion.ToString());
                    return CheckVersionListResult.NeedUpdate;
                }

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(versionListFileName, FileMode.Open, FileAccess.Read);
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        fileStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != VersionListHeader[0] || header[1] != VersionListHeader[1] || header[2] != VersionListHeader[2])
                        {
                            FrameworkLog.Debug("Latest internal resource version is '{0}', local resource version is invalid.", latestInternalResourcesVersion.ToString());
                            return CheckVersionListResult.NeedUpdate;
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);
                            applicableGameVersion = Utility.Converter.GetString(Utility.Encryption.GetSelfXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                            internalResourceVersion = binaryReader.ReadInt32();
                        }
                        else
                        {
                            throw new FrameworkException("Version list version is invalid.");
                        }
                    }
                }
                catch
                {
                    FrameworkLog.Debug("Latest internal resource version is '{0}', local resource version is invalid.", latestInternalResourcesVersion.ToString());
                    return CheckVersionListResult.NeedUpdate;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }

                if (internalResourceVersion != latestInternalResourcesVersion)
                {
                    FrameworkLog.Debug("Applicable game version is '{0}', latest internal resource version is '{1}', local resource version is '{2}'.", applicableGameVersion, latestInternalResourcesVersion.ToString(), internalResourceVersion.ToString());
                    return CheckVersionListResult.NeedUpdate;
                }

                FrameworkLog.Debug("Applicable game version is '{0}', latest internal resource version is '{1}', local resource version is up-to-date.", applicableGameVersion, latestInternalResourcesVersion.ToString());
                return CheckVersionListResult.Updated;
            }

            /// <summary>
            /// 下载失败处理事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                VersionListProcessor sersionListProcessor=e.UserData as VersionListProcessor;
                if(sersionListProcessor==null||sersionListProcessor!=this){
                    return;
                }
                if(File.Exists(e.DownloadPath)){
                    File.Delete(e.DownloadPath);
                }
                if(VersionListUpdateFailure!=null){
                    VersionListUpdateFailure(e.DownloadUrl,e.ErrorMessage);
                }
            }

            /// <summary>
            /// 下载成功处理事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                VersionListProcessor sersionListProcessor=e.UserData as VersionListProcessor;
                if(sersionListProcessor==null||sersionListProcessor!=this){
                    return;
                }
                byte[] bytes=File.ReadAllBytes(e.DownloadPath);
                if(_VersionListZipLength!=bytes.Length){
                    string errorMessage=Utility.Text.Format("Latest version list zip length error , need length {0} but download length {1}",_VersionListZipLength.ToString(),bytes.Length.ToString());
                    OnDownloadFailure(this,new DownloadFailureEventArgs(e.SerialId,e.DownloadPath,e.DownloadUrl,errorMessage,e.UserData));
                    return;
                }
                int hashCode=Utility.Converter.GetInt32(Utility.Verifier.GetCrc32(bytes));
                if(_VersionListZipHashCode!=hashCode){
                    string errorMessage=Utility.Text.Format("Latest version list zip hashCode length error , need length {0} but download length {1} ",_VersionListZipHashCode.ToString(),hashCode.ToString());
                    OnDownloadFailure(this,new DownloadFailureEventArgs(e.SerialId,e.DownloadPath,e.DownloadUrl,errorMessage,e.UserData));
                    return;
                }
                try{
                    bytes=Utility.Zip.Decompress(bytes);
                }catch(Exception exception){
                    string errorMessage=Utility.Text.Format("Unable to decompress latest version list of path :{0}, the error : {1} ",e.DownloadPath, exception.Message);
                    OnDownloadFailure(this,new DownloadFailureEventArgs(e.SerialId,e.DownloadPath,e.DownloadUrl,errorMessage,e.UserData));
                    return;
                }
                if(bytes==null){
                    string errorMessage=Utility.Text.Format("Unable to decompress latest version list {0} ",e.DownloadPath);
                    OnDownloadFailure(this,new DownloadFailureEventArgs(e.SerialId,e.DownloadPath,e.DownloadUrl,errorMessage,e.UserData));
                    return;
                }
                if(_VersionListLength!=bytes.Length){
                    string errorMessage = Utility.Text.Format("Latest version list length error, need '{0}', downloaded '{1}'.", _VersionListLength.ToString(), bytes.Length.ToString());
                    OnDownloadFailure(this,new DownloadFailureEventArgs(e.SerialId,e.DownloadPath,e.DownloadUrl,errorMessage,e.UserData));
                    return;
                }
                File.WriteAllBytes(e.DownloadPath,bytes);
                if(VersionListUpdateSuccess!=null){
                    VersionListUpdateSuccess(e.DownloadPath,e.DownloadUrl);
                }
            }
        }
    }
}
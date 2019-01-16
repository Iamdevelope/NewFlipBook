using System;
using PJW.Download;
using PJW.ObjectPool;
using System.Collections.Generic;

namespace PJW.Resources
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    internal sealed partial class ResourcesManager : FrameworkModule, IResourcesManager
    {
        private static readonly char[] PackageListHeader = new char[] { 'E', 'L', 'P' };
        private static readonly char[] VersionListHeader = new char[] { 'E', 'L', 'V' };
        private static readonly char[] ReadOnlyListHeader = new char[] { 'E', 'L', 'R' };
        private static readonly char[] ReadWriteListHeader = new char[] { 'E', 'L', 'W' };
        private const string VersionListFileName = "version";
        private const string ResourceListFileName = "list";
        private const string BackupFileSuffixName = ".bak";
        private const byte ReadWriteListVersionHeader = 0;

        private readonly Dictionary<string, AssetInfo> _AssetInfos;
        private readonly Dictionary<string, AssetDependencyInfo> _AssetDependencyInfos;
        private readonly Dictionary<ResourcesName, ResourcesInfo> _ResourcesInfos;
        private readonly Dictionary<string, ResourcesGroup> _ResourcesGroups;
        private readonly SortedDictionary<ResourcesName, ReadWriteResourcesInfo> _ReadWriteResourcesInfos;
        private ResourcesIniter _ResourcesIniter;
        private VersionListProcessor _VersionListProcessor;
        private ResourcesChecker _ResourcesChecker;
        private ResourcesUpdater _ResourcesUpdater;
        private ResourcesLoader _ResourcesLoader;
        private IResourcesHelper _ResourcesHelper;
        private string _ReadOnlyPath;
        private string _ReadWritePath;
        private ResourcesMode _ResourcesMode;
        private bool _RefuseSetCurrentVariant;
        private string _CurrentVariant;
        private string _UpdatePrefixUrl;
        private string _ApplicableGameVersion;
        private int _InternalResourcesVersion;
        private DecryptResourcesCallback _DecryptResourcesCallback;
        private InitResourcesCompleteCallback _InitResourcesCompleteCallback;
        private UpdateVersionListCallbacks _UpdateVersionListCallbacks;
        private CheckResourcesCompleteCallback _CheckResourcesCompleteCallback;
        private UpdateResourcesCompleteCallback _UpdateResourcesCompleteCallback;
        private EventHandler<ResourcesUpdateStartEventArgs> _ResourcesUpdateStartEventHandler;
        private EventHandler<ResourcesUpdateSuccessEventArgs> _ResourcesUpdateSuccessEventHandler;
        private EventHandler<ResourcesUpdateChangeEventArgs> _ResourcesUpdateChangeEventHandler;
        private EventHandler<ResourcesUpdateFailureEventArgs> _ResourcesUpdateFailureEventHandler;

        public ResourcesManager()
        {
            ResourcesNameComparer resourcesNameComparer=new ResourcesNameComparer();
            _AssetInfos=new Dictionary<string, AssetInfo>();
            _AssetDependencyInfos=new Dictionary<string, AssetDependencyInfo>();
            _ResourcesInfos=new Dictionary<ResourcesName, ResourcesInfo>();
            _ResourcesGroups=new Dictionary<string, ResourcesGroup>();
            _ReadWriteResourcesInfos=new SortedDictionary<ResourcesName, ReadWriteResourcesInfo>();

            _ResourcesIniter=null;
            _VersionListProcessor=null;
            _ResourcesChecker=null;
            _ResourcesUpdater=null;
            _ResourcesLoader=null;
            _ResourcesHelper=null;

            _ReadOnlyPath=null;
            _ReadWritePath=null;
            _ResourcesMode=ResourcesMode.Unspecified;
            _RefuseSetCurrentVariant=false;
            _CurrentVariant=null;
            _UpdatePrefixUrl=null;
            _ApplicableGameVersion=null;
            _InternalResourcesVersion=0;

            _DecryptResourcesCallback=null;
            _InitResourcesCompleteCallback=null;
            _UpdateVersionListCallbacks=null;
            _CheckResourcesCompleteCallback=null;
            _UpdateResourcesCompleteCallback=null;

            _ResourcesUpdateStartEventHandler=null;
            _ResourcesUpdateSuccessEventHandler=null;
            _ResourcesUpdateChangeEventHandler=null;
            _ResourcesUpdateFailureEventHandler=null;
        }

        /// <summary>
        /// 资源管理器优先级
        /// </summary>
        /// <value></value>
        public override int Priority
        {
            get
            {
                return 70;
            }
        }

        /// <summary>
        /// 获取资源只读路径
        /// </summary>
        /// <value></value>
        public string GetReadOnlyPath
        {
            get
            {
                return _ReadOnlyPath;
            }
        }

        /// <summary>
        /// 获取资源读写路径
        /// </summary>
        /// <value></value>
        public string GetReadWritePath
        {
            get
            {
                return _ReadWritePath;
            }
        }

        /// <summary>
        /// 获取资源模式
        /// </summary>
        /// <value></value>
        public ResourcesMode GetResourcesMode
        {
            get
            {
                return _ResourcesMode;
            }
        }

        /// <summary>
        /// 获取当前变体
        /// </summary>
        /// <value></value>
        public string GetCurrentVariant
        {
            get
            {
                return _CurrentVariant;
            }
        }

        /// <summary>
        /// 获取当前资源使用的游戏版本号
        /// </summary>
        /// <value></value>
        public string GetApplicationGameVersion
        {
            get
            {
                return _ApplicableGameVersion;
            }
        }

        /// <summary>
        /// 获取当前内部资源版本号
        /// </summary>
        /// <value></value>
        public int GetInternalResourcesVersion
        {
            get
            {
                return _InternalResourcesVersion;
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量
        /// </summary>
        /// <value></value>
        public int GetAssetCount
        {
            get
            {
                return _AssetInfos.Count;
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量
        /// </summary>
        /// <value></value>
        public int GetResourcesCount
        {
            get
            {
                return _ResourcesInfos.Count;
            }
        }

        /// <summary>
        /// 获取资源组数量。
        /// </summary>
        public int GetResourcesGroupCount
        {
            get
            {
                return _ResourcesGroups.Count;
            }
        }

        /// <summary>
        /// 获取或设置资源更新下载地址。
        /// </summary>
        public string UpdatePrefixUrl
        { 
            get
            {
                return _UpdatePrefixUrl;
            } 
            set
            {
                _UpdatePrefixUrl=value;
            }
        }

        /// <summary>
        /// 获取或设置资源更新重试次数。
        /// </summary>
        public int UpdateRetryCount
        { 
            get
            {
                return _ResourcesUpdater!=null?_ResourcesUpdater.RetryCount:0;
            } 
            set
            {
                if(_ResourcesUpdater==null){
                    throw new FrameworkException(" can not use updateRetryCount at this time ");
                }
                _ResourcesUpdater.RetryCount=value;
            }
        }

        /// <summary>
        /// 获取正在更新队列大小
        /// </summary>
        /// <value></value>
        public int GetIsUpdatingCount
        {
            get
            {
                return _ResourcesUpdater.UpdatingCount;
            }
        }

        /// <summary>
        /// 获取正在等待更新队列大小
        /// </summary>
        /// <value></value>
        public int GetUpdateWatingCount
        {
            get
            {
                return _ResourcesUpdater.UpdateWaitintCount;
            }
        }

        /// <summary>
        /// 获取加载代理总个数
        /// </summary>
        /// <value></value>
        public int GetLoadAgentTotalCount
        {
            get
            {
                return _ResourcesLoader.GetLoadAgentTotalCount;
            }
        }

        /// <summary>
        /// 获取可用加载代理个数
        /// </summary>
        /// <value></value>
        public int GetFreeAgentCount
        {
            get
            {
                return _ResourcesLoader.GetFreeAgentCount;
            }
        }

        /// <summary>
        /// 获取正在使用的加载代理的个数
        /// </summary>
        /// <value></value>
        public int GetWorkingAgentCount
        {
            get
            {
                return _ResourcesLoader.GetWorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待执行的任务数量
        /// </summary>
        /// <value></value>
        public int GetLoadWaitingTaskCount
        {
            get
            {
                return _ResourcesLoader.GetLoadWaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置对象池自动释放可释放对象的间隔时间
        /// </summary>
        /// <value></value>
        public float AssetAutoReleaseInterval 
        { 
            get
            {
                return _ResourcesLoader.AssetAutoReleaseInterval;
            } 
            set
            {
                _ResourcesLoader.AssetAutoReleaseInterval=value;
            }
        }

        /// <summary>
        /// 获取或设置对象池容量
        /// </summary>
        /// <value></value>
        public int AssetCapacity
        { 
            get
            {
                return _ResourcesLoader.AssetCapacity;
            } 
            set
            {
                _ResourcesLoader.AssetCapacity=value;
            }
        }

        /// <summary>
        /// 获取或设置对象池对象过期时间
        /// </summary>
        /// <value></value>
        public float AssetExpireTime
        { 
            get
            {
                return _ResourcesLoader.AssetExpireTime;
            } 
            set
            {
                _ResourcesLoader.AssetExpireTime=value;
            }
        }

        /// <summary>
        /// 获取或设置对象池优先级
        /// </summary>
        /// <value></value>
        public int AssetPriority
        { 
            get
            {
                return _ResourcesLoader.AssetPriority;
            } 
            set
            {
                _ResourcesLoader.AssetPriority=value;
            }
        }

        /// <summary>
        /// 资源更新发生改变事件
        /// </summary>
        public event EventHandler<ResourcesUpdateChangeEventArgs> ResourcesUpdateChangeEventArgs{
            add{
                _ResourcesUpdateChangeEventHandler+=value;
            }
            remove{
                _ResourcesUpdateChangeEventHandler-=value;
            }
        }
        
        /// <summary>
        /// 资源更新失败事件
        /// </summary>
        public event EventHandler<ResourcesUpdateFailureEventArgs> ResourcesUpdateFailureEventArgs{
            add{
                _ResourcesUpdateFailureEventHandler+=value;
            }
            remove{
                _ResourcesUpdateFailureEventHandler-=value;
            }
        }
        
        /// <summary>
        /// 资源更新开始事件
        /// </summary>
        public event EventHandler<ResourcesUpdateStartEventArgs> ResourcesUpdateStartEventArgs{
            add{
                _ResourcesUpdateStartEventHandler+=value;
            }
            remove{
                _ResourcesUpdateStartEventHandler-=value;
            }
        }
        
        /// <summary>
        /// 资源更新成功事件
        /// </summary>
        public event EventHandler<ResourcesUpdateSuccessEventArgs> ResourcesUpdateSuccessEventArgs{
            add{
                _ResourcesUpdateSuccessEventHandler+=value;
            }
            remove{
                _ResourcesUpdateSuccessEventHandler-=value;
            }
        }

        public void AddResourcesAgentHelper(ILoadResourcesAgentHelper loadResourcesAgentHelper)
        {
            throw new NotImplementedException();
        }

        public void CheckResources(CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            throw new NotImplementedException();
        }

        public CheckVersionListResult CheckVersionList(int lastestInternalResourcesVersion)
        {
            throw new NotImplementedException();
        }

        public float GetResourcesGroupProgress(string resourcesGroupName)
        {
            throw new NotImplementedException();
        }

        public bool GetResourcesGroupReady(string resourcesGroupName)
        {
            throw new NotImplementedException();
        }

        public int GetResourcesGroupReadyResourceCount(string resourcesGroupName)
        {
            throw new NotImplementedException();
        }

        public int GetResourcesGroupResourceCount(string resourcesGroupName)
        {
            throw new NotImplementedException();
        }

        public int GetResourcesGroupTotalCount(string resourcesGroupName)
        {
            throw new NotImplementedException();
        }

        public int GetResourcesGroupTotalReadyCount(string resourcesGroupName)
        {
            throw new NotImplementedException();
        }

        public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
        {
            throw new NotImplementedException();
        }

        public bool IsExistAsset(string assetName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="priority">资源优先级</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, object userData, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="priority">资源优先级</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, Type assetType, object userData, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="priority">资源优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, int priority, object userData, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="priority">资源优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数</param>
        public void LoadAsset(string assetName, Type assetType, int priority, object userData, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="priority">场景优先级</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadSceneCallbacks">加载场景回调</param>
        public void LoadScene(string sceneName, int priority, object userData, LoadSceneCallbacks loadSceneCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="userData">用户自定义数据</param>
        /// <param name="loadSceneCallbacks">加载场景回调</param>
        public void LoadScene(string sceneName, object userData, LoadSceneCallbacks loadSceneCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="priority">场景优先级</param>
        /// <param name="loadSceneCallbacks">加载场景回调</param>
        public void LoadScene(string sceneName, int priority, LoadSceneCallbacks loadSceneCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadSceneCallbacks">加载场景回调</param>
        public void LoadScene(string sceneName, LoadSceneCallbacks loadSceneCallbacks)
        {
            throw new NotImplementedException();
        }

        public void SetCurretVariant(string currentVariant)
        {
            throw new NotImplementedException();
        }

        public void SetDecryptResourcesCallback(DecryptResourcesCallback decryptResourcesCallback)
        {
            throw new NotImplementedException();
        }

        public void SetDownloadManager(IDownLoadManager downloadManager)
        {
            throw new NotImplementedException();
        }

        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            throw new NotImplementedException();
        }

        public void SetReadOnlyPath(string readOnlyPath)
        {
            throw new NotImplementedException();
        }

        public void SetReadWritePath(string readWritePath)
        {
            throw new NotImplementedException();
        }

        public void SetResourcesHelper(IResourcesHelper resourcesHelper)
        {
            throw new NotImplementedException();
        }

        public void SetResourcesMode(ResourcesMode resourcesMode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">需要卸载的资源</param>
        public void UnloadAsset(object asset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">需要卸载的场景名</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调</param>
        /// <param name="userData">用户自定义数据</param>
        public void UnloadScene(string sceneName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">需要卸载的场景名</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调</param>
        public void UnloadScene(string sceneName, UnloadSceneCallbacks unloadSceneCallbacks)
        {
            throw new NotImplementedException();
        }

        public void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback)
        {
            throw new NotImplementedException();
        }

        public void UpdateVersionList(int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode, UpdateVersionListCallbacks updateVersionListCallbacks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 关闭并清理资源管理器
        /// </summary>
        public override void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 资源管理器轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            throw new System.NotImplementedException();
        }

        private AssetInfo? GetAssetInfo(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new FrameworkException("Asset name is invalid.");
            }

            AssetInfo assetInfo = default(AssetInfo);
            if (_AssetInfos.TryGetValue(assetName, out assetInfo))
            {
                return assetInfo;
            }

            return null;
        }
    }
}
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

        /// <summary>
        /// 增加加载资源代理辅助器
        /// </summary>
        /// <param name="loadResourcesAgentHelper"></param>
        public void AddResourcesAgentHelper(ILoadResourcesAgentHelper loadResourcesAgentHelper)
        {
            _ResourcesLoader.AddLoadResourcesAgentHelper(loadResourcesAgentHelper,_ResourcesHelper,_ReadOnlyPath,_ReadWritePath,_DecryptResourcesCallback);
        }

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="checkResourcesCompleteCallback"></param>
        public void CheckResources(CheckResourcesCompleteCallback checkResourcesCompleteCallback)
        {
            if(checkResourcesCompleteCallback==null){
                throw new FrameworkException("Resource complete callback is invalid ");
            }
            if(_ResourcesMode==ResourcesMode.Unspecified){
                throw new FrameworkException("You must set resourcesMode first ");
            }
            if(_ResourcesMode!=ResourcesMode.Updatable){
                throw new FrameworkException("Not set other resourcesMode in checkResources ");
            }
            if(_ResourcesChecker==null){
                throw new FrameworkException("You must set resourcesChecker ");
            }
            _RefuseSetCurrentVariant=true;
            _CheckResourcesCompleteCallback=checkResourcesCompleteCallback;
            _ResourcesChecker.CheckResources(_CurrentVariant);
        }

        /// <summary>
        /// 检查版本资源列表
        /// </summary>
        /// <param name="lastestInternalResourcesVersion">最新的内部资源版本号</param>
        /// <returns>检查版本资源列表结果</returns>
        public CheckVersionListResult CheckVersionList(int lastestInternalResourcesVersion)
        {
            return _VersionListProcessor.CheckVersionList(lastestInternalResourcesVersion);
        }

        /// <summary>
        /// 获取资源组准备进度
        /// </summary>
        /// <param name="resourcesGroupName">资源组名称</param>
        /// <returns>准备进度</returns>
        public float GetResourcesGroupProgress(string resourcesGroupName)
        {
            return GetResourcesGroup(resourcesGroupName).Progress;
        }

        /// <summary>
        /// 获取资源组是否准备好
        /// </summary>
        /// <param name="resourcesGroupName">资源组名称</param>
        /// <returns>资源组是否准备好</returns>
        public bool GetResourcesGroupReady(string resourcesGroupName)
        {
            return GetResourcesGroup(resourcesGroupName).GetReady;
        }

        /// <summary>
        /// 获取资源组已准备完成的资源数量
        /// </summary>
        /// <param name="resourcesGroupName">资源组名称</param>
        /// <returns>资源组已准备完成的资源数量</returns>
        public int GetResourcesGroupReadyResourceCount(string resourcesGroupName)
        {
            return GetResourcesGroup(resourcesGroupName).GetReadyResourcesCount;
        }

        /// <summary>
        /// 获取资源组资源数量
        /// </summary>
        /// <param name="resourcesGroupName">资源组名称</param>
        /// <returns>资源组资源数量</returns>
        public int GetResourcesGroupResourceCount(string resourcesGroupName)
        {
            return GetResourcesGroup(resourcesGroupName).GetResourcesCount;
        }

        /// <summary>
        /// 获取资源组总大小
        /// </summary>
        /// <param name="resourcesGroupName">资源组名称</param>
        /// <returns>资源组总大小</returns>
        public int GetResourcesGroupTotalCount(string resourcesGroupName)
        {
            return GetResourcesGroup(resourcesGroupName).GetTotalLength;
        }

        /// <summary>
        /// 获取资源组已准备大小
        /// </summary>
        /// <param name="resourcesGroupName"></param>
        /// <returns></returns>
        public int GetResourcesGroupTotalReadyCount(string resourcesGroupName)
        {
            return GetResourcesGroup(resourcesGroupName).GetTotalReadyLength;
        }

        public void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback)
        {
            if(initResourcesCompleteCallback==null){
                throw new FrameworkException("Init resources complete callback is invalid ");
            }

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

        /// <summary>
        /// 设置当前变体
        /// </summary>
        /// <param name="currentVariant"></param>
        public void SetCurretVariant(string currentVariant)
        {
            if(_RefuseSetCurrentVariant){
                throw new FrameworkException("You can not set current variant at this time");
            }
            _CurrentVariant=currentVariant;
        }

        /// <summary>
        /// 设置解密资源回调
        /// </summary>
        /// <param name="decryptResourcesCallback"></param>
        public void SetDecryptResourcesCallback(DecryptResourcesCallback decryptResourcesCallback)
        {
            if(_ResourcesLoader.GetLoadAgentTotalCount>0){
                throw new FrameworkException("You must set decrypt resources callback before add load agent ");
            }
            _DecryptResourcesCallback=decryptResourcesCallback;
        }

        public void SetDownloadManager(IDownLoadManager downloadManager)
        {
            throw new NotImplementedException();
        }

        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置资源只读路径
        /// </summary>
        /// <param name="readOnlyPath"></param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if(string.IsNullOrEmpty(readOnlyPath)){
                throw new FrameworkException("Read only path is invalid ");
            }
            if(_ResourcesLoader.GetLoadAgentTotalCount>0){
                throw new FrameworkException("You must set readonly path before add load resource agent helper ");
            }
            _ReadOnlyPath=readOnlyPath;
        }

        /// <summary>
        /// 设置资源可读写路径
        /// </summary>
        /// <param name="readWritePath"></param>
        public void SetReadWritePath(string readWritePath)
        {
            if(string.IsNullOrEmpty(readWritePath)){
                throw new FrameworkException("Read write path is invalid ");
            }
            if(_ResourcesLoader.GetLoadAgentTotalCount>0){
                throw new FrameworkException("You must set read-write path before add load resource agent helper ");
            }
            _ReadWritePath=readWritePath;
        }

        /// <summary>
        /// 设置资源辅助器
        /// </summary>
        /// <param name="resourcesHelper"></param>
        public void SetResourcesHelper(IResourcesHelper resourcesHelper)
        {
            if(resourcesHelper==null){
                throw new FrameworkException("Resource helper is invalid ");
            }
            if(_ResourcesLoader.GetLoadAgentTotalCount>0){
                throw new FrameworkException("You must set resource helper before add load resource agent helper ");
            }
            _ResourcesHelper=resourcesHelper;
        }

        /// <summary>
        /// 设置资源模式
        /// </summary>
        /// <param name="resourcesMode"></param>
        public void SetResourcesMode(ResourcesMode resourcesMode)
        {
            if(resourcesMode==ResourcesMode.Unspecified){
                throw new FrameworkException("Resources mode is invalid ");
            }
            if(_ResourcesMode==ResourcesMode.Unspecified){
                _ResourcesMode=resourcesMode;
                if(_ResourcesMode==ResourcesMode.Package){
                    _ResourcesIniter=new ResourcesIniter(this);
                    _ResourcesIniter.ResourceInitComplete+=OnResourceInitComplete;
                }
                else if(_ResourcesMode==ResourcesMode.Updatable){
                    _VersionListProcessor=new VersionListProcessor(this);
                    _VersionListProcessor.VersionListUpdateFailure+=OnVersionListUpdateFailure;
                    _VersionListProcessor.VersionListUpdateSuccess+=OnVersionListUpdateSuccess;
                    
                    _ResourcesChecker=new ResourcesChecker(this);
                    _ResourcesChecker.ResourcesNeedUpdate+=OnResourcesNeedUpdate;
                    _ResourcesChecker.ResourcesCheckComplete+=OnResourcesCheckComplete;

                    _ResourcesUpdater=new ResourcesUpdater(this);
                    _ResourcesUpdater.ResourcesUpdateAllComplete+=OnResourcesUpdateAllComplete;
                    _ResourcesUpdater.ResourcesUpdateChanged+=OnResourcesUpdateChanged;
                    _ResourcesUpdater.ResourcesUpdateFailure+=OnResourcesUpdateFailure;
                    _ResourcesUpdater.ResourcesUpdateStart+=OnResourcesUpdateStart;
                    _ResourcesUpdater.ResourcesUpdateSuccess+=OnResourcesUpdateSuccess;
                }
            }
            else if(_ResourcesMode!=resourcesMode){
                throw new FrameworkException("You can not change resource mode at this time ");
            }
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
            if(updateResourcesCompleteCallback==null){
                throw new FrameworkException("Resource complete callback is invalid ");
            }
            if(_ResourcesMode==ResourcesMode.Unspecified){
                throw new FrameworkException("You must set resourcesMode first ");
            }
            if(_ResourcesMode!=ResourcesMode.Updatable){
                throw new FrameworkException("Not set other resourcesMode in checkResources ");
            }
            if(_ResourcesUpdater==null){
                throw new FrameworkException("You must set ResourcesUpdater ");
            }
            _UpdateResourcesCompleteCallback=updateResourcesCompleteCallback;
            _ResourcesUpdater.UpdateResources();
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
            if(_ResourcesIniter!=null){
                _ResourcesIniter.Shutdown();
                _ResourcesIniter=null;
            }
            if(_VersionListProcessor!=null){
                _VersionListProcessor.VersionListUpdateFailure-=OnVersionListUpdateFailure;
                _VersionListProcessor.VersionListUpdateSuccess-=OnVersionListUpdateSuccess;
                _VersionListProcessor.Shutdown();
                _VersionListProcessor=null;
            }
            if(_ResourcesChecker!=null){
                _ResourcesChecker.ResourcesCheckComplete-=OnResourcesCheckComplete;
                _ResourcesChecker.ResourcesNeedUpdate-=OnResourcesNeedUpdate;
                _ResourcesChecker.Shutdown();
                _ResourcesChecker=null;
            }
            if(_ResourcesUpdater!=null){
                _ResourcesUpdater.ResourcesUpdateSuccess-=OnResourcesUpdateSuccess;
                _ResourcesUpdater.ResourcesUpdateStart-=OnResourcesUpdateStart;
                _ResourcesUpdater.ResourcesUpdateFailure-=OnResourcesUpdateFailure;
                _ResourcesUpdater.ResourcesUpdateChanged-=OnResourcesUpdateChanged;
                _ResourcesUpdater.ResourcesUpdateAllComplete-=OnResourcesUpdateAllComplete;
                _ResourcesUpdater.Shutdown();
            }
            if(_ResourcesLoader!=null){
                _ResourcesLoader.Shutdown();
                _ResourcesLoader=null;
            }
            _AssetInfos.Clear();
            _AssetDependencyInfos.Clear();
            _ResourcesInfos.Clear();
            _ResourcesGroups.Clear();
            _ReadWriteResourcesInfos.Clear();
        }


        /// <summary>
        /// 资源管理器轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if(_ResourcesUpdater!=null){
                _ResourcesUpdater.Update(elapseSeconds,realElapseSeconds);
            }
            _ResourcesLoader.Update(elapseSeconds,realElapseSeconds);
        }

        private AssetInfo? GetAssetInfo(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new FrameworkException("Asset name is invalid.");
            }
            if(!_AssetInfos.ContainsKey(assetName)){
                throw new FrameworkException("The asset info is invalid ");
            }
            return _AssetInfos[assetName];
        }
        private ResourcesInfo? GetResourcesInfo(ResourcesName resourcesName)
        {
            if(resourcesName==null){
                throw new FrameworkException("ResourcesName is invalid ");
            }
            if(!_ResourcesInfos.ContainsKey(resourcesName)){
                throw new FrameworkException("The resourcesInfo is invalid ");
            }
            return _ResourcesInfos[resourcesName];
        }
        private AssetDependencyInfo? GetAssetDependencyInfo(string assetName){
            if (string.IsNullOrEmpty(assetName))
            {
                throw new FrameworkException("Asset name is invalid.");
            }
            if(!_AssetDependencyInfos.ContainsKey(assetName)){
                throw new FrameworkException("The asset dependency info is invalid ");
            }
            return _AssetDependencyInfos[assetName];
        }
        private ResourcesGroup GetResourcesGroup(string resourcesGroupName)
        {
            if(resourcesGroupName==null){
                throw new FrameworkException(" Resources group name is invalid ");
            }
            if(!_ResourcesGroups.ContainsKey(resourcesGroupName)){
                throw new FrameworkException("The resources group not exist ");
            }
            return _ResourcesGroups[resourcesGroupName];
        }
        
        /// <summary>
        /// 资源初始化完成事件
        /// </summary>
        private void OnResourceInitComplete()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新资源成功事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        private void OnResourcesUpdateSuccess(ResourcesName t1, string t2, string t3, int t4, int t5)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新资源开始事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="t6"></param>
        private void OnResourcesUpdateStart(ResourcesName t1, string t2, string t3, int t4, int t5, int t6)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新资源失败事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        private void OnResourcesUpdateFailure(ResourcesName t1, string t2, int t3, int t4, string t5)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 更新资源发生变化事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        private void OnResourcesUpdateChanged(ResourcesName t1, string t2, string t3, int t4, int t5)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新资源全部完成事件
        /// </summary>
        private void OnResourcesUpdateAllComplete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检查资源完成事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        private void OnResourcesCheckComplete(int t1, int t2, int t3, int t4)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 资源需要更新事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="t6"></param>
        private void OnResourcesNeedUpdate(ResourcesName t1, LoadType t2, int t3, int t4, int t5, int t6)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 版本列表更新成功事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        private void OnVersionListUpdateSuccess(string t1, string t2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 版本列表更新失败事件
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        private void OnVersionListUpdateFailure(string t1, string t2)
        {
            throw new NotImplementedException();
        }
    }
}
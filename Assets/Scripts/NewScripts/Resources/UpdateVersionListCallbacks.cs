namespace PJW.Resources
{
    /// <summary>
    /// 版本资源列表更新成功回调
    /// </summary>
    /// <param name="savePath">资源列表更新后保存路径</param>
    /// <param name="downloadUrl">版本资源列表下载地址</param>
    public delegate void UpdateVersionListSuccessCallback(string savePath,string downloadUrl);

    /// <summary>
    /// 版本资源列表更新失败回调函数。
    /// </summary>
    /// <param name="savePath">版本资源列表更新地址。</param>
    /// <param name="errorMessage">错误信息。</param>
    public delegate void UpdateVersionListFailureCallback(string savePath, string errorMessage);

    /// <summary>
    /// 版本资源列表更新回调函数集
    /// </summary>
    public class UpdateVersionListCallbacks
    {
        private readonly UpdateVersionListFailureCallback _UpdateVersionListFailureCallback;
        private readonly UpdateVersionListSuccessCallback _UpdateVersionListSuccessCallback;

        /// <summary>
        /// 版本资源列表更新回调函数集实例
        /// </summary>
        /// <param name="updateVersionListFailureCallback">版本资源列表更新失败回调函数。</param>
        /// <param name="updateVersionListSuccessCallback">版本资源列表更新成功回调</param>
        public UpdateVersionListCallbacks(UpdateVersionListFailureCallback updateVersionListFailureCallback,UpdateVersionListSuccessCallback updateVersionListSuccessCallback){
            if(updateVersionListSuccessCallback==null){
                throw new FrameworkException(" Update Version list callback is invalid ");
            }
            _UpdateVersionListFailureCallback=updateVersionListFailureCallback;
            _UpdateVersionListSuccessCallback=updateVersionListSuccessCallback;
        }

        /// <summary>
        /// 版本资源列表更新回调函数集实例
        /// </summary>
        /// <param name="updateVersionListSuccessCallback">版本资源列表更新成功回调</param>
        public UpdateVersionListCallbacks(UpdateVersionListSuccessCallback updateVersionListSuccessCallback)
        :this(null,updateVersionListSuccessCallback){

        }
        public UpdateVersionListSuccessCallback GetUpdateVersionListSuccessCallback{
            get{return _UpdateVersionListSuccessCallback;}
        }
        public UpdateVersionListFailureCallback GetUpdateVersionListFailureCallback{
            get{return _UpdateVersionListFailureCallback;}
        }
    }
}
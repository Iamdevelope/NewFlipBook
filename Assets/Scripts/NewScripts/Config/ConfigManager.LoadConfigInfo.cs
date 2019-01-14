namespace PJW.Config
{
    internal partial class ConfigManager{
        /// <summary>
        /// 加载配置文件信息
        /// </summary>
        private sealed class LoadConfigInfo
        {
            private readonly LoadType _LoadType;
            private readonly object _UserData;

            public LoadConfigInfo(LoadType loadType,object userData){
                _LoadType=loadType;
                _UserData=userData;
            }
            public LoadType GetLoadType{
                get{return _LoadType;}
            }
            public object GetUserData{
                get{return _UserData;}
            }
        }
    }
}
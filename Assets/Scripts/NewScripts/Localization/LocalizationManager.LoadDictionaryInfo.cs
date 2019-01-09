
namespace PJW.Localization
{
    internal partial class LocalizationManager
    {

        private sealed class LoadDictionaryInfo
        {
            private readonly LoadType _LoadType;
            private readonly object _UserData;

            public LoadDictionaryInfo(LoadType loadType,object userData)
            {
                _LoadType = loadType;
                _UserData = userData;
            }
            public LoadType GetLoadType
            {
                get { return _LoadType; }
            }
            public object GetUserData
            {
                get { return _UserData; }
            }
        }
    }
}
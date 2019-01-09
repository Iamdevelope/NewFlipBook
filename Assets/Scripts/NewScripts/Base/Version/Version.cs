
namespace PJW.Version
{

    public partial class Version
    {
        private const string GameFrameworkVersion = "0.0.1";
        private static IVersionHelper _VersionHelper = null;

        public static string GetGameFrameworkVersion
        {
            get { return GameFrameworkVersion; }
        }
        public static string GetGameVersion
        {
            get
            {
                if (_VersionHelper == null)
                {
                    return string.Empty;
                }
                return _VersionHelper.GetGameVersion;
            }
        }
        public static int GetInternalGameVersion
        {
            get
            {
                if (_VersionHelper == null)
                {
                    return 0;
                }
                return _VersionHelper.GetInternalGameVersion;
            }
        }
        public static void SetVersionHelper(IVersionHelper versionHelper)
        {
            _VersionHelper = versionHelper;
        }
    }
}
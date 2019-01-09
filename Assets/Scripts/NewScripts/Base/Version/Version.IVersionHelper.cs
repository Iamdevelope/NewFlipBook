namespace PJW.Version
{
    public partial class Version
    {
        /// <summary>
        /// 游戏版本号管理器
        /// </summary>
        public interface IVersionHelper
        {
            /// <summary>
            /// 游戏版本号
            /// </summary>
            string GetGameVersion
            {
                get;
            }
            /// <summary>
            /// 内部游戏版本号
            /// </summary>
            int GetInternalGameVersion
            {
                get;
            }
        }
    }
}

namespace PJW.Resources
{
    /// <summary>
    /// 卸载场景失败委托
    /// </summary>
    /// <param name="sceneName">要卸载的场景名</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void UnloadSceneFailureCallback(string sceneName, object userData);

}
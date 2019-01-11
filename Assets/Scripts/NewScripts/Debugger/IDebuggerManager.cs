namespace PJW.Debugger
{
    /// <summary>
    /// 调试窗口管理器接口
    /// </summary>
    public interface IDebuggerManager
    {
        /// <summary>
        /// 获取或设置调试窗口激活
        /// </summary>
        /// <value></value>
         bool ActiveWindow{get;set;}

         /// <summary>
         /// 获取调试窗口根节点
         /// </summary>
         /// <value></value>
         IDebuggerWindowGroup GetDebuggerWindowRoot{get;}
         /// <summary>
         /// 注册调试窗口
         /// </summary>
         /// <param name="path">调试窗口路径</param>
         /// <param name="debuggerWindow">需要注册的调试窗口</param>
         /// <param name="args">初始化调试窗口时的参数</param>
         void RegisterDebuggerWindow(string path,IDebuggerWindow debuggerWindow,params object[] args);

         /// <summary>
         /// 获取调试窗口
         /// </summary>
         /// <param name="path">调试窗口路径</param>
         /// <returns>调试窗口</returns>
         IDebuggerWindow GetDebuggerWindow(string path);

         /// <summary>
         /// 选择调试窗口
         /// </summary>
         /// <param name="path">调试窗口路径</param>
         /// <returns>是否成功选中调试窗口</returns>
         bool SelectDebuggerWindow(string path);
    }
}
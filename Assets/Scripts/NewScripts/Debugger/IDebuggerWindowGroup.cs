namespace PJW.Debugger
{
    /// <summary>
    /// 调试窗口组接口
    /// </summary>
    public interface IDebuggerWindowGroup:IDebuggerWindow
    {
        /// <summary>
        /// 获取调试窗口数量
        /// </summary>
        /// <value></value>
         int GetDebuggerWindowCount{get;}

         /// <summary>
         /// 当前选中调试窗口索引
         /// </summary>
         /// <value></value>
         int SelectedIndex{
             get;
             set;
         }

         /// <summary>
         /// 获取当前选中调试窗口
         /// </summary>
         /// <value></value>
         IDebuggerWindow GetCurrentSelectedWindow{get;}
         
         /// <summary>
         /// 获取所有调试窗口的名称
         /// </summary>
         /// <returns></returns>
         string[] GetDebuggerWindowNames();

         /// <summary>
         /// 获取调试窗口
         /// </summary>
         /// <param name="path">调试窗口路径</param>
         /// <returns>要获取的调试窗口</returns>
         IDebuggerWindow GetDebuggerWindow(string path);

         /// <summary>
         /// 注册调试窗口
         /// </summary>
         /// <param name="path">调试窗口路径</param>
         /// <param name="debuggerWindow">要注册的调试窗口</param>
         void RegisterDebuggerWindow(string path,IDebuggerWindow debuggerWindow);
    }
}
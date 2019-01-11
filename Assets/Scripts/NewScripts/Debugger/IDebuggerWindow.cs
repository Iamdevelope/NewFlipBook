namespace PJW.Debugger
{
    /// <summary>
    /// 调试窗口接口
    /// </summary>
    public interface IDebuggerWindow
    {
        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <param name="args">初始化窗口参数</param>
        void Init(params object[] args);

        /// <summary>
        /// 调试窗口每帧执行
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
         void Update(float elapseSeconds,float realElapseSeconds);

         /// <summary>
         /// 窗口绘制
         /// </summary>
         void OnDraw();

        /// <summary>
        /// 进入调试窗口
        /// </summary>
         void OnEnter();

         /// <summary>
         /// 离开调试窗口
         /// </summary>
         void OnExit();
         /// <summary>
         /// 关闭调试窗口
         /// </summary>
        void Shutdown();
    }
}
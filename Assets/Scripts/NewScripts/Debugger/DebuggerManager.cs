namespace PJW.Debugger
{
    /// <summary>
    /// 调试窗口管理器
    /// </summary>
    internal sealed partial class DebuggerManager : FrameworkModule, IDebuggerManager
    {
        private readonly DebuggerWindowGroup _DebuggerWindowGroupRoot;
        private bool _ActiveWindow;
        
        public DebuggerManager()
        {
            _DebuggerWindowGroupRoot=new DebuggerWindowGroup();
            _ActiveWindow=false;
        }
        
        public bool ActiveWindow
        {
            get
            {
                return _ActiveWindow;
            }

            set
            {
                _ActiveWindow=value;
            }
        }
        public override int Priority{
            get{return -1;}
        }
        public IDebuggerWindowGroup GetDebuggerWindowRoot{
            get{return _DebuggerWindowGroupRoot;}
        }

        /// <summary>
        /// 获取调试窗口
        /// </summary>
        /// <param name="path">调试窗口路径</param>
        /// <returns>要获取的调试窗口</returns>
        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            return _DebuggerWindowGroupRoot.GetDebuggerWindow(path);
        }

        /// <summary>
        /// 注册调试窗口
        /// </summary>
        /// <param name="path">调试窗口路径</param>
        /// <param name="debuggerWindow">需要注册的调试窗口</param>
        /// <param name="args">参数</param>
        public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            if(string.IsNullOrEmpty(path)){
                throw new FrameworkException(" The path is invalid ");
            }
            if(debuggerWindow==null){
                throw new FrameworkException(" Debugger window is invalid ");
            }
            _DebuggerWindowGroupRoot.RegisterDebuggerWindow(path,debuggerWindow);
            debuggerWindow.Init(args);
        }

        /// <summary>
        /// 选中调试窗口
        /// </summary>
        /// <param name="path">调试窗口路径</param>
        /// <returns>是否选中调试窗口</returns>
        public bool SelectDebuggerWindow(string path)
        {
            return _DebuggerWindowGroupRoot.SelectedDebuggerWindow(path);
        }

        
        public override void Update(float elapseSeconds, float realElapseSeconds){
            if(!_ActiveWindow){
                return;
            }
            _DebuggerWindowGroupRoot.Update(elapseSeconds,realElapseSeconds);
        }
        public override void Shutdown(){
            _ActiveWindow=false;
            _DebuggerWindowGroupRoot.Shutdown();
        }
    }
}
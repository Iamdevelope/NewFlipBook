namespace PJW.Debugger
{
    /// <summary>
    /// 调试窗口管理器
    /// </summary>
    internal sealed partial class DebuggerManager : FrameworkModule, IDebuggerManager
    {
        private readonly DebuggerWindowGroup _DebuggerWindowGroupRoot;
        public bool ActiveWindow
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public IDebuggerWindowGroup GetDebuggerWindowRoot{
            get{return _DebuggerWindowGroupRoot;}
        }

        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public bool SelectDebuggerWindow(string path)
        {
            throw new System.NotImplementedException();
        }

        public override int Priority{
            get{return -1;}
        }
        public override void Update(float elapseSeconds, float realElapseSeconds){

        }
        public override void Shutdown(){

        }
    }
}
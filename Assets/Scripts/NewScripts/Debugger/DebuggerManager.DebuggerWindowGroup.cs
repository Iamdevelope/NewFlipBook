using System.Collections.Generic;

namespace PJW.Debugger
{
    internal sealed partial class DebuggerManager{
        /// <summary>
        /// 调试窗口组
        /// </summary>
        public partial class DebuggerWindowGroup : IDebuggerWindowGroup
        {
            private readonly List<KeyValuePair<string,IDebuggerWindow>> _DebuggerWindows;
            private int _SelectedIndex;
            private string[] _DebuggerWindowNames;

            public DebuggerWindowGroup(){
                _DebuggerWindows=new List<KeyValuePair<string, IDebuggerWindow>>();
                _SelectedIndex=0;
                _DebuggerWindowNames=null;
            }
            public int GetDebuggerWindowCount
            {
                get
                {
                    return _DebuggerWindows.Count;
                }
            }

            public int SelectedIndex { 
                get {
                    return _SelectedIndex;
                }
                set{
                    _SelectedIndex=value;
                }
            }

            /// <summary>
            /// 获取当前选中调试窗口
            /// </summary>
            /// <value></value>
            public IDebuggerWindow GetCurrentSelectedWindow {
                get{
                    if(_SelectedIndex>=_DebuggerWindows.Count){
                        return null;
                    }
                    return _DebuggerWindows[_SelectedIndex].Value;
                }
            }

            /// <summary>
            /// 获取所有调试窗口的名称
            /// </summary>
            /// <returns></returns>
            public string[] GetDebuggerWindowNames()
            {
                return _DebuggerWindowNames;
            }

            /// <summary>
            /// 窗口绘制
            /// </summary>
            public void OnDraw()
            {
                GetCurrentSelectedWindow.OnDraw();
            }

            /// <summary>
            /// 进入调试窗口
            /// </summary>
            public void OnEnter()
            {
                GetCurrentSelectedWindow.OnEnter();
            }

            /// <summary>
            /// 离开调试窗口
            /// </summary>
            public void OnExit()
            {
                GetCurrentSelectedWindow.OnExit();
            }

            /// <summary>
            /// 初始化调试窗口
            /// </summary>
            /// <param name="args"></param>
            public void Init(params object[] args)
            {
                GetCurrentSelectedWindow.Init(args);
            }

            /// <summary>
            /// 获取调试窗口
            /// </summary>
            /// <param name="path">调试窗口路径</param>
            /// <returns>要获取的调试窗口</returns>
            public IDebuggerWindow GetDebuggerWindow(string path)
            {
                if(string.IsNullOrEmpty(path)){
                    return null;
                }
                int pos=path.IndexOf('/');
                if(pos<0||pos>=path.Length-1){
                    return InternalGetDebuggerWindow(path);
                }
                string debuggerWindowGroupName=path.Substring(0,pos);
                string leftPath=path.Substring(pos+1);
                DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(path);
                if(debuggerWindowGroup==null){
                    return null;
                }
                return debuggerWindowGroup.GetDebuggerWindow(leftPath);
            }

            /// <summary>
            /// 选中调试窗口
            /// </summary>
            /// <param name="path">调试窗口路径</param>
            /// <returns>是否选中</returns>
            public bool SelectedDebuggerWindow(string path)
            {
                if(string.IsNullOrEmpty(path)){
                    return false;
                }
                int pos=path.IndexOf('/');
                if(pos<0||pos>=path.Length-1){
                    return InternalSelectedDebuggerWindow(path);
                }
                string debuggerWindowGroupName=path.Substring(0,pos);
                string leftPath=path.Substring(pos+1);
                DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(path);
                if(debuggerWindowGroup==null){
                    return false;
                }
                return debuggerWindowGroup.SelectedDebuggerWindow(leftPath);
            }
            /// <summary>
            /// 注册调试窗口。
            /// </summary>
            /// <param name="path">调试窗口路径。</param>
            /// <param name="debuggerWindow">要注册的调试窗口。</param>
            public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
            {
                if(string.IsNullOrEmpty(path)){
                    throw new FrameworkException(" path is invalid ");
                }
                int pos=path.IndexOf('/');
                if(pos<0||pos>=path.Length-1){
                    if(InternalGetDebuggerWindow(path)!=null){
                        throw new FrameworkException(" debugger window has been register ");
                    }
                    _DebuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(path,debuggerWindow));
                    RefreshDebuggerWindowNames();
                }
                else{
                    string debuggerWindowGroupName=path.Substring(0,pos);
                    string leftPath=path.Substring(pos+1);
                    DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(path);
                    if(debuggerWindowGroup==null){
                        if(InternalGetDebuggerWindow(debuggerWindowGroupName)!=null){
                            throw new FrameworkException(" debugger window has been register ");
                        }
                        debuggerWindowGroup=new DebuggerWindowGroup();
                        _DebuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(debuggerWindowGroupName,debuggerWindow));
                        RefreshDebuggerWindowNames();
                    }
                    debuggerWindowGroup.RegisterDebuggerWindow(leftPath,debuggerWindow);
                }
            }

            /// <summary>
            /// 关闭调试窗口组
            /// </summary>
            public void Shutdown()
            {
                foreach(KeyValuePair<string,IDebuggerWindow> item in _DebuggerWindows){
                    item.Value.Shutdown();
                }
                _DebuggerWindows.Clear();
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                GetCurrentSelectedWindow.Update(elapseSeconds,realElapseSeconds);
            }

            /// <summary>
            /// 获取调试窗口
            /// </summary>
            /// <param name="path">调试窗口路径。</param>
            /// <returns>调试窗口</returns>
            private IDebuggerWindow InternalGetDebuggerWindow(string path){
                foreach (KeyValuePair<string,IDebuggerWindow> item in _DebuggerWindows)
                {
                    if(item.Key==path){
                        return item.Value;
                    }
                }
                return null;
            }

            /// <summary>
            /// 选中调试窗口
            /// </summary>
            /// <param name="path">调试窗口路径</param>
            /// <returns>是否选中</returns>
            private bool InternalSelectedDebuggerWindow(string path){
                int index=0;
                foreach (KeyValuePair<string,IDebuggerWindow> item in _DebuggerWindows)
                {
                    index++;
                    if(item.Key==path){
                        _SelectedIndex=index;
                        return true;
                    }
                }
                return false;
            }

            private void RefreshDebuggerWindowNames(){
                _DebuggerWindowNames=new string[_DebuggerWindows.Count];
                int index=0;
                foreach (KeyValuePair<string,IDebuggerWindow> item in _DebuggerWindows)
                {
                    _DebuggerWindowNames[index++]=item.Key;
                }
            }
        }
    }
}
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
            /// 初始化调试窗口
            /// </summary>
            /// <param name="args"></param>
            public void Init(params object[] args)
            {
                
            }

            /// <summary>
            /// 获取调试窗口
            /// </summary>
            /// <param name="path">调试窗口路径</param>
            /// <returns>要获取的调试窗口</returns>
            public IDebuggerWindow GetDebuggerWindow(string path)
            {
                throw new System.NotImplementedException();
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
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 进入调试窗口
            /// </summary>
            public void OnEnter()
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 离开调试窗口
            /// </summary>
            public void OnExit()
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 注册调试窗口。
            /// </summary>
            /// <param name="path">调试窗口路径。</param>
            /// <param name="debuggerWindow">要注册的调试窗口。</param>
            public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
            {
                throw new System.NotImplementedException();
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
                
            }
        }
    }
}
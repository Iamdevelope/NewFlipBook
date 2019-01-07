
namespace PJW.FSM
{
    /// <summary>
    /// 有限状态机事件类
    /// </summary>
    /// <typeparam name="T">有限状态机类型</typeparam>
    /// <param name="fsm">有限状态机</param>
    /// <param name="sender">发送者</param>
    /// <param name="userData">用户自定义数据</param>
    public delegate void FsmEventHandler<T>(IFsm<T> fsm,object sender,object userData) where T : class;
}
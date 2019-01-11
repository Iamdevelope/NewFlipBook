using System;
using System.Collections.Generic;

namespace PJW
{
    /// <summary>
    /// 事件池基类
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    internal sealed partial class EventPool<T> where T : BaseEventAvgs
    {
        private readonly Dictionary<int, LinkedList<EventHandler<T>>> _EventHandlers;
        private readonly Queue<Event> _Events;
        private readonly EventPoolMode _EventPoolMode;
        private EventHandler<T> _DefaultEventHandler;

        /// <summary>
        /// 事件池构造函数
        /// </summary>
        /// <param name="eventPoolMode"></param>
        public EventPool(EventPoolMode eventPoolMode)
        {
            _EventHandlers = new Dictionary<int, LinkedList<EventHandler<T>>>();
            _Events = new Queue<Event>();
            _EventPoolMode = eventPoolMode;
            _DefaultEventHandler = null;
        }

        public int GetEventHandlerCount
        {
            get { return _EventHandlers.Count; }
        }

        public int GetEventCount
        {
            get { return _Events.Count; }
        }

        public EventPoolMode EventPoolMode
        {
            get { return _EventPoolMode; }
        }

        /// <summary>
        /// 事件池轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public void Update(float elapseSeconds,float realElapseSeconds)
        {
            lock(_Events){
                while(_Events.Count>0){
                    Event e=_Events.Dequeue();
                    FrameworkLog.Debug(e.GetSender.ToString()+" ----- "+e.GetEventAvg.Id);
                    HandlerEvent(e.GetSender,e.GetEventAvg);
                }
            }
        }
        
        /// <summary>
        /// 关闭并清理事件池
        /// </summary>
        public void ShutDown()
        {
            Clear();
            _EventHandlers.Clear();
            _DefaultEventHandler=null;
        }

        /// <summary>
        /// 清理事件
        /// </summary>
        public void Clear(){
            lock(_Events){
                _Events.Clear();
            }
        }

        /// <summary>
        /// 获取事件处理函数的数量
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <returns>事件处理函数的数量</returns>
        public int Count(int id){
            LinkedList<EventHandler<T>> handlers=null;
            if(_EventHandlers.TryGetValue(id,out handlers)){
                return handlers.Count;
            }
            return 0;
        }

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Check(int id,EventHandler<T> handler){
            if(handler==null){
                throw new FrameworkException(" the event handler is invalid ");
            }
            LinkedList<EventHandler<T>> handlers=null;
            if(!_EventHandlers.TryGetValue(id,out handlers)){
                return false;
            }
            return handlers.Contains(handler);
        }

        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <param name="handler">事件函数</param>
        public void Subscribe(int id,EventHandler<T> handler){
            if(handler==null){
                throw new FrameworkException(" the event handler is invalid ");
            }
            LinkedList<EventHandler<T>> handlers=null;
            if(!_EventHandlers.TryGetValue(id,out handlers)){
                handlers=new LinkedList<EventHandler<T>>();
                handlers.AddLast(handler);
                _EventHandlers.Add(id,handlers);
            }
            else if ((_EventPoolMode&EventPoolMode.AllowMultiHandler)==0){
                throw new FrameworkException(Utility.Text.Format(" event {0} not allow multi handler ",id.ToString()));
            }
            else if((_EventPoolMode&EventPoolMode.AllowDuplicateHandler)==0&&Check(id,handler)){
                throw new FrameworkException(Utility.Text.Format(" event {0} not allow duplicate handler ",id.ToString()));
            }
            else{
                handlers.AddLast(handler);
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <param name="handler">事件处理函数</param>
        public void Unsubscribe(int id,EventHandler<T> handler){
            if(handler==null){
                throw new FrameworkException(" the event handler is invalid ");
            }
            LinkedList<EventHandler<T>> handlers=null;
            if(!_EventHandlers.TryGetValue(id,out handlers)){
                throw new FrameworkException(Utility.Text.Format(" event {0} not exit ",id.ToString()));
            }
            if(!handlers.Remove(handler)){
                throw new FrameworkException(Utility.Text.Format(" event {0} not found allow dulicate handler ",id.ToString()));
            }
        }

        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">要设置的事件处理函数</param>
        public void SetDefaultHandler(EventHandler<T> handler){
            _DefaultEventHandler=handler;
        }

        /// <summary>
        /// 抛出事件，这个是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Fire(object sender,T e){
            Event eventNode=new Event(sender,e);
            lock(_Events){
                _Events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件立即模式，这个不是线程安全的，事件会立刻触发
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender,T e){
            HandlerEvent(sender,e);
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void HandlerEvent(object sender,T e)
        {
            int eventId=e.Id;
            bool noHandlerException=false;
            LinkedList<EventHandler<T>> handlers=null;
            if(_EventHandlers.TryGetValue(eventId,out handlers)){
                LinkedListNode<EventHandler<T>> currentHandlers=handlers.First;
                while(currentHandlers!=null){
                    LinkedListNode<EventHandler<T>> next=currentHandlers.Next;
                    currentHandlers.Value(sender,e);
                    currentHandlers=next;
                }
            }
            // 判断事件池类型是否为不允许存在事件函数
            else if((_EventPoolMode&EventPoolMode.AllowNoHandler)==0){
                noHandlerException=true;
            }
            ReferencePool.Release(e);
            if(noHandlerException){
                throw new FrameworkException(Utility.Text.Format(" event {0} not allow no handler ",eventId.ToString()));
            }
        }
    }
}

using System;

namespace PJW.Event{

	/// <summary>
	/// 事件处理
	/// </summary>
    internal sealed class EventManager : FrameworkModule, IEventManager
    {
		private readonly EventPool<GameEventAvgs> _EventPool;

		public EventManager()
		{
			_EventPool=new EventPool<GameEventAvgs>(EventPoolMode.AllowNoHandler|EventPoolMode.AllowMultiHandler);
		}

        public int EventHandlerCount
        {
            get
            {
                return _EventPool.GetEventHandlerCount;
            }
        }

        public int EventCount {
			get{return _EventPool.GetEventCount;}
		}

		public override int Priority{ get{return 100;}}
		/// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Check(int id, EventHandler<GameEventAvgs> handler)
        {
			return _EventPool.Check(id,handler);
        }

		/// <summary>
        /// 获取事件处理函数的数量
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <returns>事件处理函数的数量</returns>
        public int Count(int id)
        {
			return _EventPool.Count(id);
        }

		/// <summary>
        /// 抛出事件，这个是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="avgs"></param>
        public void Fire(object sender, GameEventAvgs avgs)
        {
			_EventPool.Fire(sender,avgs);
        }

		/// <summary>
        /// 抛出事件立即模式，这个不是线程安全的，事件会立刻触发
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="avgs">事件参数</param>
        public void FireNow(object sender, GameEventAvgs avgs)
        {
			_EventPool.FireNow(sender,avgs);
        }

		/// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">要设置的事件处理函数</param>
        public void SetDefaultHandler(EventHandler<GameEventAvgs> handler)
        {
			_EventPool.SetDefaultHandler(handler);
        }

		/// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <param name="handler">事件函数</param>
        public void Subscribe(int id, EventHandler<GameEventAvgs> handler)
        {
			_EventPool.Subscribe(id,handler);
        }

		/// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <param name="handler">事件处理函数</param>
        public void Unsubscribe(int id, EventHandler<GameEventAvgs> handler)
        {
			_EventPool.Unsubscribe(id,handler);
        }

		
        public override void Shutdown()
        {
			_EventPool.ShutDown();
        }
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
			_EventPool.Update(elapseSeconds,realElapseSeconds);
        }
    }
}
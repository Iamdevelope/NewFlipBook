
namespace PJW
{
    internal partial class EventPool<T>
    {
        /// <summary>
        /// 事件节点
        /// </summary>
        private sealed class Event
        {
            private readonly object _Sender;
            private readonly T _EventAvg;

            public Event(object sender,T t)
            {
                _Sender = sender;
                _EventAvg = t;
            }
            public object GetSender
            {
                get { return _Sender; }
            }
            public T GetEventAvg
            {
                get { return _EventAvg; }
            }
        }
    }
}


namespace PJW.MVC.Patterns
{
    /// <summary>
    /// 消息的数据结构
    /// </summary>
    public class Notification
    {
        public string name;
        public object data;
        public Notification(string name,object data)
        {
            this.name = name;
            this.data = data;
        }
    }
}
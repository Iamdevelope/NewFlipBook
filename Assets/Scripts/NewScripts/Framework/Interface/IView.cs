

namespace PJW.MVC.Interface
{
    /// <summary>
    /// 管理所有的IMediator
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 注册IMediator
        /// </summary>
        /// <param name="mediator"></param>
        void RegisterMediator(IMediator mediator);
        /// <summary>
        /// 移除IMediator
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IMediator RemoveMediator(string mediatorName);
        /// <summary>
        /// 获取IMediator
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IMediator GetMediator(string mediatorName);
    }
}

namespace PJW
{
    /// <summary>
    /// 封装一个没有参数，但是有一个TResult类型的返回值的方法
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <returns>返回值</returns>
    public delegate TResult FrameworkFunc<out TResult>();
    /// <summary>
    /// 封装了一个具有一个参数，一个TResult类型的返回值的方法
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="t">参数</param>
    /// <returns>返回值</returns>
    public delegate TResult FrameworkFunc<in T, out TResult>(T t);

}
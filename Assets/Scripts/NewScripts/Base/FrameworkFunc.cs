
namespace PJW
{
    /// <summary>
    /// ��װһ��û�в�����������һ��TResult���͵ķ���ֵ�ķ���
    /// </summary>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <returns>����ֵ</returns>
    public delegate TResult FrameworkFunc<out TResult>();
    /// <summary>
    /// ��װ��һ������һ��������һ��TResult���͵ķ���ֵ�ķ���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="t">����</param>
    /// <returns>����ֵ</returns>
    public delegate TResult FrameworkFunc<in T, out TResult>(T t);

}
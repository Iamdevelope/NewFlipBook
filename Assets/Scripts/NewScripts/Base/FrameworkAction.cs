namespace PJW
{
    /// <summary>
    /// 封装了一个无参无返回值的方法
    /// </summary>
    public delegate void FrameworkAction();
    /// <summary>
    /// 封装了一个方法，该方法具有一个参数一个返回值
    /// </summary>
    /// <typeparam name="T1">该方法的第一个返回值</typeparam>
    /// <param name="t1">该方法的第一个参数</param>
    public delegate void FrameworkAction<in T1>(T1 t1);
    /// <summary>
    /// 封装了一个方法，该方法具有二个参数二个返回值
    /// </summary>
    /// <typeparam name="T1">该方法的第一个返回值</typeparam>
    /// <typeparam name="T2">该方法的第二个返回值</typeparam>
    /// <param name="t1">该方法的第一个参数</param>
    /// <param name="t2">该方法的第二个参数</param>
    public delegate void FrameworkAction<in T1,in T2>(T1 t1,T2 t2);
    /// <summary>
    /// 封装了一个方法，该方法具有三个参数三个返回值
    /// </summary>
    /// <typeparam name="T1">该方法的第一个返回值</typeparam>
    /// <typeparam name="T2">该方法的第二个返回值</typeparam>
    /// <typeparam name="T3">该方法的第三个返回值</typeparam>
    /// <param name="t1">该方法的第一个参数</param>
    /// <param name="t2">该方法的第二个参数</param>
    /// <param name="t3">该方法的第三个参数</param>
    public delegate void FrameworkAction<in T1, in T2,in T3>(T1 t1, T2 t2,T3 t3);
    /// <summary>
    /// 封装了一个方法，该方法具有四个参数四个返回值
    /// </summary>
    /// <typeparam name="T1">该方法的第一个返回值</typeparam>
    /// <typeparam name="T2">该方法的第二个返回值</typeparam>
    /// <typeparam name="T3">该方法的第三个返回值</typeparam>
    /// <typeparam name="T4">该方法的第四个返回值</typeparam>
    /// <param name="t1">该方法的第一个参数</param>
    /// <param name="t2">该方法的第二个参数</param>
    /// <param name="t3">该方法的第三个参数</param>
    /// <param name="t4">该方法的第四个参数</param>
    public delegate void FrameworkAction<in T1, in T2, in T3,in T4>(T1 t1, T2 t2, T3 t3,T4 t4);

    /// <summary>
    /// 封装了一个方法，该方法具有四个参数四个返回值
    /// </summary>
    /// <typeparam name="T1">该方法的第一个返回值</typeparam>
    /// <typeparam name="T2">该方法的第二个返回值</typeparam>
    /// <typeparam name="T3">该方法的第三个返回值</typeparam>
    /// <typeparam name="T4">该方法的第四个返回值</typeparam>
    /// <typeparam name="T5">该方法的第五个返回值</typeparam>
    /// <param name="t1">该方法的第一个参数</param>
    /// <param name="t2">该方法的第二个参数</param>
    /// <param name="t3">该方法的第三个参数</param>
    /// <param name="t4">该方法的第四个参数</param>
    /// <param name="t5">该方法的第五个参数</param>
    public delegate void FrameworkAction<in T1, in T2, in T3,in T4,in T5>(T1 t1, T2 t2, T3 t3,T4 t4,T5 t5);

    /// <summary>
    /// 封装了一个方法，该方法具有四个参数四个返回值
    /// </summary>
    /// <typeparam name="T1">该方法的第一个返回值</typeparam>
    /// <typeparam name="T2">该方法的第二个返回值</typeparam>
    /// <typeparam name="T3">该方法的第三个返回值</typeparam>
    /// <typeparam name="T4">该方法的第四个返回值</typeparam>
    /// <typeparam name="T5">该方法的第五个返回值</typeparam>
    /// <typeparam name="T6">该方法的第六个返回值</typeparam>
    /// <param name="t1">该方法的第一个参数</param>
    /// <param name="t2">该方法的第二个参数</param>
    /// <param name="t3">该方法的第三个参数</param>
    /// <param name="t4">该方法的第四个参数</param>
    /// <param name="t5">该方法的第五个参数</param>
    /// <param name="t6">该方法的第六个参数</param>
    public delegate void FrameworkAction<in T1, in T2, in T3,in T4,in T5,in T6>(T1 t1, T2 t2, T3 t3,T4 t4,T5 t5,T6 t6);
}
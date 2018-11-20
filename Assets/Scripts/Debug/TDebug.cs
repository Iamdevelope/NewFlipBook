using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
///  封装可以开关的Debug类
/// </summary>
public class TDebug {
    /// <summary>
    /// 是否启用Log
    /// </summary>
    static public bool enableLog = false;
    /// <summary>
    /// 输出信息到控制台
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    static public void Log ( object message , UnityEngine.Object context = null )
    {
        if ( enableLog )  Debug.Log ( message , context );
    }
    /// <summary>
    /// 输出错误信息到控制台
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    static public void LogError ( object message , UnityEngine.Object context = null )
    {
        if ( enableLog ) Debug.LogError (  message , context );
    }
    /// <summary>
    /// 输出警告信息到控制台
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    static public void LogWarning ( object message , UnityEngine.Object context = null )
    {
        if ( enableLog ) Debug.LogWarning (  message , context );
    }

    static private string Now ( )
    {
        return System.DateTime.Now.ToString ( "yyyy-MM-dd HH:mm:ss:ffff" );
    }
}

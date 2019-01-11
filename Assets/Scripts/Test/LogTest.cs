using PJW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTest : MonoBehaviour {

    private void Start()
    {
        FrameworkLog.SetLogHelper(new Log());
    }
}
public class Log : ILogHelper
{
    void ILogHelper.Log(LogLevel logLevel, object message)
    {
        Debug.Log(Utility.Text.Format("[{0}] - {1} : {2} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), logLevel.ToString(), message));
    }
}
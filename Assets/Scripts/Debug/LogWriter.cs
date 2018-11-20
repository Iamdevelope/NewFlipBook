using UnityEngine;
using System.Collections;
using System;
using System.IO;

/// <summary>
/// Debug输出写入本地硬件方法
/// </summary>
public class LogWriter
{
    /// <summary>
    /// 初始化文件写入的入口
    /// </summary>
    /// <param name="logName"></param>
    public void InitWriter ( string logName )
    {
        GetLogPath ( logName );
        CreateLogFile ( );
    }

    /// <summary>
    /// 设置及获取对应发布平台的日记存放路径
    /// </summary>
    /// <param name="logName"></param>
    private void GetLogPath ( string logName )
    {
        switch ( Application.platform )
        {
            case RuntimePlatform.Android:
                {
                    //m_logPath = "mnt/sdcard/123.txt";
                    m_logPath = string.Format( "{0}/{1}.txt" , Application.persistentDataPath , logName );
                    //m_logPath = string.Format ( "mnt/sdcard/{0}.txt" , logName );
                }
                break;
            case RuntimePlatform.IPhonePlayer:
                m_logPath = string.Format ( "{0}/{1}.txt" , Application.persistentDataPath , logName );
                break;
            case RuntimePlatform.WindowsPlayer:
                m_logPath = string.Format ( "{0}/../{1}.txt" , Application.dataPath , logName );
                break;
            case RuntimePlatform.WindowsEditor:
                m_logPath = string.Format ( "{0}/../{1}.txt" , Application.dataPath , logName );
                break;
            case RuntimePlatform.OSXEditor:

                break;
        }
    }

    /// <summary>
    /// 根据路径创建日记文件，并注册文件写入的函数。
    /// </summary>
    private void CreateLogFile ( )
    {
        if ( !File.Exists ( m_logPath ) )
        {
            FileStream fs = File.Create ( m_logPath );
            fs.Close ( );
            Debug.Log ( string.Format ( "Create file = {0}" , m_logPath ) );
        }
        try
        {
            /// 注册事件，当Debug调用时，就会调用：
            Application.logMessageReceived += OnLogCallBack;
            OutputSystemInfo ( );
        }
        catch ( System.Exception ex )
        {
            Debug.LogError ( string.Format ( "can't Create file = {0},\n{1}" , m_logPath , ex ) );
        }
    }

    /// <summary>
    /// 日记文件写入的函数
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    private void OnLogCallBack ( string condition , string stackTrace , LogType type )
    {
        if ( File.Exists ( m_logPath ) )
        {
            var filestream = File.Open ( m_logPath , FileMode.Append );
            using ( StreamWriter sw = new StreamWriter ( filestream ) )
            //using (StreamWriter sw = File.AppendText(m_logPath))
            {
                string logStr = string.Empty;
                switch ( type )
                {
                    case LogType.Log:
                        logStr = string.Format ( "{0}：{1}\n" , Now ( ) + type , condition );
                        break;
                    case LogType.Assert:
                    case LogType.Warning:
                    case LogType.Exception:
                    case LogType.Error:
                        if ( string.IsNullOrEmpty ( stackTrace ) )
                            /// 发布到对应平台后，调用堆栈获取不到。使用 Environment.StackTrace 获取调用堆栈
                            logStr = string.Format ( "{0}：{1}\n{2}\n" , Now ( ) + type , condition , System.Environment.StackTrace );
                        else
                            logStr = string.Format ( "{0}：{1}\n{2}" , Now ( ) + type , condition , stackTrace );
                        break;
                }
                sw.WriteLine ( logStr );
            }
            filestream.Close ( );
        }
        else
        {
            Debug.LogError ( string.Format ( "not Exists File = {0} ！" , m_logPath ) );
        }
    }
    static private string Now ( )
    {
        return System.DateTime.Now.ToString ( "[yyyy-MM-dd HH:mm:ss:ffff]" );
    }
    /// <summary>
    /// 输出系统/硬件等一些信息
    /// </summary>
    private void OutputSystemInfo ( )
    {
        string str2 = string.Format ( "logger Start, time: {0}, version: {1}." , Now ( ) , Application.unityVersion );

        string systemInfo = SystemInfo.operatingSystem + " "
                            + SystemInfo.processorType + " " + SystemInfo.processorCount + " "
                            + "memorySize:" + SystemInfo.systemMemorySize + " "
                            + "Graphics: " + SystemInfo.graphicsDeviceName + " vendor: " + SystemInfo.graphicsDeviceVendor
                            + " memorySize: " + SystemInfo.graphicsMemorySize + " " + SystemInfo.graphicsDeviceVersion;

        Debug.Log ( string.Format ( "{0}\n\n{1}" , str2 , systemInfo ) );
    }

    public static LogWriter instance
    {
        get
        {
            if ( s_instance == null )
            {
                s_instance = new LogWriter ( );
            }
            return s_instance;
        }
    }

    public string logPath
    {
        get { return m_logPath; }
    }

    private static LogWriter s_instance;
    private string m_logPath;
}


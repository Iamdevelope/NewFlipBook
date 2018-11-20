using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

//公用函数方法都写在此文件中
public enum eCOLOR_TYPE
{
    //装备颜色
    WHITE_COLOR_TYPE = 1,        //白色    
    GREEN_COLOR_TYPE,           //绿色
    BLUE_COLOR_TYPE,            //蓝色
    PURPLE_COLOR_TYPE,          //紫色
    RED_COLOR_TYPE,             //红色  
    GOLDEN_COLOR_TYPE,          //金色
    DARKGOLD_COLOR_TYPE,        //暗金色
    YELLOW_COLOR_TYPE,          //黄色
    GRAY_COLOR_TYPE,            //灰色

}

public class Common
{
   

#if UNITY_EDITOR

    public static string GetWindowPath(string srcPath,string entension)
    {
        string dictName = Path.GetDirectoryName(srcPath);
        string FileName = Path.GetFileNameWithoutExtension(srcPath);

        string dstpath = "Assetbundles/" + dictName + "/" + FileName + entension;
        dstpath = dstpath.Replace("\\","/");
        dstpath = dstpath.ToLower();
        return dstpath;
    }

#endif

    public static void CreatePath(string path)
    {
        string NewPath = path.Replace("\\", "/");

        string[] strs = NewPath.Split('/');
        string p = "";

        for (int i = 0; i < strs.Length; ++i)
        {
            p += strs[i];

            if (i != strs.Length - 1)
            {
                p += "/";
            }

            if (!Path.HasExtension(p))
            {
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            }

        }


    }

    public static bool ClearDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return false;
            }
        }
        return true;
    }

    public static void ClearFiles(string path)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] files = dirInfo.GetFiles();   // 获取该目录下的所有文件
        foreach (FileInfo file in files) { file.Delete(); }
    }

    //获得两点之间的水平距离
    public static float GetHorizontalDis(Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(new Vector3(v1[0], 0, v1[2]), new Vector3(v2[0], 0, v2[2]));
    }

    public static byte[] GetBytesByLength(byte[] bytes, int nOffest, int length)
    {
        byte[] NewBytes = new byte[length];

        int nIndex = 0;
        for (int i = nOffest; i < nOffest + length; ++i)
        {
            if (i >= bytes.Length) break;
            NewBytes[nIndex] = bytes[i];
            nIndex++;
        }

        return NewBytes;
    }

    //获得从一点发射的射线方向偏移fDistance后获得的新点,fDistance是数值
    public static Vector3 GetRayPosition(Vector3 srcPosition, Vector3 dstPosition, float fDistance)
    {
        Vector3 NewPosition = new Vector3(0, 0, 0);

        float fOldDistance = Vector3.Distance(srcPosition, dstPosition);

        if (fOldDistance == 0)
        {
            return NewPosition;
        }

        NewPosition[0] = (dstPosition[0] - srcPosition[0]) * (fDistance / fOldDistance) + srcPosition[0];
        NewPosition[1] = (dstPosition[1] - srcPosition[1]) * (fDistance / fOldDistance) + srcPosition[1];
        NewPosition[2] = (dstPosition[2] - srcPosition[2]) * (fDistance / fOldDistance) + srcPosition[2];

        return NewPosition;
    }

    //获得从一点发射的射线方向偏移fDistance后获得的新点,fDistance是数值
    public static Vector3 GetRayPositionByPercent(Vector3 srcPosition, Vector3 dstPosition, float fPercent)
    {
        Vector3 NewPosition = new Vector3(0, 0, 0);

        float fOldDistance = Vector3.Distance(srcPosition, dstPosition);

        return Common.GetRayPosition(srcPosition, dstPosition, fOldDistance * fPercent);
    }

    //获得从一个点(绕y轴偏转角度为fAngle)延伸出去的射线上截取1米后获得的新点
    public static Vector3 GetRotatePosition(Vector3 TargetPosition, float fAngle)
    {
        return new Vector3((float)(TargetPosition[0] + 1 * Math.Sin(fAngle / 180.0f * Math.PI)), TargetPosition[1], (float)(TargetPosition[2] + 1 * Math.Cos(fAngle / 180.0f * Math.PI)));
    }

    //获得一个点相对于另一个点关于Y轴旋转的角度
    public static float GetRotateAngle(Vector3 TargetPos, Vector3 StartPos)
    {
        return (float)(Math.Atan2(StartPos[0] - TargetPos[0], StartPos[2] - TargetPos[2]) * 180.0f / Math.PI);
    }

    public static byte LOBYTE(ushort usValue)
    {
        return ((byte)(((ulong)(usValue)) & 0xff));
    }

    public static byte HIBYTE(ushort usValue)
    {
        return ((byte)((((ulong)(usValue)) >> 8) & 0xff));

    }

    public static byte[] StringToUnicode2(string text)
    {
        byte[] bytes;
        bytes = MyConvert_Convert.StringToByteArray(text, "UNICODE");
        return bytes;
    }

    //检测字符串中有没有非法字符
    //true 为有非法字符
    //false 为没有非法字符
    public static bool DetectString(string strValue)
    {
        byte[] pByName = new byte[34];
        pByName = Common.StringToUnicode2(strValue);
        byte byIndex = 1;
        byte byValue = 0;
        foreach (var byName in pByName)
        {
            //Common.LogText("byValue : " + byName);
            if (byIndex % 2 == 0)
            {
                //检测这个字符是不是中文
                if (byName == 0)
                {
                    //如果不是中文
                    //检测这个字符是不是数字或者大小字英文
                    //(48 > byValue && byValue > 57)  数字
                    //(byValue >= 65 && byValue <= 90) 大写字母
                    //(byValue >= 97 && byValue <= 122) 小写字母
                    //10是回车

                    if (!((byValue >= 48 && byValue <= 57) || (byValue >= 65 && byValue <= 90) || (byValue >= 97 && byValue <= 122) || (byValue == 10)))
                    {
                        MyLog.Log("true");
                        return true;
                    }

                }
            }
            else
            {
                byValue = byName;
            }
            byIndex++;
        }
        return false;
    }


    public static ushort MAKEWORD(byte a, byte b)
    {
        return ((ushort)(((byte)(((ulong)(a)) & 0xff)) | ((ushort)((byte)(((ulong)(b)) & 0xff))) << 8));

    }

    public static Transform GetBone(Transform trans, string boneName)
    {
        //Log.Log("boneName.name = " + boneName);
        Transform[] tran = trans.GetComponentsInChildren<Transform>();
        foreach (Transform t in tran)
        {
            //Log.Log("t.name = " + t.name);
            if (t.name == boneName)
            {
                return t;
            }

        }
        MyLog.LogError("fatal error : canot find the WeaponPoint  " + trans.name + "boneName = " + boneName);
        return null;
    }//取得相应骨骼名称的变换


    //返回一个字符串的长度
    //把一个时间字符串转换为一个豪秒数
    public static uint strTimeToMillisecond(byte[] byTime)
    {
        if (byTime.Length <= 0)
        {
            return 0;
        }
        String strTemp = MyConvert_Convert.ToUTF8String(byTime); ;

        int nYear = 0, nMonth = 0, nDay = 0, nHour = 0, nMinute = 0, nSecond = 0;
        uint nTotal = 0;


        nYear =MyConvert_Convert.ToInt32(strTemp.Substring(0, 2));
        nTotal +=MyConvert_Convert.ToUInt32(nYear * 12 * 30 * 24 * 60 * 60);


        nMonth =MyConvert_Convert.ToInt32(strTemp.Substring(3, 2));
        nTotal +=MyConvert_Convert.ToUInt32(nMonth * 30 * 24 * 60 * 60);


        nDay =MyConvert_Convert.ToInt32(strTemp.Substring(6, 2));
        nTotal +=MyConvert_Convert.ToUInt32(nDay * 24 * 60 * 60);

        nHour =MyConvert_Convert.ToInt32(strTemp.Substring(9, 2));
        nTotal +=MyConvert_Convert.ToUInt32(nHour * 60 * 60);


        nMinute =MyConvert_Convert.ToInt32(strTemp.Substring(12, 2));
        nTotal +=MyConvert_Convert.ToUInt32(nMinute * 60);


        nSecond =MyConvert_Convert.ToInt32(strTemp.Substring(15, 2));
        nTotal +=MyConvert_Convert.ToUInt32(nSecond);
        //Log.Log("nYear : " + nYear + "nMonth : " + nMonth + "nDay : " + nDay + "nHour : " + nHour + "nMinute : " + nMinute + "nSecond : " + nSecond + "nTotal : " + nTotal);
        return nTotal;
    }

    public static int TextLength(string szText)
    {
        int len = 0;

        for (int i = 0; i < szText.Length; ++i)
        {
            byte[] byte_len = MyConvert_Convert.StringToByteArray(szText.Substring(i, 1), "gb2312");
            if (byte_len.Length > 1)
            {
                len += 2;
            }
            else
            {
                len += 1;
            }
        }

        return len;
    }


    public static string TimeToString(float fTime)
    {
        string sTime = "";
        uint uiH = (uint)(fTime / 3600);
        uint uiM = (uint)((fTime % 3600) / 60);
        uint uiS = (uint)(fTime % 60);

        string sH = "";
        string sM = "";
        string sS = "";

        if (uiM < 10)
        {
            sM = "0" +MyConvert_Convert.ToString(uiM);
        }
        else if (uiM < 60)
        {
            sM =MyConvert_Convert.ToString(uiM);
        }
        else
        {
            MyLog.LogError(" ____________________ 时间换字符串出错！（在分针上）");
            return "";
        }

        if (uiS < 10)
        {
            sS = "0" +MyConvert_Convert.ToString(uiS);
        }
        else if (uiS < 60)
        {
            sS =MyConvert_Convert.ToString(uiS);
        }
        else
        {
            MyLog.LogError(" ____________________ 时间换字符串出错！（在秒针上）");
            return "";
        }

        if (uiH > 0)
        {
            sTime =MyConvert_Convert.ToString(uiH) + ":" + sM + ":" + sS;
        }
        else
        {
            sTime = sM + ":" + sS;
        }

        return sTime;
    }

    //根据一个字符串,判断是否为数字
    public static bool IsNumber(string szText)
    {
        bool rlt = true;

        Regex reg = new Regex("^[0-9]+$");
        Match ma = reg.Match(szText);

        rlt = ma.Success;

        return rlt;
    }

    public static Color GetColor(eCOLOR_TYPE eColorType)
    {
        switch (eColorType)
        {
            case eCOLOR_TYPE.WHITE_COLOR_TYPE:
                {
                    return new Color(1.0f, 1.0f, 1.0f);//255,255,255
                }
            case eCOLOR_TYPE.GREEN_COLOR_TYPE:
                {
                    return new Color(0.0f, 1.0f, 0.0705f); //0,255,18
                }
            case eCOLOR_TYPE.BLUE_COLOR_TYPE:
                {
                    return new Color(0.0f, 0.7764f, 1.0f);//0,198,255
                }
            case eCOLOR_TYPE.PURPLE_COLOR_TYPE:
                {
                    return new Color(0.5411f, 0.0f, 1f);//138,0,255
                }
            case eCOLOR_TYPE.RED_COLOR_TYPE:
                {
                    return new Color(1f, 0.0f, 0.0f);//255,0,0
                }
            case eCOLOR_TYPE.GOLDEN_COLOR_TYPE:
                {
                    return new Color(0.9843f, 0.7411f, 0.0823f);//251,189,21
                }
            case eCOLOR_TYPE.DARKGOLD_COLOR_TYPE:
                {
                    return new Color(0.7529f, 0.4117f, 0.0431f);//192,105,11
                }
            case eCOLOR_TYPE.YELLOW_COLOR_TYPE:
                {
                    return new Color(1f, 0.98f, 0.074f);//255,251,19
                }
        }

        return new Color(1f, 1f, 1f, 1f);
    }

    //服务端的名字转换成客户端的名字，如：Name*100001 转换成 [S1]Name 
    public static string ServerName(string src)
    {
        string rlt = "";

        string[] temp = src.Split('*');
        if (temp.Length == 2)
        {
            //temp
            rlt = "[S" + (MyConvert_Convert.ToInt32(temp[1]) % 1000) + "]"; //server
            rlt += temp[0];//name
        }
        else
        {
            Debug.LogWarning("server name is wrong , param src is :" + src);
        }

        return rlt;
    }

    //客户端名字转成服务端使用的名字，如：[S1]Name 转换成 Name*100001
    public static string ToServerName(string src)
    {
        string rlt = "";

        int iIndex = src.IndexOf(']');
        if (iIndex == -1)
        {
            Debug.LogWarning("to server name is wrong , param src is :" + src);
        }
        else
        {
            string szTempA = src.Substring(2, iIndex - 2);

            if (IsNumber(szTempA) == false)
            {
                Debug.LogWarning("to server name is wrong , param src is :" + src);
            }
            else
            {
                string szTempB = src.Substring(iIndex + 1);
                rlt += szTempB; //name
                rlt += "*";

                int iServer = MyConvert_Convert.ToInt32(szTempA);
                int iPlatform = 100000;//如果有以后有其他平台，这个位置需要更改
                rlt += (iServer + iPlatform).ToString();
            }


        }

        return rlt;
    }

    //设置层次(包含子类)
    public static void SetObjectAlllayer(GameObject o, int layer)
    {
        Transform[] trans = o.GetComponentsInChildren<Transform>();
        foreach (Transform tran in trans)
        {
            tran.gameObject.layer = layer;
        }
        o.layer = layer;
    }

    public static string CurrTimeString
    {
        get
        {
            return DateTime.Now.Month.ToString() + "_"
                + DateTime.Now.Day.ToString() + "_"
                + DateTime.Now.Hour.ToString() + "_"
                + DateTime.Now.Minute.ToString() + "_"
                + DateTime.Now.Second.ToString();
        }
    }

}
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PJW.HotUpdate
{
    public class GetMD5FromFile
    {
        public static List<string> fileName = new List<string>();
        public static List<string> fileMD5 = new List<string>();
        /// <summary>
        /// 得到资源的MD5值
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static string GetAssetOfMD5(string assetPath)
        {
            try
            {
                //FileStream fs = new FileStream(assetPath, FileMode.Append);

                byte[] bytes = Encoding.UTF8.GetBytes(assetPath);
                //fs.Read(bytes, 0, bytes.Length);
                //fs.Close();
                MD5 fileMD5 = new MD5CryptoServiceProvider();
                byte[] md5Bytes = fileMD5.ComputeHash(bytes);
                string str = "";
                foreach (var item in md5Bytes)
                {
                    str += Convert.ToString(item, 16);
                }
                return str;
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e);
                return "";
            }
        }
        public static void GetAllAssetOfMD5(string assetPath,Action callBack)
        {
            string[] arr = Directory.GetFiles(assetPath);
            for (int i = 0; i < arr.Length; i++)
            {
                fileName.Add(arr[i]);
                fileMD5.Add(GetAssetOfMD5(arr[i]));
            }
            if (callBack != null)
                callBack();
        }
    }
}
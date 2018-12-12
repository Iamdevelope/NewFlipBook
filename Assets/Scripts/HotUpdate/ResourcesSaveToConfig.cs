
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace PJW.HotUpdate
{
    [SerializeField]
    public class MyFileInfo
    {
        public string FileMD5 { get; set; }
        public string Version { get; set; }
        //是否存在包外
        public bool IsFinish { get; set; }
        //是否需要解压下载文件
        public bool IsUnZip { get; set; }
    }
    [SerializeField]
    public class MyFileName
    {
        public FileInfo FileInfo { get; set; }
    }
    /// <summary>
    /// 生成文件的资源xml文件
    /// </summary>
    public class ResourcesSaveToConfig
    {
        private static Dictionary<string, string> fileMD5 = new Dictionary<string, string>();
        public static void ResToConfig(string savePath)
        {
            if (savePath.EndsWith(".json"))
            {
                string json = JsonMapper.ToJson(fileMD5);
                File.WriteAllText(savePath, json);
            }
            else if (savePath.EndsWith(".xml"))
            {

            }
        }
        /// <summary>
        /// 通过资源名字得到资源的MD5值
        /// </summary>
        /// <param name="fileName">资源名</param>
        public static string GetResMD5(string fileName)
        {
            string str = "";
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                MD5 mD5 = new MD5CryptoServiceProvider();
                byte[] bytes = mD5.ComputeHash(fs);
                foreach (var item in bytes)
                {
                    str += Convert.ToString(item, 16);
                }
            }
            catch (FileNotFoundException e)
            {
                str = "";
            }
            return str;
        }
        /// <summary>
        /// 得到目录下的所有文件的MD5
        /// </summary>
        /// <param name="filePath"></param>
        public static void GetAllResMD5(string filePath, string savePath)
        {
            DirectoryInfo directories = new DirectoryInfo(filePath);
            SetAssetBundleName(directories, savePath);
            if (fileMD5.Count > 0)
                ResToConfig(savePath);
        }
        private static void SetAssetBundleName(DirectoryInfo dirInfo, string savePath)
        {
            FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo file in files)
            {
                if (file is FileInfo && file.Extension != ".meta" && file.Extension != ".txt")
                {
                    string temp = file.FullName.Replace('\\', '/');
                    temp = temp.Replace(Application.persistentDataPath, "");
                    if (!fileMD5.ContainsKey(temp))
                        fileMD5[temp] = GetResMD5(file.FullName);
                }
                else if (file is DirectoryInfo)
                {
                    SetAssetBundleName(file as DirectoryInfo, savePath);
                }
            }
        }
    }
}
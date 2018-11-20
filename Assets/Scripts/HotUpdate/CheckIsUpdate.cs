using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace PJW.HotUpdate
{
    /// <summary>
    /// 检查是否需要更新
    /// </summary>
    public class CheckIsUpdate : MonoBehaviour
    {
        public string url;
        public string fileName;
        private string savePath = @"D:/Test/";
        private void Start()
        {
            GetMD5FromFile.GetAllAssetOfMD5(url, GetOver);
        }
        public void GetOver()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            string text = "";
            for (int i = 0; i < GetMD5FromFile.fileName.Count; i++)
            {
                text += string.Format("\r\n" + GetMD5FromFile.fileName[i] + "|" + GetMD5FromFile.fileMD5[i]);
            }
            text = text.TrimStart();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            FileStream stream = File.Open(savePath + fileName, FileMode.OpenOrCreate);
            //FileStream stream = new FileStream(savePath, FileMode.Append);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            stream.Dispose();
        }

        private void WWWDownCall(WWW obj)
        {
            Debug.Log("WWW isDone");
        }

        private void DownOver()
        {
            Debug.Log("资源下载完成。");
        }
    }
}
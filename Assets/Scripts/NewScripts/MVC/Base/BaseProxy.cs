using PJW.MVC.Patterns;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;
using PJW.Book;
using System;
using System.Collections;

namespace PJW.MVC.Base
{
    /// <summary>
    ///处理数据库功能
    /// </summary>
    public class BaseProxy : Proxy
    {
        private string dbName = "flipBook.db";
        protected SqliteDataReader reader;
        protected MessageData MessageData;
        protected DbAccess db;
        private string dbPath;
        /// <summary>
        /// 打开数据库
        /// </summary>
        public void OpenDB()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                dbPath = Application.persistentDataPath + "/" + dbName;
                if (!File.Exists(dbPath))
                    GameCore.Instance.StartCoroutine(CopyDB());
            }
            else if(Application.platform==RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                dbPath = Application.streamingAssetsPath + "/" + dbName;
            }
            db = new DbAccess("URI=file:" + dbPath);
        }
        private IEnumerator CopyDB()
        {
            WWW www = new WWW(Application.streamingAssetsPath + "/" + dbName);
            yield return www;
            File.WriteAllBytes(dbPath, www.bytes);
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void CloseDB()
        {
            db.CloseSqlConnection();
            reader = null;
        }
    }
}

using UnityEngine;
using DG.Tweening;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections;
using System;

namespace PJW.Book.UI
{
    public class BasePanel : MonoBehaviour
    {
        public delegate void ButtonClickedEvent(string name);
        public event ButtonClickedEvent ClassTypeButtonHandle;
        public event ButtonClickedEvent ClassButtonHandle;
        public event Action EnterClickedEvent;
        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (EnterClickedEvent != null)
                    EnterClickedEvent();
            }
        }
        public virtual void Init() { }
        public virtual void Reset(Vector3 scale,float t,string msg="")
        {
            transform.DOScale(scale, t);
        }
        public void PlayClickSound()
        {
            GameCore.Instance.PlaySoundBySoundName();
        }
        public virtual void StartAnim() { }
        public virtual void OverAnim() { }
        public virtual void OverAnim(string nextName) { }
        
        
        protected DbAccess db;// 数据库操作类
        private string dbName = "flipBook.db"; // 数据库名称
        private string dbPath; // 数据库路径
        protected SqliteDataReader reader;
        /// <summary>
        /// 打开数据库
        /// </summary>
        protected void OpenDB()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
                dbPath = Application.streamingAssetsPath + "/" + dbName;
            else if (Application.platform == RuntimePlatform.Android)
            {
                dbPath = Application.persistentDataPath + "/" + dbName;
                Debug.Log(File.Exists(dbPath) + " " + dbPath);
                if (!File.Exists(dbPath)) // 如果数据库不存在,则复制到持久化目录下
                    StartCoroutine(CopyDB());
            }
            db = new DbAccess("URI=file:" + dbPath);
        }
        /// <summary>
        /// 复制文件到持久化目录
        /// </summary>
        /// <returns></returns>
        private IEnumerator CopyDB()
        {
            // 从StreamingAssets目录使用WWW下载data.db
            WWW www = new WWW(Application.streamingAssetsPath + "/" + dbName);
            yield return www; // 等待下载完毕
            Debug.Log("下载完毕");
            //下载完毕后写到persistentDataPath路径
            File.WriteAllBytes(dbPath, www.bytes);
            Debug.Log("写入完毕");
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        protected void CloseDB()
        {
            db.CloseSqlConnection();
            reader = null;
        }
        protected string GetStr(object o)
        {
            return "'" + o + "'";
        }
    }
}
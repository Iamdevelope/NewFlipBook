using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCommon;
using PJW.Book.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace PJW.Book
{
    /// <summary>
    /// 核心类
    /// </summary>
    public class GameCore : MonoSingleton<GameCore> {
        public static List<string> allBooksName = new List<string>();
        public static GameData GameData;
        public static GameObject CurrentObject;
        public static GameObject CharacterCamera;
        [HideInInspector]
        public float effectPositionZ = 1;
        [HideInInspector]
        public GameObject TouchEffect;
        [HideInInspector]
        public static UIManager uiManager;
        [HideInInspector]
        private SoundManager soundManager;
        private GeneratePage generatePage;
        private BookDummy bookDummy;
        private GenerateBookStore generateBookStore;
        [HideInInspector]
        public AssetBundle asset;
        public float fpsMeasuringDelta = 0.1f;
        private float timePassed;
        private int m_FrameCount = 0;
        private float m_FPS = 0.0f;

        public string SavePath
        {
            get
            {
                return Application.streamingAssetsPath + "/ABFiles/";
            }
        }
        public string LocalXMLPath
        {
            get { return Application.persistentDataPath + "/Books/XMLContent/";  }
        }
        public string URL
        {
            get
            {
                return @"C:\Users\User\AppData\LocalLow\YG\FlipBook\AssetBundle\";
            }
        }
        public BookDummy BookDummy
        {
            get { return bookDummy; }
            set { if (bookDummy == null) bookDummy = value; }
        }
        public GeneratePage GeneratePage { get
            {
                return generatePage;
            }
            set
            {
                if (generatePage == null)
                    generatePage = value;
            }
        }
        public GenerateBookStore GenerateBookStore
        {
            get { return generateBookStore; }
            set { if (generateBookStore == null) generateBookStore = value; }
        }
        public SoundManager SoundManager
        {
            get { return soundManager; }
            private set { if (soundManager == null) soundManager = value; }
        }
        public bool isSuccessLogin { get; set; }
        //test
        private NewGenerateBookstore generateBookstore;
        public NewGenerateBookstore NewGenerateBookstore {
            get { return generateBookstore; }
            set { generateBookstore = value; }
        }

        private string dbName = "flipBook.db"; // 数据库名称
        private string dbPath; // 数据库路径
        /// <summary>
        /// 游戏初始化
        /// </summary>
        public void Init()
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

            CharacterCamera = FindObjectOfType<CharacterCamera>().gameObject;
            if (CharacterCamera != null)
                CharacterCamera.SetActive(false);
            TouchEffect = Resources.Load<GameObject>("Effect/TouchEffect");

            uiManager = FindObjectOfType<UIManager>();
            soundManager = GetComponent<SoundManager>();


            uiManager.Init();
            soundManager.Init();
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
        private void LateUpdate()
        {
            m_FrameCount = m_FrameCount + 1;
            timePassed = timePassed + Time.deltaTime;

            if (timePassed > fpsMeasuringDelta)
            {
                m_FPS = m_FrameCount / timePassed;

                timePassed = 0.0f;
                m_FrameCount = 0;
            }
        }
        private void FixedUpdate()
        {

            if (Input.GetMouseButton(0))
            {
                if (SceneManager.GetActiveScene().name != "Main")
                {
                    Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, effectPositionZ));
                    GameObject go = GameObjectPool.Instance.CreateObject("Effect", TouchEffect, temp, Quaternion.identity);
                    PlaySoundBySoundName(SoundManager.CLICKDRAG);
                    GameObjectPool.Instance.CollectObject(go, 0.4f);
                    //Instantiate(TouchEffect, temp, Quaternion.identity);
                }

            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        private void OnGUI()
        {
            GUIStyle bb = new GUIStyle();
            bb.normal.background = null;
            bb.normal.textColor = new Color(1.0f, 1f, 1.0f, 0.8f);
            bb.fontSize = 10;
            // 右上角显示
            GUI.Label(new Rect(Screen.width - 60, Screen.height-20, 200, 200), "FPS: " + Mathf.Floor(m_FPS), bb);
        }
        /// <summary>
        /// 给消息提示面板发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessageToMessagePanel(string msg)
        {
            uiManager.SendMessageToMessagePanel(msg);
        }
        /// <summary>
        /// 打开下一个需要显示的UI面板
        /// </summary>
        /// <param name="panel"></param>
        public void OpenNextUIPanel(GameObject panel,string msg="")
        {
            uiManager.OpenNextUIPanel(panel,msg);
        }
        /// <summary>
        /// 关闭当前显示的UI
        /// </summary>
        public void CloseCurrentUIPanel()
        {
            uiManager.CloseCurrentUIPanel();
        }
        /// <summary>
        /// 打开等待面板
        /// </summary>
        public void OpenLoadingPanel(Vector3 vector)
        {
            uiManager.OpenLoadingPanel(vector);
        }
        /// <summary>
        /// 播放语音
        /// </summary>
        /// <param name="audio"></param>
        public void PlaySound(AudioClip audio)
        {
            soundManager.clipStack.Push(audio);
            soundManager.PlayAudioClip();
        }
        /// <summary>
        /// 设置声音大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundSize(float volume)
        {
            soundManager.SetSoundSize(volume);
        }
        /// <summary>
        /// 按钮点击时的声音
        /// </summary>
        /// <param name="name">需要播放的声音的名字</param>
        public void PlaySoundBySoundName(string name=SoundManager.CLICK_01)
        {
            soundManager.PlayAudioClip(name);
        }
    }
}
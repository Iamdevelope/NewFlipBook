
using System.Collections.Generic;
using UnityEngine;
using MyCommon;
using PJW.Book.UI;
using UnityEngine.SceneManagement;
using PJW.HotUpdate;
using cn.sharesdk.unity3d;

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
        public ShareSDK ssdk;
        [HideInInspector]
        public float effectPositionZ = 1;
        [HideInInspector]
        public GameObject TouchEffect;
        [HideInInspector]
        public static UIManager uiManager;
        [HideInInspector]
        private SoundManager soundManager;
        private GeneratePage generatePage;
        private GenerateBookStore generateBookStore;
        [HideInInspector]
        public AssetBundle asset;
        [HideInInspector]
        public WWW www;
        public float fpsMeasuringDelta = 0.1f;
        private float timePassed;
        private int m_FrameCount = 0;
        private float m_FPS = 0.0f;

        public string SavePath
        {
            get
            {
                return Application.persistentDataPath + "/AssetBundles/";
                //Application.streamingAssetsPath + "/ABFiles/";
            }
        }
        public string LocalConfigPath
        {
            get { return Application.persistentDataPath + "/Books"; }
        }
        public string URL
        {
            get
            {
                return @"ftp://192.168.1.110:66" + "/ABFiles/";
            }
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
        public NewGenerateBookstore NewGenerateBookstore { get; set; }


        /// <summary>
        /// 游戏初始化
        /// </summary>
        public void Init()
        {
            //设置帧率
            Application.targetFrameRate = 30;
            ssdk = FindObjectOfType<ShareSDK>();
            CharacterCamera = FindObjectOfType<CharacterCamera>().gameObject;
            if (CharacterCamera != null)
                CharacterCamera.SetActive(false);
            TouchEffect = Resources.Load<GameObject>("Effect/TouchEffect");

            uiManager = FindObjectOfType<UIManager>();
            soundManager = GetComponent<SoundManager>();

            uiManager.Init();
            soundManager.Init();

            //进行资源判断，是否需要进行更新
            FindObjectOfType<CheckIsUpdate>().Init();
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
                //Application.Quit();
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
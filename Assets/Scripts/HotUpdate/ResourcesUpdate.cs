using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using UnityEngine;

namespace PJW.HotUpdate
{
    public struct ResourcesReference
    {
        //是否存在包外
        public bool isFinish;
        //是否需要解压下载文件
        public bool isUnZip;
        //资源的版本号
        public int version;
        //资源MD5值
        public string fileMD5;
        public ResourcesReference(bool isFinish, int version, bool isUnZip,string fileMD5)
        {
            this.isFinish = isFinish;
            this.version = version;
            this.isUnZip = isUnZip;
            this.fileMD5 = fileMD5;
        }
    }
    /// <summary>
    /// 资源更新
    /// </summary>
    public class ResourcesUpdate
    {
        public static ResourcesUpdate Instance;
        //服务器资源目录
        public static string RESOURCES_SERVER_PATH;
        //本地资源保存目录
        public static string LOCAL_RESOURCES_PATH;
        //资源文件信息文件
        public static string MAIN_VERSION_FILE = "fileInfo.xml";
        //本地解压目录
        public static string LOCAL_DECOMPRESS_RES = "";

        //需要下载的资源列表
        private static List<string> NeadDownAsset = new List<string>();
        //服务器资源及其对应的版本
        private static Dictionary<string, int> ServerResourcesVersion = new Dictionary<string, int>();
        //本地资源及其对应的版本
        private static Dictionary<string, ResourcesReference> LocalResourcesVersion = new Dictionary<string, ResourcesReference>();

        private Thread thread;

        static ResourcesUpdate()
        {
            Instance = new ResourcesUpdate();
            if(Application.platform==RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                RESOURCES_SERVER_PATH = "http://127.0.0.1:88/AssetBundle/Windows32/";
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                RESOURCES_SERVER_PATH = "http://192.168.0.106:80/AssetBundle/Android/";
            }
            LOCAL_RESOURCES_PATH = Application.persistentDataPath + "/Books/";
            LOCAL_DECOMPRESS_RES = LOCAL_RESOURCES_PATH + "DeCompress/";
        }

        public static void StartDownLoad(bool isAuto = true)
        {

        }
        /// <summary>
        /// 加载版本
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadVersion()
        {
            NeadDownAsset.Clear();
            ServerResourcesVersion.Clear();

            string localResourcesFile = Path.Combine(LOCAL_RESOURCES_PATH, MAIN_VERSION_FILE);
            //如果本地已经进行过资源更新了，则存在资源配置文件
            if (File.Exists(localResourcesFile))
            {
                Instance.ParseXMLFileToDictionary(localResourcesFile, LocalResourcesVersion);
            }
            string serverResourcesFile = RESOURCES_SERVER_PATH + MAIN_VERSION_FILE;
            WWW www = new WWW(serverResourcesFile);
            yield return www;

        }
        /// <summary>
        /// 解析XML版本配置文件，将其转化为字典数据
        /// </summary>
        /// <param name="xmlFile">xml文件路径</param>
        /// <param name="dic">保存的数据</param>
        public void ParseXMLFileToDictionary(string xmlFile, Dictionary<string, ResourcesReference> dic)
        {
            if (xmlFile == null || xmlFile.Length == 0)
            {
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(ConfigFileElement.FILE);
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlAttribute xmlAttribute = nodeList[i].Attributes[ConfigFileElement.FILENAME];

            }
        }
    }
}
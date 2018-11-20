using PJW.Book;
using PJW.Book.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace PJW.HotUpdate
{
    /// <summary>
    /// 利用AssetBundle加载资源
    /// </summary>
    public class LoadAssetByAssetBundle : MonoBehaviour
    {
        private List<string> updateAssetName = new List<string>();
        private WWW progressWWW;
        private int index = 0;
        [HideInInspector]
        public Text progressText;
        [HideInInspector]
        public Slider slider;
        private WWW www;
        public string assetBundleName;
        public bool isLoadAssetByAsset;

        private void Start()
        {
            if (!Directory.Exists(GameCore.Instance.SavePath))
                Directory.CreateDirectory(GameCore.Instance.SavePath);
            if (isLoadAssetByAsset)
                OnClickUpdate();
        }

        private void Update()
        {
            if (progressWWW != null && !progressWWW.isDone && progressText != null)
            {
                progressText.text = string.Format("下载进度:{0:F}% \n当前正在下载第{1}个资源，共{2}个资源。", progressWWW.progress * 100, index, updateAssetName.Count);
                slider.value = progressWWW.progress;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void OnClickUpdate()
        {
            StartCoroutine(DownLoadMainfest(GameCore.Instance.URL, GameCore.Instance.SavePath));
        }
        /// <summary>
        /// 将保存所有Asset的Manifest下载，然后进行一一比对，判断是否有需要下载的资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">资源保存的路径</param>
        /// <returns></returns>
        private IEnumerator DownLoadMainfest(string url, string savePath)
        {

            string[] urlAllAssetNames = null;
            www = new WWW(url + "\\" + assetBundleName);
            yield return www;
            GameCore.Instance.asset = www.assetBundle;
            AssetBundleManifest manifest = GameCore.Instance.asset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            urlAllAssetNames = manifest.GetAllAssetBundles();
            if (Directory.Exists(savePath))
            {
                DirectoryInfo directory = new DirectoryInfo(savePath);
                FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);

                foreach (var item in urlAllAssetNames)
                {
                    GameCore.allBooksName.Add(item);
                    updateAssetName.Add(item);
                }
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (GameCore.allBooksName.Contains(file.Name) && !file.Name.EndsWith(".meta"))
                            updateAssetName.Remove(file.Name);
                    }
                }
            }
            if (updateAssetName.Count > 0)
            {
                Debug.Log("有资源需要更新");
                slider.gameObject.GetComponentInParent<DownLoadPanel>().Reset(Vector3.one, 0.3f);
                DownLoad(savePath);
            }
            else
            {
                Debug.Log("没有资源需要更新");
                //没有资源需要更新
                www.Dispose();
                www = null;
            }
            
        }
        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="savePath"></param>
        private void DownLoad(string savePath)
        {
            if (index >= updateAssetName.Count)
            {
                www.Dispose();
                www = null;
                progressWWW = null;
                progressText.text = string.Format("下载进度:{0:F}% \n当前正在下载第{1}个资源，共{2}个资源。", 100, updateAssetName.Count, updateAssetName.Count);
                slider.value = 1;
                StartCoroutine(slider.gameObject.GetComponentInParent<DownLoadPanel>().DownOver());
                //需要更新的资源下载完毕,将需要更新的资源列表清空
                updateAssetName.Clear();
                Debug.Log("所有资源更新完毕");
                return;
            }
            StartCoroutine(GenerateAssetBundle(GameCore.Instance.URL, savePath, updateAssetName[index], () => {
                DownLoad(savePath);
                index++;
                }));
        }

        /// <summary>
        /// 创建AssetBundle资源
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="savePath">下载的资源保存路径</param>
        /// <param name="assetBundleName">资源名字</param>
        /// <param name="callBack">回调</param>
        public IEnumerator GenerateAssetBundle(string url, string savePath, string assetBundleName, Action callBack)
        {
            progressWWW = new WWW(url + "/" + assetBundleName);
            while (!progressWWW.isDone)
            {
                yield return null;
                Debug.Log("正在下载资源...");
            }
            yield return progressWWW;
            if (progressWWW.isDone)
            {
                Debug.Log("资源下载完毕...");
                CreateFile(savePath + assetBundleName, progressWWW.bytes, callBack);
            }
        }

        /// <summary>
        /// 将下载的资源保存到指定路劲
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="bytes"></param>
        private void CreateFile(string savePath, byte[] bytes, Action callBack)
        {
            FileStream fs = new FileStream(savePath, FileMode.Append);
            fs.Write(bytes, 0, bytes.Length);
            //利用文件流进行写数据时，会进行缓存，Flush就是不让它缓存，直接写到文件
            fs.Flush();
            //关闭流，并将所有资源释放
            fs.Close();
            //释放流
            fs.Dispose();
            progressWWW.Dispose();
            //释放www
            if (callBack != null)
                callBack();
        }

        public void Reset()
        {
            File.Delete(GameCore.Instance.SavePath);
        }
    }
}
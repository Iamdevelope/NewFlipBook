using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyCommon;
using UnityEngine.SceneManagement;

namespace PJW.Book
{

    public class CanvasController : MonoSingleton<CanvasController>
    {
        private InputField inputField;
        public Text allPage;
        private void Start()
        {
            transform.Find("SkipPage").GetComponent<Button>().onClick.AddListener(SkipPageHandle);
            inputField = GetComponentInChildren<InputField>();
        }

        public void ShowAllPage(int page)
        {
            allPage.text = "共" + page + "页";
        }

        private void SkipPageHandle()
        {
            int num;
            if(CheckInt(inputField.text,out num))
            {
                Debug.Log(num);
                GameCore.Instance.GeneratePage.SkipPageToTargetPage(num);
            }
            else
            {
                Debug.LogWarning("输入的页码有误，请重新输入。");
            }
        }
        public void UpdatePageNum(int page)
        {
            if (page != 0)
                inputField.text = page.ToString();
            else if (page == 0)
                inputField.text = "";
        }
        /// <summary>
        /// 检查传入过来的对象是否为整数
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CheckInt(string o,out int result)
        {
            bool isTrue = false;
            if (int.TryParse(o, out result))
                isTrue = true;
            return isTrue;
        }
        public void BackClickHandle()
        {
            GameCore.Instance.PlaySoundBySoundName();
            GameCore.Instance.SoundManager.source.clip = null;
            if (GameCore.Instance.asset != null)
            {
                //卸载所有AssetBundle资源
                GameCore.Instance.asset.Unload(true);
                GameCore.Instance.asset = null;
            }
            //将持久化数据删除
            PlayerPrefs.DeleteKey("AssetBundle");
            if (GameCore.Instance.GeneratePage.notGenerateSoundManager)
                GameCore.Instance.SoundManager.Reset();
            else
                GameCore.Instance.GeneratePage.soundManager.Reset();
            Globe.nextSceneName = "Bookstore";
            SceneManager.LoadScene("Loading");
        }
    }
}
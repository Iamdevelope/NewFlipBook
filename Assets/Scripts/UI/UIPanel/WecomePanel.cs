using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace PJW.Book.UI
{
    /// <summary>
    /// 欢迎面板
    /// </summary>
    public class WecomePanel : BasePanel {

        public override void Init()
        {
            transform.Find("EnterButton").GetComponent<Button>().onClick.AddListener(EnterClickHandle);
        }

        private void EnterClickHandle()
        {
            FindObjectOfType<GameInit>().SaveSceneData();
            Globe.nextSceneName = "Bookstore";
            SceneManager.LoadScene("Loading");
        }
        

        public override void Reset(Vector3 scale, float t,string msg)
        {
            if (msg != "")
            {
                string str = "欢迎来到" + msg + "!";
                GetComponentInChildren<Text>().text = str;
            }
            //transform.DOScale(scale, t);
            base.Reset(scale, t, msg);
        }
    }
}

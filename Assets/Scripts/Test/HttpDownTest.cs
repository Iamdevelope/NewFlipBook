using PJW.Book.UI;
using PJW.HotUpdate;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpDownTest : MonoBehaviour {
    private HTTPLoad http;
    private DownLoadPanel loadPanel;
    private string url = @"ftp://192.168.1.110:66/AllBookImage.rar";
    private void Start()
    {
        http = new HTTPLoad();
        loadPanel = FindObjectOfType<DownLoadPanel>();
        http.DownLoadByFTP(url, Application.persistentDataPath, "AllBookImage.rar", DownOver);
    }
    private void Update()
    {
        if (http!=null)
        {
            loadPanel.Reset(Vector3.one, 0.3f);
            loadPanel.slider.value = http.progress;
        }
    }
    private void DownOver()
    {
        Debug.Log("资源下载完成");
    }
}

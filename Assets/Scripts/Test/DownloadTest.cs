using PJW;
using PJW.Download;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadTest : MonoBehaviour {
    private DownloadManager downloadManager;
    private DownloadAgentTest agentTest;

    private void Start()
    {
        agentTest = new DownloadAgentTest();
        downloadManager = new DownloadManager();
        downloadManager.DownLoadStartHandler += DownloadStart;
        downloadManager.DownLoadUpdateHandler += DownloadUpdate;
        downloadManager.DownLoadSuccessHandler += DownloadSuccess;
        downloadManager.AddDownloadAgent(agentTest);
        downloadManager.AddDownload(Application.persistentDataPath, Application.persistentDataPath + "/Books/filePathAndName.json");
        
    }

    private void DownloadSuccess(object sender, DownloadSuccessEventArgs e)
    {
        Debug.Log(Utility.Text.Format(" the sender is : {0}, the asset of save of path : {1} ", sender.ToString(), e.DownloadPath));
    }

    private void Update()
    {
        downloadManager.Update(1, Time.deltaTime);
    }


    private void DownloadUpdate(object sender, DownloadUpdateEventArgs e)
    {
        Debug.Log(" the download is runing ");
    }
    private void DownloadStart(object sender, DownloadStartEventArgs e)
    {
        Debug.Log(" the download is start ");
    }
}

public class DownloadAgentTest : IDownLoadAgentManager
{
    public event EventHandler<DownloadAgentManagerErrorEventArgs> DownloadAgentErrorHandler;
    public event EventHandler<DownloadAgentManagerSuccessEventArgs> DownloadAgentSuccessHandler;
    public event EventHandler<DownloadAgentManagerUpdateEventArgs> DownloadAgentUpdateHandler;

    public void Download(string downloadUrl, object userData)
    {
        Debug.Log(" is down load ... ");
    }

    public void Download(string downloadUrl, float formPosition, object userData)
    {
        throw new NotImplementedException();
    }

    public void Download(string downloadUrl, float formPosition, float toPosition, object userData)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        Debug.Log("reset");
    }
}
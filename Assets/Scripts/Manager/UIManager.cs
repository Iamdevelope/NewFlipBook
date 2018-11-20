using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book.UI
{
    /// <summary>
    /// UI控制器
    /// </summary>
    public class UIManager : MonoBehaviour
    {

        private MessagePanel messagePanel;
        private LoadingPanel loadingPanel;
        private Stack<GameObject> uiPanel = new Stack<GameObject>();
        private List<GameObject> allUIPanel = new List<GameObject>();
        public void Init()
        {
            BasePanel[] basePanels = FindObjectsOfType<BasePanel>();
            if (GameCore.GameData==null)
            {
                for (int i = 0; i < basePanels.Length; i++)
                {
                    basePanels[i].Init();
                    if (basePanels[i].gameObject.name.Equals("MessageTipPanel"))
                        messagePanel = basePanels[i].GetComponent<MessagePanel>();
                    if (basePanels[i].gameObject.name.Equals("LoadingPanel"))
                        loadingPanel = basePanels[i].GetComponent<LoadingPanel>();
                    if (basePanels[i].gameObject.name != "LoginPanel")
                        basePanels[i].Reset(Vector3.zero, 0);
                    else
                        uiPanel.Push(basePanels[i].gameObject);
                }
            }
            else
            {
                for (int i = 0; i < basePanels.Length; i++)
                {
                    basePanels[i].Init();
                    if (basePanels[i].gameObject.name.Equals("MessageTipPanel"))
                        messagePanel = basePanels[i].GetComponent<MessagePanel>();
                    if (basePanels[i].gameObject.name.Equals("LoadingPanel"))
                        loadingPanel = basePanels[i].GetComponent<LoadingPanel>();
                    basePanels[i].Reset(Vector3.zero, 0);
                }
            }
            //GameObject[] games = Resources.LoadAll<GameObject>("UI/");
            //foreach (var item in games)
            //{
            //    CreateUIPanel(item);
            //}
        }
        /// <summary>
        /// 给消息提示面板发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessageToMessagePanel(string msg)
        {
            messagePanel.ShowMessage(msg);
        }
        /// <summary>
        /// 打开下一个需要显示的UI面板
        /// </summary>
        /// <param name="panel"></param>
        public void OpenNextUIPanel(GameObject panel,string msg)
        {
            if (uiPanel.Count > 0)
            {
                GameObject temp = uiPanel.Pop();
                temp.GetComponent<BasePanel>().Reset(Vector3.zero, 0.6f,msg);
            }
            uiPanel.Push(panel);
            panel.GetComponent<BasePanel>().Reset(Vector3.one, 0.6f,msg);
        }
        /// <summary>
        /// 关闭UI
        /// </summary>
        public void CloseCurrentUIPanel()
        {
            if (uiPanel.Count <= 0) return;
            GameObject temp = uiPanel.Pop();
            temp.GetComponent<BasePanel>().Reset(Vector3.zero, 0.3f);
        }
        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="go"></param>
        public void CreateUIPanel(GameObject go)
        {
            GameObject temp = Instantiate(go, transform);
            temp.name = go.name;
            temp.GetComponent<BasePanel>().Init();
            if (temp.name != "LoginPanel")
                temp.GetComponent<BasePanel>().Reset(Vector3.zero, 0);
            else
                uiPanel.Push(temp);
            if (temp.name.Equals("MessageTipPanel"))
                messagePanel = temp.GetComponent<BasePanel>() as MessagePanel;
            if (!allUIPanel.Contains(temp))
                allUIPanel.Add(temp);
        }
        /// <summary>
        /// 打开等待面板
        /// </summary>
        public void OpenLoadingPanel(Vector3 vector)
        {
            loadingPanel.Reset(vector, 0);
        }
    }
}
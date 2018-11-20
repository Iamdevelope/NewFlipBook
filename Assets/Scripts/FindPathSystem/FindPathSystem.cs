using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class FindPathSystem {
    private string path;
    public string fileName;
    public Dictionary<string, List<string>> pathMap = new Dictionary<string, List<string>>();

    public IEnumerator Init(string name)
    {
        path = Application.streamingAssetsPath +"/"+ name;
        WWW www = new WWW(path);
        yield return www;
        string content = www.text;
        ReadContent(content);
    }

    private void ReadContent(string content)
    {
        if (content.Contains("|"))
        {
            string[] arr = content.Split('|');
            for (int i = 0; i < arr.Length; i++)
            {
                string[] temp = arr[i].Split(':');
                if (!pathMap.ContainsKey(temp[0]))
                    pathMap[temp[0]] = new List<string>();
                SplitArray(temp[1],pathMap[temp[0]]);
            }
        }
    }

    private void SplitArray(string str,List<string> list)
    {
        if (str.Contains(","))
        {
            string[] arr = str.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                list.Add(arr[i]);
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using System;
using System.IO;

public class ChangeName
{
    [MenuItem("Tools/规范化命名")]
    public static void fun()
    {
        string path = EditorUtility.OpenFolderPanel("打开目标文件夹", "Assets", null);
        string datapath = path.Substring(path.IndexOf("Assets"), path.Length - path.IndexOf("Assets"));
        List<FileInfo> listFileName = new List<FileInfo>();
        GetFileName(path, ref listFileName);
        Debug.Log(listFileName.Count);
        for (int i = 0; i < listFileName.Count; i++)
        {

            string name = listFileName[i].FullName.Substring(listFileName[i].FullName.IndexOf("Assets"), listFileName[i].FullName.Length - listFileName[i].FullName.IndexOf("Assets")).Replace('\\', '/');

            Debug.Log(name);
            string nameNew = listFileName[i].Name.Replace(listFileName[i].Extension, "").Replace('（', '(').Replace('）', ')');
            Debug.Log(nameNew);
            Debug.Log(AssetDatabase.RenameAsset(name, nameNew));
        }
        AssetDatabase.Refresh();
    }
    public static void GetFileName(string path, ref List<FileInfo> filename)
    {
        string[] directory = Directory.GetDirectories(path);
        if (directory.Length > 0)
        {
            for (int i = 0; i < directory.Length; i++)
            {

                GetFileName(directory[i], ref filename);
            }
        }
        string[] files = Directory.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {

            if ((files[i].Contains("（") || files[i].Contains("）")) && !files[i].EndsWith(".meta"))
            {
                filename.Add(new FileInfo(files[i]));
            }
        }

    }
}
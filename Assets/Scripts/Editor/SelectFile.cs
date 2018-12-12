using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SelectFile : Editor
{
    private static Dictionary<string, List<string>> filePathAndName = new Dictionary<string, List<string>>();
    [MenuItem("Tools/获取文件路径")]
    static void GetAllSelectFile()
    {
        Object[] selectObj = Selection.GetFiltered(typeof(Object), SelectionMode.Unfiltered);
        foreach (Object item in selectObj)
        {
            string objPath = AssetDatabase.GetAssetPath(item);
            DirectoryInfo directory = new DirectoryInfo(objPath);
            if (directory.GetFiles().Length <= 1)
            {
                Debug.LogError("--------请检查是否选中了非文件夹对象--------");
                return;
            }
            SetAssetBundleName(directory);
        }
        GenerateJsonFile();
        AssetDatabase.Refresh();
    }

    private static void GenerateJsonFile()
    {
        string str = JsonMapper.ToJson(filePathAndName);
        string path = Application.streamingAssetsPath + "/filePathAndName.json";
        if (File.Exists(path))
            File.Delete(path);
        StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.Write(str);
        writer.Close();
    }

    static void SetAssetBundleName(DirectoryInfo dirInfo)
    {
        FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
        foreach (FileSystemInfo file in files)
        {
            if (file is FileInfo && file.Extension != ".meta" && file.Extension != ".txt")
            {
                
                string temp = dirInfo.FullName.Replace('\\', '/');
                temp = temp.Replace(Application.streamingAssetsPath, "");

                if (!filePathAndName.ContainsKey(temp))
                    filePathAndName[temp] = new List<string>();
                string filePath = file.Name;
                filePathAndName[temp].Add(filePath);
            }
            else if (file is DirectoryInfo)
            {
                string filePath = file.FullName.Replace('\\', '/');
                filePath = filePath.Replace(Application.streamingAssetsPath, "");

                SetAssetBundleName(file as DirectoryInfo);
            }
        }
    }
}
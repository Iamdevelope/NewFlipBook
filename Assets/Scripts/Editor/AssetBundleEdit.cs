using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleEdit : Editor
{
    [MenuItem("Tools/Load AssetBundle")]
    static void LoadAssetBundleAsset()
    {
        BuildPipeline.BuildAssetBundles(Application.persistentDataPath + "/AssetBundle/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/设置固定名")]
    public static void saveAsStaticName()
    {
        string Path = "Prefabs";
        string abName = "building.ab";
        SetVersionDirAssetName(Path, abName);//第一个参数是路径 第二个参数是Ab名字 默认前缀为 Application.dataPath + "/"＋ Path  
    }

    [MenuItem("Tools/设定文件名")]
    public static void saveAsPrefabName()
    {
        string Path = "Prefabs";
        SetAssetNameAsPrefabName(Path);//第一个参数是路径   
    }

    public static void SetVersionDirAssetName(string fullPath, string abName)
    {
        var relativeLen = fullPath.Length + 8; // Assets 长度  
        fullPath = Application.dataPath + "/" + fullPath + "/";

        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);
            var dir = new DirectoryInfo(fullPath);
            var files = dir.GetFiles("*", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; ++i)
            {
                var fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))
                {
                    var basePath = fileInfo.FullName.Substring(fullPath.Length - relativeLen);//.Replace('\\', '/');  
                    var importer = AssetImporter.GetAtPath(basePath);
                    if (importer && importer.assetBundleName != abName)
                    {
                        importer.assetBundleName = abName;
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
    }

    public static void SetAssetNameAsPrefabName(string fullPath)
    {
        var relativeLen = fullPath.Length + 8; // Assets 长度  
        fullPath = Application.dataPath + "/" + fullPath + "/";

        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);
            var dir = new DirectoryInfo(fullPath);
            var files = dir.GetFiles("*", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; ++i)
            {
                var fileInfo = files[i];
                string abName = fileInfo.Name;
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))
                {
                    var basePath = fileInfo.FullName.Substring(fullPath.Length - relativeLen);//.Replace('\\', '/');  
                    var importer = AssetImporter.GetAtPath(basePath);
                    //abName = AssetDatabase.AssetPathToGUID(basePath);
                    if (importer && importer.assetBundleName != abName)
                    {
                        importer.assetBundleName = abName;
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
    }

    /// <summary>  
    /// AssetBundleManifestName == 对应AB依赖列表文件  
    /// </summary>  

    private static string AssetBundle_BuildDirectory_Path = @Application.streamingAssetsPath + "/../../../" + "AssetBundles";
    private static string AssetBundle_TargetDirectory_Path = @Application.streamingAssetsPath + "/" + "ABFiles";
    [MenuItem("Tools/Asset Bundle/Build Asset Bundles", false, 0)]
    public static void BuildAssetBundleAndroid()
    {
        //Application.streamingAssetsPath对应的StreamingAssets的子目录  
        DirectoryInfo AB_Directory = new DirectoryInfo(AssetBundle_BuildDirectory_Path);
        if (!AB_Directory.Exists)
        {
            AB_Directory.Create();
        }
        FileInfo[] filesAB = AB_Directory.GetFiles();
        foreach (var item in filesAB)
        {
            Debug.Log("******删除旧文件：" + item.FullName + "******");
            item.Delete();
        }
#if UNITY_ANDROID
        BuildPipeline.BuildAssetBundles(AB_Directory.FullName, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);  
#elif UNITY_IPHONE
        BuildPipeline.BuildAssetBundles(AB_Directory.FullName, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS); 
#else
        BuildPipeline.BuildAssetBundles(AB_Directory.FullName, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
#endif
        Debug.Log("******AssetBundle打包完成******");

        Debug.Log("将要转移的文件夹是：" + AssetBundle_TargetDirectory_Path);
        FileInfo[] filesAB_temp = AB_Directory.GetFiles();

        DirectoryInfo streaming_Directory = new DirectoryInfo(AssetBundle_TargetDirectory_Path);

        FileInfo[] streaming_files = streaming_Directory.GetFiles();
        foreach (var item in streaming_files)
        {
            item.Delete();
        }
        AssetDatabase.Refresh();
        foreach (var item in filesAB_temp)
        {
            if (item.Extension == "")
            {
                item.CopyTo(AssetBundle_TargetDirectory_Path + "/" + item.Name, true);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("******文件传输完成******");
    }

    private static string _dirName = "";
    /// <summary>  
    /// 批量命名所选文件夹下资源的AssetBundleName.  
    /// </summary>  
    [MenuItem("Tools/Asset Bundle/Set Asset Bundle Name")]
    static void SetSelectFolderFileBundleName()
    {
        UnityEngine.Object[] selObj = Selection.GetFiltered(typeof(Object), SelectionMode.Unfiltered);
        foreach (Object item in selObj)
        {
            string objPath = AssetDatabase.GetAssetPath(item);
            DirectoryInfo dirInfo = new DirectoryInfo(objPath);
            if (dirInfo.GetFiles().Length == 0)
            {
                Debug.LogError("******请检查，是否选中了非文件夹对象******");
                return;
            }
            _dirName = dirInfo.Name;

            string filePath = dirInfo.FullName.Replace('\\', '/');
            filePath = filePath.Replace(Application.dataPath, "Assets");
            AssetImporter ai = AssetImporter.GetAtPath(filePath);
            //给Asset资源添加名字
            ai.assetBundleName = _dirName;
            //给Asset资源添加后缀
            ai.assetBundleVariant = _dirName;
            SetAssetBundleName(dirInfo);
        }
        AssetDatabase.Refresh();
        Debug.Log("******批量设置AssetBundle名称成功******");
    }
    static void SetAssetBundleName(DirectoryInfo dirInfo)
    {
        FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
        foreach (FileSystemInfo file in files)
        {
            if (file is FileInfo && file.Extension != ".meta" && file.Extension != ".txt")
            {
                string filePath = file.FullName.Replace('\\', '/');
                filePath = filePath.Replace(Application.dataPath, "Assets");
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = _dirName;
                ai.assetBundleVariant = _dirName;
            }
            else if (file is DirectoryInfo)
            {
                string filePath = file.FullName.Replace('\\', '/');
                filePath = filePath.Replace(Application.dataPath, "Assets");
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = _dirName;
                ai.assetBundleVariant = _dirName;
                SetAssetBundleName(file as DirectoryInfo);
            }
        }
    }
    /// <summary>  
    /// 批量清空所选文件夹下资源的AssetBundleName.  
    /// </summary>  
    [MenuItem("Tools/Asset Bundle/Reset Asset Bundle Name")]
    static void ResetSelectFolderFileBundleName()
    {
        UnityEngine.Object[] selObj = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Unfiltered);
        foreach (UnityEngine.Object item in selObj)
        {
            string objPath = AssetDatabase.GetAssetPath(item);
            DirectoryInfo dirInfo = new DirectoryInfo(objPath);
            if (dirInfo == null)
            {
                Debug.LogError("******请检查，是否选中了非文件夹对象******");
                return;
            }
            _dirName = null;

            string filePath = dirInfo.FullName.Replace('\\', '/');
            filePath = filePath.Replace(Application.dataPath, "Assets");
            AssetImporter ai = AssetImporter.GetAtPath(filePath);
            ai.assetBundleName = _dirName;

            SetAssetBundleName(dirInfo);
        }
        AssetDatabase.Refresh();
        Debug.Log("******批量清除AssetBundle名称成功******");
    }
}
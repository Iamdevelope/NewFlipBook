using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class SettingPrefabPicture : Editor
{
    [MenuItem("Tools/获取预制体文件对应的贴图")]
    public static void GetPrefabPath()
    {
        List<string> prefabPath = new List<string>();
        string path = EditorUtility.OpenFolderPanel("获取预设体文件夹", null, null);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        DirectoryInfo dir = new DirectoryInfo(path);
        CreatResInfo(dir, ref prefabPath);
        List<GameObject> golist = new List<GameObject>();
        //获取所有的材质
        List<Material> prafabMatrial = new List<Material>();
        for (int i = 0; i < prefabPath.Count; i++)
        {
            if (prefabPath[i].EndsWith(".prefab"))
            {
                //Debug.Log(prefabPath[i]);
                GameObject go = AssetDatabase.LoadMainAssetAtPath(prefabPath[i]) as GameObject;
                List<MeshRenderer> renderlist = new List<MeshRenderer>();
                GetObjRender(go.transform, ref renderlist);

                for (int j = 0, count = renderlist.Count; j < count; j++)
                {
                    if (renderlist[j].sharedMaterials.Length > 0)
                    {
                        prafabMatrial.AddRange(renderlist[j].sharedMaterials);
                    }
                }
            }
        }
        //获取贴图路径
        List<string> picPath = new List<string>();
        for (int i = 0; i < prafabMatrial.Count; i++)
        {
            //获取主贴图
            if (prafabMatrial[i].mainTexture != null)
            {
                string assetpath = AssetDatabase.GetAssetPath(prafabMatrial[i].mainTexture);
                if (assetpath.EndsWith(".dds") || assetpath.EndsWith(".DDS"))
                {
                    if (!picPath.Contains(assetpath))
                    {
                        picPath.Add(assetpath);
                    }
                }
            }
        }

        string newpath = "Assets/OldTexture";
        DirectoryInfo dirinfo = new DirectoryInfo(Application.dataPath + "/OldTexture");
        if (!dirinfo.Exists)
        {
            AssetDatabase.CreateFolder("Assets", "OldTexture");
            AssetDatabase.Refresh();
        }
        for (int i = 0, count = picPath.Count; i < count; i++)
        {
            string oldPath = picPath[i];
            string newPath = newpath + oldPath.Substring(oldPath.LastIndexOf('/'), oldPath.Length - oldPath.LastIndexOf('/'));
            Debug.Log(newPath);
            if (oldPath != newPath)
            {
                AssetDatabase.MoveAsset(oldPath, newPath);
                AssetDatabase.Refresh();
            }
        }
        EditorUtility.DisplayDialog("提示", "贴图移动完成!", "OK");

    }


    [MenuItem("Tools/设置预制体文件对应的贴图")]
    public static void SetPrefabPath()
    {
        List<string> prefabPath = new List<string>();
        string path = EditorUtility.OpenFolderPanel("获取预设体文件夹", null, null);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        DirectoryInfo dir = new DirectoryInfo(path);
        CreatResInfo(dir, ref prefabPath);
        List<GameObject> golist = new List<GameObject>();
        //获取所有的材质
        List<Material> prafabMatrial = new List<Material>();
        for (int i = 0; i < prefabPath.Count; i++)
        {
            if (prefabPath[i].EndsWith(".prefab"))
            {
                //Debug.Log(prefabPath[i]);
                GameObject go = AssetDatabase.LoadMainAssetAtPath(prefabPath[i]) as GameObject;
                List<MeshRenderer> renderlist = new List<MeshRenderer>();
                GetObjRender(go.transform, ref renderlist);

                for (int j = 0, count = renderlist.Count; j < count; j++)
                {
                    if (renderlist[j].sharedMaterials.Length > 0)
                    {
                        prafabMatrial.AddRange(renderlist[j].sharedMaterials);
                    }
                }
            }
        }
        string newpath = EditorUtility.OpenFolderPanel("获取贴图新路径文件夹", null, null);
        if (string.IsNullOrEmpty(newpath))
        {
            return;
        }
        List<string> xintietuList = new List<string>();
        DirectoryInfo newDirinfo = new DirectoryInfo(newpath);
        CreatResInfo(newDirinfo, ref xintietuList);
        Dictionary<string, Texture> mpicDic = new Dictionary<string, Texture>();
        for (int i = 0, count = xintietuList.Count; i < count; i++)
        {
            if (xintietuList[i].EndsWith("png") || xintietuList[i].EndsWith("PNG")
                || xintietuList[i].EndsWith("TGA") || xintietuList[i].EndsWith("tga") || xintietuList[i].EndsWith("jpg") || xintietuList[i].EndsWith("JPG"))
            {
                Texture go = AssetDatabase.LoadMainAssetAtPath(xintietuList[i]) as Texture;
                if (!mpicDic.ContainsKey(go.name))
                {
                    mpicDic.Add(go.name, go);
                    Debug.Log("资源夹中的贴图名称为" + go.name);
                }
                else
                {
                    Debug.LogError("存在相同的贴图" + go.name);
                }
            }
        }


        ///材质贴图
        for (int i = 0; i < prafabMatrial.Count; i++)
        {
            //获取主贴图
            if (prafabMatrial[i].mainTexture != null)
            {
                string name = prafabMatrial[i].mainTexture.name;
                string assetpath = AssetDatabase.GetAssetPath(prafabMatrial[i].mainTexture);
                if (assetpath.EndsWith(".dds") || assetpath.EndsWith(".DDS"))
                {
                    if (mpicDic.ContainsKey(name))
                    {
                        prafabMatrial[i].mainTexture = mpicDic[name];
                    }
                    else
                    {
                        Debug.LogError("无法找到此贴图____贴图名为：" + name);
                    }
                }

            }
        }
        EditorUtility.DisplayDialog("提示", "设置贴图完成!", "OK");
    }

    [MenuItem("Tools/检测预制体文件是否含有dds的贴图")]
    public static void JianCeShiFuoHanyouDDS()
    {

        string path = EditorUtility.OpenFolderPanel("获取预设体文件夹", null, null);
        List<string> prefabPath = new List<string>();
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        DirectoryInfo dir = new DirectoryInfo(path);
        CreatResInfo(dir, ref prefabPath);
        for (int i = 0, count = prefabPath.Count; i < count; i++)
        {
            string[] piclist = AssetDatabase.GetDependencies(new string[1] { prefabPath[i] });
            foreach (string item in piclist)
            {
                if (item.EndsWith(".dds") || item.EndsWith(".DDS"))
                {
                    Debug.LogError("源文件预设体为" + prefabPath[i] + "       " + "贴图位置在" + item);
                }
            }
        }
        //for (int i = 0,count=piclist.Length; i < count; i++)
        //{
        //    if (piclist[i].EndsWith(".dds")||piclist[i].EndsWith(".DDS"))
        //    {
        //        Debug.LogError(string);
        //    }
        //}
        EditorUtility.DisplayDialog("提示", "检测成功!", "OK");
    }


    #region 获取目录文件下的资源文件
    /// <summary>
    /// 目录下文件的读取
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="dic">返回到字典</param>
    public static void CreatResInfo(DirectoryInfo dir, ref List<string> filepaths)
    {
        //获取此目录下的实例
        //DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            return;
        }
        //获取此目录下的实例
        FileInfo[] files = dir.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo info = files[i];
            if (!(info.Name.IndexOf(".meta", 0) > 0))
            {
                //移除至resources下的文件夹路径
                string pathdir = "Assets/" + info.FullName.Replace("\\", "/")
                    .Replace((Application.dataPath + "/"), "");
                //去除后缀名的文件名称
                string fileName = Path.GetFileNameWithoutExtension(info.Name);
                //存入字典
                filepaths.Add(pathdir);
            }
        }
        DirectoryInfo[] dirinfos = dir.GetDirectories();
        int infolength = dirinfos.Length;
        if (infolength > 0)
        {
            for (int i = 0; i < infolength; i++)
            {
                CreatResInfo(dirinfos[i], ref filepaths);
            }
        }
    }



    public static void GetObjRender(Transform go, ref List<MeshRenderer> meshrenderlist)
    {
        MeshRenderer rend = go.GetComponent<MeshRenderer>();
        if (rend != null)
        {
            meshrenderlist.Add(rend);
        }
        int childnum = go.transform.childCount;
        if (childnum <= 0)
        {
            return;
        }
        for (int i = 0; i < childnum; i++)
        {
            GetObjRender(go.GetChild(i), ref meshrenderlist);
        }

    }
    #endregion

    [MenuItem("Tools/收集所有资源到相应文件夹")]
    public static void CollectResources()
    {
        CSV.CsvStreamReader reader = new CSV.CsvStreamReader("Assets/Scenes/Config/TotalDynamic.csv");
        reader.LoadCsvFile();
        List<string> textureList = new List<string>();
        List<string> materialList = new List<string>();
        List<string> fbxList = new List<string>();
        List<string> prefabList = new List<string>();
        ArrayList rowList = reader.GetRowList();
        foreach (ArrayList i in rowList)
        {
            foreach (string j in i)
            {
                if (!string.IsNullOrEmpty(j))
                {
                    prefabList.Add(j);
                }
            }
        }
        //打包动态物体依赖
        for (int i = 0; i < prefabList.Count; ++i)
        {
            string path = prefabList[i];
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(fileName))
            {
                continue;
            }
            string[] tmpdependencies = AssetDatabase.GetDependencies(new string[] { path });
            foreach (string tmpdependencie in tmpdependencies)
            {
                string tmp = tmpdependencie.ToLower();
                string mame = Path.GetFileNameWithoutExtension(tmp).ToLower();
                if (tmp.Contains(".tga") || tmp.Contains(".png") || tmp.Contains(".jpg") || tmp.Contains(".dds"))
                {
                    if (!textureList.Contains(tmpdependencie))
                    {
                        textureList.Add(tmpdependencie);
                    }
                }
                else if (tmp.Contains(".mat"))
                {
                    if (!materialList.Contains(tmpdependencie))
                    {
                        materialList.Add(tmpdependencie);
                    }
                }
                else if (tmp.Contains(".fbx"))
                {
                    if (!fbxList.Contains(tmpdependencie))
                    {
                        fbxList.Add(tmpdependencie);
                    }
                }
            }
        }

        //移动资源
        string newtexturepath = "Assets/Scenes/All_Texture/";
        string newmatpath = "Assets/Scenes/All_Material/";
        string newfbxpath = "Assets/Scenes/All_Fbx/";
        for (int i = 0, count = textureList.Count; i < count; i++)
        {
            string name = Path.GetFileName(textureList[i]);
            string newpath = newtexturepath + name;
            MoveAsset(textureList[i], newpath);
        }
        for (int i = 0, count = materialList.Count; i < count; i++)
        {
            string name = Path.GetFileName(materialList[i]);
            string newpath = newmatpath + name;
            MoveAsset(materialList[i], newpath);
        }
        for (int i = 0, count = fbxList.Count; i < count; i++)
        {
            string name = Path.GetFileName(fbxList[i]);
            string newpath = newfbxpath + name;
            MoveAsset(fbxList[i], newpath);
        }

        EditorUtility.DisplayDialog("提示", "操作完成!", "OK");
    }

    public static void MoveAsset(string oldpath, string newpath)
    {
        //Debug.LogError("资源" + "oldpath:  " + oldpath + "--------" + "newpath:  " + newpath);
        //string objname = Path.GetFileNameWithoutExtension(newpath);
        //string objext = Path.GetExtension(newpath);
        //string newnewpath = newpath.Remove(newpath.LastIndexOf("/")) +"/"+objname + "_biaozhi" + objext;
        //Debug.LogError("资源" + "oldpath:  " + oldpath + "--------" + "newpath:  " + newnewpath);
        if (string.IsNullOrEmpty(oldpath) || string.IsNullOrEmpty(newpath))
        {
            return;
        }
        if (oldpath == newpath)
        {
            return;
        }
        Object oldobj = AssetDatabase.LoadMainAssetAtPath(oldpath);
        Object newobj = AssetDatabase.LoadMainAssetAtPath(newpath);
        if (newobj == null)
        {
            AssetDatabase.MoveAsset(oldpath, newpath);
            AssetDatabase.Refresh();
            return;
        }

        if (oldobj.Equals(newobj))
        {
            Debug.LogError("存在相同资源" + "oldpath:  " + oldpath + "--------" + "newpath:  " + newpath);
        }
        else
        {
            string objname = Path.GetFileNameWithoutExtension(newpath);
            string objext = Path.GetExtension(newpath);
            string newnewpath = newpath.Remove(newpath.LastIndexOf("/")) + "/" + objname + "_biaozhi" + objext;
            AssetDatabase.MoveAsset(oldpath, newnewpath);
            AssetDatabase.Refresh();
        }

    }


}
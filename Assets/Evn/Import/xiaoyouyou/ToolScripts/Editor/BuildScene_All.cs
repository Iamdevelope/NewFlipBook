using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class BuildScene_All
{
    public static void CreateDependencieAssetBundle(string path, string Extension, BuildAssetBundleOptions options, BuildTarget target)
    {
        UnityEngine.Object dependencie = AssetDatabase.LoadMainAssetAtPath(path);

        string dependenciePath = "";
        string fileName = Path.GetFileNameWithoutExtension(path);
        if (path.Contains(".tga") || path.Contains(".png") || path.Contains(".jpg") || path.Contains(".dds"))
        {
            dependenciePath = "assetbundles/assets/scenes/texture/" + fileName + Extension;
        }
        else if (path.Contains(".shader"))
        {
            dependenciePath = "assetbundles/assets/scenes/shader/" + fileName + Extension;
        }
        else if (path.Contains(".mat"))
        {
            dependenciePath = "assetbundles/assets/scenes/fbx/materials/" + fileName + Extension;
        }
        else if (path.Contains(".fbx"))
        {
            dependenciePath = "assetbundles/assets/scenes/fbx/" + fileName + Extension;
        }
        else
        {
            dependenciePath = Common.GetWindowPath(path, Extension);
        }

        Common.CreatePath(dependenciePath);
        if (BuildPipeline.BuildAssetBundle((UnityEngine.Object)dependencie, null, dependenciePath, options, target))
        {
            byte[] bytes = AssetsEncrypt.ReadFileToByte(dependenciePath);
            AssetsEncrypt.EncryptBytes(bytes);
            AssetsEncrypt.WriteByteToFile(bytes, dependenciePath);
        }
    }

    //public static void CreateConfigAssetBundle(string Extension, BuildTarget target)
    //{
    //    string[] ds = Directory.GetDirectories("Assets/Scenes/Config");
    //    foreach (string d in ds)
    //    {
    //        string[] files = Directory.GetFiles(d);
    //        foreach (string filePath in files)
    //        {
    //            if (Path.GetExtension(filePath) == ".csv")
    //            {
    //                CSV.CsvStreamReader csv = new CSV.CsvStreamReader(filePath);
    //                if (!csv.LoadCsvFile()) continue;
    //                string FileName = Path.GetFileNameWithoutExtension(filePath);


    //                FileStreamHolder holder = ScriptableObject.CreateInstance<FileStreamHolder>();
    //                holder.Init(csv.GetRowList());

    //                string time = Common.CurrTimeString;
    //                string p = "Assets" + Path.DirectorySeparatorChar + FileName + "_" + time + ".asset";
    //                AssetDatabase.CreateAsset(holder, p);
    //                UnityEngine.Object tmpObject = AssetDatabase.LoadAssetAtPath(p, typeof(FileStreamHolder));

    //                string dest = Common.GetWindowPath(filePath, Extension);
    //                Common.CreatePath(dest);

    //                tmpObject.name = FileName;
    //                if (BuildPipeline.BuildAssetBundle(tmpObject, null, dest, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target))
    //                {
    //                    byte[] bytes = AssetsEncrypt.ReadFileToByte(dest);
    //                    AssetsEncrypt.EncryptBytes(bytes);
    //                    AssetsEncrypt.WriteByteToFile(bytes, dest);
    //                }

    //                AssetDatabase.DeleteAsset(p);
    //            }
    //        }
    //    }

        
    //}

    public static void MassSetTextureImporter(string path,BuildTarget target)
    {
        TextureImporterType TextureType = TextureImporterType.Default;
        TextureImporterFormat tif;

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer==null)
        {
            return;
        }
        importer.mipmapEnabled = true;
        importer.npotScale = TextureImporterNPOTScale.None;
        importer.textureType = TextureType;
        importer.isReadable = false;

        if (importer.grayscaleToAlpha)
        {
            tif = TextureImporterFormat.Alpha8;
        }
        else if (importer.DoesSourceTextureHaveAlpha())
        {
            tif = TextureImporterFormat.RGBA32;
        }
        else
        {

            switch (target)
            {
                case BuildTarget.Android:
                    {
                        tif = TextureImporterFormat.ETC_RGB4;
                    }
                    break;
                case BuildTarget.iOS:
                    {
                        tif = TextureImporterFormat.PVRTC_RGB4;
                    }
                    break;
                default:
                    {
                        tif = TextureImporterFormat.RGB24;
                    }
                    break;
            }
        }

        importer.textureFormat = tif;


        importer.maxTextureSize = 512;

        AssetDatabase.ImportAsset(path);
    }
}

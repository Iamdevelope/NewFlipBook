
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

//将prefab资源打包

#if UNITY_EDITOR
public class BuildAsssetBundleForSelection 
{
    static void Exec(string Extension, BuildTarget target)
    {
        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(tmp);
            if (path.Contains(".prefab"))
            {

                MassSetTextureImporter.ChangeTextureFormat(tmp);

                //打包window版本
                string dstPath = Common.GetWindowPath(path, Extension);
                Debug.Log("dstPath = " + dstPath);

                Common.CreatePath(dstPath);

                if (BuildPipeline.BuildAssetBundle((UnityEngine.Object)tmp, null, dstPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target))
                {
                    byte[] bytes = AssetsEncrypt.ReadFileToByte(dstPath);
                    AssetsEncrypt.EncryptBytes(bytes);
                    AssetsEncrypt.WriteByteToFile(bytes, dstPath);
                }

            }
            else
            {
                Debug.Log("please make sure you selected the prefab");
            }
        }
    }

    //[MenuItem("[Build Windows]/Build Prefab For  [Windows]")]
    //static void ExecuteForWindows()
    //{
    //    Exec(".x", BuildTarget.WebPlayer);
 
    //}

    //[MenuItem("[Build IOS]/Build Prefab For  [IOS]")]
    //static void ExecuteForIOS()
    //{
    //    Exec(".x.iphone", BuildTarget.iPhone);

    //}
    [MenuItem("[Build Android]/Build Prefab For  [Android]")]
    static void ExecuteForAndroid()
    {
        Exec(".x.android", BuildTarget.Android);

    }

}

#endif
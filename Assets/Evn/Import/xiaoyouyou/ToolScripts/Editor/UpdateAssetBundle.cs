using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class UpdateAssetBundle
{
    static void Execute(string Extension, BuildTarget target)
    {
        BuildAssetBundleOptions options =
           BuildAssetBundleOptions.CollectDependencies |
           BuildAssetBundleOptions.CompleteAssets |
           BuildAssetBundleOptions.DeterministicAssetBundle;

        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(tmp);
            path = path.ToLower();

            string e = "";
            if( path.Contains(".tga") || path.Contains(".png"))
            {
                e = ".tex";
            }
            else if(path.Contains(".mat"))
            {
                e = ".mat";
            }
            else if(path.Contains(".fbx"))
            {
                e = ".fbx";
            }
            else
            {
                continue;
            }

            string dest = Common.GetWindowPath(path, e + Extension);
            Common.CreatePath(dest);

            BuildPipeline.PushAssetDependencies();
            if (BuildPipeline.BuildAssetBundle(tmp, null, dest, options, target))
            {
                byte[] bytes = AssetsEncrypt.ReadFileToByte(dest);
                AssetsEncrypt.EncryptBytes(bytes);
                AssetsEncrypt.WriteByteToFile(bytes, dest);
            }

            BuildPipeline.PopAssetDependencies();
        }

       

        
    }

    //[MenuItem("[Build Windows]/UpdateAssetBundle for [Windows]")]
    //public static void UpdateAssetBundleForWindows()
    //{
    //    Execute("", BuildTarget.WebPlayer);
    //}



    //[MenuItem("[Build IOS]/UpdateAssetBundle for [IOS]")]
    //public static void UpdateAssetBundleForIOS()
    //{
    //    Execute(".iphone", BuildTarget.iPhone);
    //}

    [MenuItem("[Build Android]/UpdateAssetBundle for [Android]")]
    public static void UpdateAssetBundleForAndroid()
    {
        Execute(".android", BuildTarget.Android);
    }
}

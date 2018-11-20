using UnityEngine;
using UnityEditor;
using System.Collections;

public class FindTextureDepen
{
    [MenuItem("Scripts/FindTextureWhereUse")]
    public static void Execute()
    {
        string[] assets = AssetDatabase.GetAllAssetPaths();

        Object selectObj = Selection.activeObject;

        foreach (string asset in assets)
        {
            if (asset.Contains(".mat"))
            {
                Material m = (Material)AssetDatabase.LoadMainAssetAtPath(asset);
                if (m.mainTexture == selectObj)
                {
                    Debug.Log("path:" + asset);
                }
            }
        }
    }
}

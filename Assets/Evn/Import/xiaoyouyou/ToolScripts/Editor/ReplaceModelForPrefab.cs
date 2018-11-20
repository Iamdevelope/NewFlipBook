using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ReplaceModelForPrefab 
{
    [MenuItem("Scripts/ReplaceModelForPrefab")]
    static void Execute()
    {
        List<string> dynamicChildren = new List<string>();
        dynamicChildren.Add("UsableEffect");
        dynamicChildren.Add("AdornEffect");
        dynamicChildren.Add("Tree");
        dynamicChildren.Add("Grass");
        dynamicChildren.Add("Building");
        dynamicChildren.Add("Other");
        dynamicChildren.Add("GUT");

        List<GameObject> deleteList = new List<GameObject>();

        string currScene = EditorApplication.currentScene;
        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(tmp);
            if (path.Contains(".unity"))
            {
                EditorApplication.OpenScene(path);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                GameObject o = GameObject.Find(fileName);

                foreach (string child in dynamicChildren)
                {
                    Transform t = o.transform.Find("DynamicObject/" + child);
                    if (null == t)
                    {
                        continue;
                    }
                    deleteList.Clear();

                    int count = t.childCount;
                    for (int i = 0; i < count; ++i)
                    {
                        Transform model = t.GetChild(i);
                        deleteList.Add(model.gameObject);
                    }

                    foreach (GameObject deleteObj in deleteList)
                    {

                        string prefabPath = "Assets/Scenes/Prefab/" + child + "/" + deleteObj.name + ".prefab";
                        Object prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);
                        if (null == prefab)
                        {
                            Debug.LogError("Can't find prefab:" + prefabPath);
                        }
                        else
                        {
                            GameObject replaceObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                            replaceObj.name = deleteObj.name;
                            replaceObj.transform.parent = t;
                            replaceObj.transform.localPosition = deleteObj.transform.localPosition;
                            replaceObj.transform.localRotation = deleteObj.transform.localRotation;
                            replaceObj.transform.localScale = deleteObj.transform.localScale;

                            UnityEngine.Object.DestroyImmediate(deleteObj);

                        }
                       
                        
                        
                    }
                }

                EditorApplication.SaveScene(path);
            }
        }

        EditorApplication.OpenScene(currScene);

        EditorUtility.DisplayDialog("提示", "成功,恭喜你!", "OK");
    }
	
}

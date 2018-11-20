using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class MassRecordDynamicInfo
{

    [MenuItem("Scripts/MassRecordDynamicInfo")]
    static void Execute()
    {
        EditorApplication.SaveScene();
       

        string currScene = EditorApplication.currentScene;

        Common.ClearFiles("Assets/Scenes/Config/SceneConfig");
        Common.ClearFiles("Assets/Scenes/Scene");
        Common.ClearFiles("Assets/Scenes/Scene_bak");
        CSV.CsvStreamWriter writer = new CSV.CsvStreamWriter("Assets/Scenes/Config/TotalDynamic.csv");
        int row = 1;
        int col = 1;
        List<string> dynamicList = new List<string>();

        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(tmp);
            if (path.Contains(".unity"))
            {
                EditorApplication.OpenScene(path);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                GameObject o = GameObject.Find(fileName);
                Selection.activeGameObject = o;
                if (o != null)
                {
                    if (!RecordDynamicInfo.Execute(dynamicList,true))
                    {
                        break;
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("错误", "批量打包:场景名称和根场景物体应该完全一致!", "OK");
                    return;
                }
            }
        }

        foreach (string path in dynamicList)
        {
            writer[row, col] = path;
            row++;
        }

        writer.Save();

        EditorApplication.OpenScene(currScene);

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("提示", "成功,恭喜你!", "OK");
    }
}

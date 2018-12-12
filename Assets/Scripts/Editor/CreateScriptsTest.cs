using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateScriptsTest : Editor {
    [MenuItem("Tools/skill")]
    public static void Create()
    {
        ScriptTableTest temp = ScriptableObject.CreateInstance<ScriptTableTest>();
        AssetDatabase.CreateAsset(temp, "Assets/Sprites/skill.asset");
        Selection.activeObject = temp;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class ChangeNameHierarchy
{

    private Dictionary<string, object> mdic = new Dictionary<string, object>();
    [MenuItem("Tools/改名(去空格和双引号)")]
    private static void ChangeName()
    {
        Object[] go = Selection.objects;
        for (int i = 0; i < go.Length; i++)
        {
            if (go[i].name.Contains(" ") || go[i].name.Contains("（") || go[i].name.Contains("）"))
            {
                string name = go[i].name;
                name = name.Replace(" ", "").Replace("（", "(").Replace("）", ")");
                go[i].name = name;
            }
        }

    }
    [MenuItem("Tools/改名(改为dimian)")]
    private static void ChangeName2()
    {
        Object[] go = Selection.objects;
        List<Transform> trans = new List<Transform>();
        for (int i = 0; i < go.Length; i++)
        {
            GameObject go1 = go[i] as GameObject;
            ChangeNameForChild(go1.transform,ref trans);
        }
        if (trans.Count>0)
        {
            for (int i = 0; i < trans.Count; i++)
            {
                trans[i].gameObject.name = "dimian";
            }
        }
    }

    private static void ChangeNameForChild(Transform go, ref List<Transform> trans)
    {
        if (!trans.Contains(go))
        {
            trans.Add(go);
        }
        if (go.childCount > 0)
        {
            for (int i = 0; i < go.childCount; i++)
            {
                Transform child = go.GetChild(i);
                if (child.childCount > 0)
                {
                    ChangeNameForChild(child, ref trans);
                }
                else
                {
                    trans.Add(child);
                }
            }
        }
    }
}

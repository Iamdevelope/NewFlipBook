using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;


public class MechanismChangePrefab{

    static void Execte()
    {
        Transform tran = Selection.activeTransform;

        if (!tran.name.Contains("mode"))
        {
            EditorUtility.DisplayDialog("错误", "没有选中mode物体", "ok");
            return;
        }


        if (tran.childCount == 0)
        {
            EditorUtility.DisplayDialog("错误", "该物体下没有子物体", "ok");
            return;
        }

        int childCount = tran.childCount;

        GameObject[] newObjs = new GameObject[childCount];
        Transform[] oldObjects = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            newObjs[i] = new GameObject();

            oldObjects[i] = tran.GetChild(i);
        }

        for (int i = 0; i < childCount; i++)
        {
            newObjs[i].transform.parent = oldObjects[i].parent;
            newObjs[i].transform.localPosition = oldObjects[i].localPosition;
            newObjs[i].transform.localRotation = oldObjects[i].localRotation;
            newObjs[i].transform.localScale = oldObjects[i].localScale;
            newObjs[i].name = oldObjects[i].name;
            
            Object.DestroyImmediate(oldObjects[i].gameObject);
        }
    }

    //[MenuItem("Mechanism/ChangePrefab")]
    //static void ChangePrefab()
    //{
    //    Execte();
    //}

    public static void ChangePrefab(GameObject obj, string containsName)
    {
        int childCount = obj.transform.childCount;


        List<GameObject> newObjs = new List<GameObject>();
        List<Transform> oldObjects = new List<Transform>();
        Transform temp;
        for (int i = 0; i < childCount; i++)
        {
            temp = obj.transform.GetChild(i);
            if (temp.name.Contains(containsName))
            {
                newObjs.Add(new GameObject());
                oldObjects.Add(obj.transform.GetChild(i));
            }
        }

        int count = newObjs.Count;
        BoxCollider collider;
        for (int i = 0; i < count; i++)
        {
            newObjs[i].transform.parent = oldObjects[i].parent;
            newObjs[i].transform.localPosition = oldObjects[i].localPosition;
            newObjs[i].transform.localRotation = oldObjects[i].localRotation;
            newObjs[i].transform.localScale = oldObjects[i].localScale;
            newObjs[i].name = oldObjects[i].name;

            collider = oldObjects[i].GetComponent<BoxCollider>();
            if (collider != null)
            {
                BoxCollider newCollider = newObjs[i].AddComponent<BoxCollider>();
                newCollider.center = collider.center;
                newCollider.size = collider.size;
                newCollider.isTrigger = collider.isTrigger;
            }

            Object.DestroyImmediate(oldObjects[i].gameObject);
        }
    }
}

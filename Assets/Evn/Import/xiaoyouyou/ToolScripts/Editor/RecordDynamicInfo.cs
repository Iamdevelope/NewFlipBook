using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RecordDynamicInfo 
{
    [MenuItem("Scripts/RecordDynamicInfo")]
    static void Execute()
    {
        Execute(null,false);
    }

    public static bool Execute(List<string> dynamicList, bool bAuto = false)
    {
        if (!bAuto)
        {
            if (LightmapSettings.lightmaps.Length <= 0)
            {
                if (!EditorUtility.DisplayDialog("����", "���Ⱥ決����ͼ����ִ�иò���,�������Ҫ�決����ͼ������һ��", "��һ��", "ȡ��"))
                {
                    return false;
                }
            }
        }
        

        string currScene = EditorApplication.currentScene;
        string currSceneName = System.IO.Path.GetFileNameWithoutExtension(currScene);

        GameObject root = Selection.activeGameObject;
        if (null == root)
        {
            EditorUtility.DisplayDialog("����", "��ѡ�и���������!" + currScene, "OK");
            return false;
        }

        if (root.transform.parent != null)
        {
            EditorUtility.DisplayDialog("����", "��ѡ�и���������!" + currScene, "OK");
            return false;
        }

        if (root.transform.position != Vector3.zero)
        {
            EditorUtility.DisplayDialog("����", "������λ��û����0!" + currScene, "OK");
            return false;
        }

        if (root.transform.rotation.eulerAngles != Vector3.zero)
        {
            EditorUtility.DisplayDialog("����", "��������תû����0!" + currScene, "OK");
            return false;
        }

        if (root.transform.localScale != Vector3.one)
        {
            EditorUtility.DisplayDialog("����", "����������û����1!" + currScene, "OK");
            return false;
        }

        if (currSceneName != root.name)
        {
            EditorUtility.DisplayDialog("����", "�������ƺ͸���������Ӧ����ȫһ��!" + currScene, "OK");
            return false;
        }

        //�޳�����Mesh Collider
        for (int i = 0; i < root.transform.childCount; ++i)
        {
            Transform child = root.transform.GetChild(i);
            MeshCollider mc = child.GetComponent<MeshCollider>();
            if(mc != null)
                Object.DestroyImmediate(mc);
        }

            //�������еƹ�
            if (!bAuto)
            {
                Light[] lights = root.GetComponentsInChildren<Light>();
                foreach (Light l in lights)
                {
                    if (l.enabled && l.gameObject.activeSelf)
                    {
                        if (!EditorUtility.DisplayDialog("����", "��ȷ�����еƹⱻ����,��ȷʵ��Ҫ�ƹ������һ��!" + currScene, "��һ��", "ȡ��"))
                        {
                            return false;
                        }
                        break;
                    }
                }

            }

        Transform dynamicTrans = root.transform.Find("DynamicObject");
        if (null == dynamicTrans)
        {
            if (!bAuto)
            {
                if (!EditorUtility.DisplayDialog("����", "��ȷ����ҪDynamicObject,�����밴��һ��!" + currScene, "��һ��", "ȡ��"))
                {
                    return false;
                }
            }
            
        }
        else
        {

            List<string> dynamicChildren = new List<string>();
            dynamicChildren.Add("UsableEffect");
            dynamicChildren.Add("AdornEffect");
            dynamicChildren.Add("Tree");
            dynamicChildren.Add("Grass");
            dynamicChildren.Add("Building");
            dynamicChildren.Add("Other");
		    dynamicChildren.Add("GUT");
            dynamicChildren.Add("Terrain");
            dynamicChildren.Add("GuJian");

            int dynamicObjectCount = 0;
            for (int i = 0; i < dynamicTrans.childCount; ++i)
            {
                Transform dynamicChild = dynamicTrans.GetChild(i);
                if (!dynamicChildren.Contains(dynamicChild.name))
                {
                    EditorUtility.DisplayDialog("����", currScene + ":DynamicObject����������������,������Ϊ"+dynamicChild.name, "OK");
                    return false;
                }

                dynamicObjectCount += dynamicChild.childCount;
            }

            if (dynamicObjectCount <= 0)
            {
                EditorUtility.DisplayDialog("����", "�ó��������ڶ�̬����,����ִ�иýű�" + currScene, "OK");
                return false;
            }
        }

        Camera[] cameras = root.GetComponentsInChildren<Camera>();
        //ǿ������UsePlayerSettings
        foreach (Camera camera in cameras)
        {
            camera.renderingPath = RenderingPath.UsePlayerSettings;

            AudioListener al = camera.GetComponent<AudioListener>();
            if (al != null)
            {
                UnityEngine.Object.DestroyImmediate(al);
            }
        }

        Transform cameraTrans = root.transform.Find("Main Camera");
        if (null == cameraTrans)
        {
            EditorUtility.DisplayDialog("����", "��ȷ����������ڸ���������,��ȷ������ΪMain Camera!" + currScene, "OK");
            return false;
        }

        WorldRotationTween[] wrts = root.GetComponentsInChildren<WorldRotationTween>();
        foreach (WorldRotationTween wrt in wrts)
        {
            UnityEngine.Object.DestroyImmediate(wrt);
        }

        if (!bAuto)
        {
            Transform pengzhuang = root.transform.Find("pengzhuang_zong");
            if (null == pengzhuang)
            {

                if (!EditorUtility.DisplayDialog("����", "û���ҵ�����Ϊpengzhuang_zong������,����ó�������Ҫ��ײ��,������һ��" + currScene, "��һ��", "ȡ��"))
                {
                    return false;
                }

            }
            else
            {
                bool bFind = false;
                for (int i = 0, count = pengzhuang.childCount; i < count; ++i)
                {
                    Transform t = pengzhuang.GetChild(i);
                    if (t.name.Contains("dimian"))
                    {
                        bFind = true;
                        break;
                    }
                }

                if (!bFind)
                {
                    if (!EditorUtility.DisplayDialog("����", "û���ҵ���������dimian������,����ó�������Ҫ������ײ,������һ��" + currScene, "��һ��", "ȡ��"))
                    {
                        return false;
                    }
                }
            }
        }

        Transform[] children = root.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child == root.transform)
            {
                continue;
            }
            if (child.name == root.name)
            {
                EditorUtility.DisplayDialog("����", "�������ĳ�������������͸�����һ��,���޸ĸ������������!" + currScene, "OK");
                return false;
            }
        }

        
        EditorApplication.SaveScene("Assets/Scenes/Scene_bak/" + root.name + "_bak.unity");

        Common.CreatePath("Assets/Scenes/Config/SceneConfig");

        SceneConfigHolder holder = ScriptableObject.CreateInstance<SceneConfigHolder>();

        List<GameObject> deleteList = new List<GameObject>();
        if (dynamicTrans != null)
        {
            for (int i = 0; i < dynamicTrans.childCount; ++i)
            {
                Transform child = dynamicTrans.GetChild(i);

                for (int j = 0, count = child.childCount; j < count; ++j)
                {
                    Transform child2 = child.GetChild(j);
                    Record(child2.gameObject, holder,i);

                    string prefabPath = "Assets/Scenes/Prefab/" + child2.parent.name + "/" + child2.name + ".prefab";
                    if (!System.IO.File.Exists(prefabPath))
                    {
                        EditorUtility.DisplayDialog("����", currScene + "�Ҳ�����̬�����prefab,��̬��������:" + child2.parent.name + "/" + child2.name, "OK");
                        return false;
                    }

                    if (dynamicList != null && !dynamicList.Contains(prefabPath))
                    {
                        dynamicList.Add(prefabPath);
                    }
                    deleteList.Add(child2.gameObject);
                }
            }

            foreach (GameObject o in deleteList)
            {
                UnityEngine.Object.DestroyImmediate(o);
            }
            deleteList.Clear();
        }

        AssetDatabase.CreateAsset(holder, "Assets/Scenes/Config/SceneConfig/" + root.name + ".asset");

        //CSV.CsvStreamWriter writer = new CSV.CsvStreamWriter("Assets/Scenes/Config/SceneConfig/" + root.name + ".csv");
        //int row = 1;
        //int col = 1;
        //writer[row, col++] = "//ID";
        //writer[row, col++] = "����";
        //writer[row, col++] = "����������";
        //writer[row, col++] = "����";
        //writer[row, col++] = "��ת";
        //writer[row, col++] = "����";
        //writer[row, col++] = "��������";
        //writer[row, col++] = "����ƫ��";
        //row++;

        //List<GameObject> deleteList = new List<GameObject>();
        //if (dynamicTrans != null)
        //{     
        //    for (int i = 0; i < dynamicTrans.childCount; ++i)
        //    {
        //        Transform child = dynamicTrans.GetChild(i);

        //        for (int j = 0, count = child.childCount; j < count; ++j)
        //        {
        //            Transform child2 = child.GetChild(j);
        //            Record(child2.gameObject, writer, ref row);

        //            string prefabPath = "Assets/Scenes/Prefab/" + child2.parent.name + "/" + child2.name + ".prefab";
        //            if (!System.IO.File.Exists(prefabPath))
        //            {
        //                EditorUtility.DisplayDialog("����",  currScene + "�Ҳ�����̬�����prefab,��̬��������:" + child2.parent.name + "/" + child2.name, "OK");
        //                return false;
        //            }

        //            if (dynamicList != null && !dynamicList.Contains(prefabPath))
        //            {
        //                dynamicList.Add(prefabPath);
        //            }

        //            deleteList.Add(child2.gameObject);
        //        }
        //    }

        //    foreach (GameObject o in deleteList)
        //    {
        //        UnityEngine.Object.DestroyImmediate(o);
        //    }
        //    deleteList.Clear();
        //}
        //writer.Save();

        deleteList.Clear();
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (Object obj in objects)
        {
            GameObject o = obj as GameObject;
            if (!o.activeSelf)
            {
                deleteList.Add(o);
                 
            }
        }
        foreach (GameObject o in deleteList)
        {
            UnityEngine.Object.DestroyImmediate(o,true);
        }

        List<Transform> modeList = new List<Transform>();
        GameObject UDT_Zones = GameObject.Find("__UDT__/Zones");
        if (UDT_Zones != null)
        {
            Transform temp;
            int childCound = UDT_Zones.transform.childCount;
            for (int i = 0; i < childCound; i++)
            {
                temp = UDT_Zones.transform.GetChild(i);
                if (temp.name.Contains("mode_"))
                {
                    modeList.Add(temp);
                }
            }

            int count = modeList.Count;
            for (int i = 0; i < count; i++)
            {
                MechanismChangePrefab.ChangePrefab(modeList[i].gameObject, "mechanism");
                MechanismChangePrefab.ChangePrefab(modeList[i].gameObject, "guangmu");
                MechanismChangePrefab.ChangePrefab(modeList[i].gameObject, "chuansong");
            }
        }


        EditorApplication.SaveScene("Assets/Scenes/Scene/" + root.name + ".unity");


        if (!bAuto)
        {
            EditorApplication.OpenScene(currScene);

            EditorUtility.DisplayDialog("��ʾ", "�ɹ�,��ϲ��!", "OK");
        }
        

        return true;
    }

    static void Record( GameObject o , SceneConfigHolder holder,int index)
    {
        MeshRenderer[] mrs = o.GetComponentsInChildren<MeshRenderer>();

        SceneConfigElement element = new SceneConfigElement();
        element.nID = index + 1;
        element.sName = o.name.ToLower();
        element.sParentName = o.transform.parent.name.ToLower();
        element.position = o.transform.localPosition;
        element.rotation = o.transform.localRotation.eulerAngles;
        element.scale = o.transform.localScale;

        element.lightmapIndex = new List<int>();
        element.lightmapOffset = new List<Vector4>();

        foreach (MeshRenderer mr in mrs)
        {
            element.lightmapIndex.Add(mr.lightmapIndex);
            element.lightmapOffset.Add(mr.lightmapScaleOffset);
        }

        holder.Add(element);
    }

    //���ﲻ������������,�������ɹ���ͼƫ��
    //static string TransVector3ToString(Vector3 v)
    //{
    //    string s = "";
    //    s += Math.Round(v.x,2) + "~";
    //    s += Math.Round(v.y, 2) + "~";
    //    s += Math.Round(v.z, 2);
    //    return s;
    //}

    //static string TransVector4ToString(Vector4 v)
    //{
    //    string s = "";
    //    s += Math.Round(v.x, 2) + "~";
    //    s += Math.Round(v.y, 2) + "~";
    //    s += Math.Round(v.z, 2) + "~";
    //    s += Math.Round(v.w, 2);
    //    return s;
    //}

    static string TransVector3ToString(Vector3 v)
    {
        string s = "";
        s += v.x + "~";
        s += v.y + "~";
        s += v.z;
        return s;
    }

    static string TransVector4ToString(Vector4 v)
    {
        string s = "";
        s += v.x + "~";
        s += v.y + "~";
        s += v.z + "~";
        s += v.w;
        return s;
    }
}

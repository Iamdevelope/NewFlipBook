using UnityEngine;
using UnityEditor;
using System.Collections;

public class NavmeshToCollider
{
    //[MenuItem("Scripts/NavmeshToCollider")]
    static void Execute()
    {
        Vector3[] vertices;
        int[] indices;


        UnityEngine.AI.NavMesh.Triangulate(out vertices, out indices);

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;

        GameObject dimian = new GameObject("dimian");
        MeshCollider mc = dimian.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;

        string currScene = EditorApplication.currentScene;
        string currSceneName = System.IO.Path.GetFileNameWithoutExtension(currScene);

        string path = currSceneName + "/" + "pengzhuang_zong";
        GameObject pengzhuang_zong = GameObject.Find(path);
        if (pengzhuang_zong == null)
        {
            EditorUtility.DisplayDialog("错误", "场景找不到指定路径" + path, "OK");
            return;
        }

        dimian.transform.parent = pengzhuang_zong.transform;
    }
}

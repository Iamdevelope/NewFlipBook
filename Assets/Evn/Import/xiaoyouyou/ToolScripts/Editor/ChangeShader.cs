using UnityEngine;
using System.Collections;
using UnityEditor;

public class ChangeShader : EditorWindow
{
    private Shader m_oldShader;
    private Shader m_newShader;
    private GameObject m_obj;

    

    static void Execte(string oldShaderName,Shader newShader ,GameObject obj)
    {

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        int count = renderers.Length;

        if (count == 0)
        {
            EditorUtility.DisplayDialog("警告！", "选中的物体没有Renderer组建", "ok");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < renderers[i].sharedMaterials.Length; j++)
            {

                if (renderers[i].sharedMaterials[j].shader.name.Equals(oldShaderName))
                    renderers[i].sharedMaterials[j].shader = newShader;
            }
        }

        EditorUtility.DisplayDialog("提示！", "替换成功", "ok");
    }

    static void Execte(string oldShaderName, Shader newShader)
    {
        Material material;
        foreach (Object valuse in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
        {
            if (valuse is Material)
            {
                material= (Material)valuse;
                if (material.shader.name.Equals(oldShaderName))
                    material.shader = newShader;
            }
        }
        EditorUtility.DisplayDialog("提示！", "替换成功", "ok");
    }

    [MenuItem("Materials/ChangeShader")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(ChangeShader));
        window.position = new Rect(500, 500, 500, 200);
        window.Show();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    bool m_toggle;
    void OnGUI()
    {
        m_toggle = EditorGUI.Toggle(new Rect(5, 5, position.width - 6, 20), "选择文件夹", m_toggle);
        if (!m_toggle)
            m_obj = EditorGUI.ObjectField(new Rect(5, 30, position.width - 6, 20), "要替换的根物体", m_obj, typeof(GameObject), true) as GameObject;
        m_oldShader = EditorGUI.ObjectField(new Rect(5, 55, position.width - 6, 20), "被换掉的Shader", m_oldShader, typeof(Shader),true) as Shader;
        m_newShader = EditorGUI.ObjectField(new Rect(5, 80, position.width - 6, 20), "想要的Shader", m_newShader, typeof(Shader),true) as Shader;

        bool shaderBool = false, objBool = false;

        if (m_oldShader && m_newShader)
        {
            shaderBool = true;
        }

        if (!m_toggle && m_obj)
            objBool = true;

        if(!shaderBool)
            EditorGUI.LabelField(new Rect(5, 105, position.width - 6, 20), "提示:", "请选择将要被换掉的shader和想要的shader");

        if (m_oldShader && !objBool)
            EditorGUI.LabelField(new Rect(5, 130, position.width - 6, 20), "提示:", "请选择根物体");

        if (!m_toggle)
        {
            if (shaderBool && objBool)
            {
                if (GUI.Button(new Rect(25, 100, 50, 20), "替换"))
                    Execte(m_oldShader.name, m_newShader, m_obj);
            }
        }
        else
        {
            if (shaderBool)
            {
                if (GUI.Button(new Rect(25, 100, position.width - 6, 20), "替换"))
                    Execte(m_oldShader.name, m_newShader);
            }
        }
    }
}
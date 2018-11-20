using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class EditorBook : EditorWindow {

    private string bookName;
    private string[] pagePaths;
    private string selectObject;
    bool isCan;

    [MenuItem("Editor Book/Create Book")]
    static  void GUIWindow()
    {
         EditorWindow.GetWindow<EditorBook>().Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        bookName = EditorGUILayout.TextField("bookName", bookName);
        isCan = EditorGUILayout.BeginToggleGroup("nead",isCan);
        EditorGUILayout.LabelField("需要选中的对象");
        selectObject = EditorGUILayout.TextArea(selectObject);
        EditorGUILayout.EndToggleGroup();
        if(GUI.Button(new Rect(100,200,50,30),"创 建"))
        {
            CreateOneBook();
        }
    }

    private void OnProjectChange()
    {
        
    }
    private void OnSelectionChange()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            selectObject += Selection.gameObjects[i].name + "\n";
        }
        for (int i = 0; i < Selection.objects.Length; i++)
        {

        }
    }

    private void CreateOneBook()
    {
        Debug.Log("创建好了一本书了");
    }
}

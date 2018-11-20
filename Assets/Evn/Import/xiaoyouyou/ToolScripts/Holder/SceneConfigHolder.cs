using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SceneConfigElement
{
    public int nID;//ID
    public string sName;//名称
    public string sParentName;//父物体名称
    public Vector3 position;//坐标
    public Vector3 rotation;//旋转
    public Vector3 scale;//缩放
    public List<int> lightmapIndex;//光照索引
    public List<Vector4> lightmapOffset;//光照偏移
}

public class SceneConfigHolder : ScriptableObject
{
    public List<SceneConfigElement> content;

    public void Add(SceneConfigElement element)
    {
        if (content == null)
        {
            content = new List<SceneConfigElement>();
        }
        content.Add(element);
    }
}

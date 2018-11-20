using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneDependenciesHolder : ScriptableObject
{
    public int nID;//ID
    public string sName;//名称
    public List<string> textureList;//贴图列表
    public List<string> shaderList;//shader列表
    public List<string> materialList;//材质球列表
}

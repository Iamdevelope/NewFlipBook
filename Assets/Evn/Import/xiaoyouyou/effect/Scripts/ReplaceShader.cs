using UnityEngine;
using System.Collections;

public class ReplaceShader
{
    private float _Cutoff = 0.5f;
    private float _offset;

    private GameObject m_obj;

    private Shader _originShader;//原先的
    private Shader _replaceShader;//被替换的

	public ReplaceShader(GameObject obj,float offset)
    {
        _offset = offset;
        m_obj = obj;

        SkinnedMeshRenderer[] smrs = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            foreach (Material material in smr.materials)
            {
                material.SetFloat("_Cutoff", _Cutoff);
            }

        }

        _originShader = Shader.Find("Transparent/Cutout/Cross");
        _replaceShader = Shader.Find("Transparent/Cutout/Cross_Alpha");
    }

    public void UpdateShader(Color color)
    {
        SkinnedMeshRenderer[] smrs = m_obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            foreach (Material material in smr.materials)
            {
                material.color = color;

                if (color.a >= 0.8f)
                {
                    material.shader = _originShader;
                }
                else
                {
                    material.shader = _replaceShader;
                }

                //当_cutoff大于等于alpha时,该shader会导致模型看不见,从而导致闪烁现象
                if (color.a < _Cutoff + _offset && color.a > 0)
                {
                    material.SetFloat("_Cutoff", color.a - _offset);
                }
                else
                {
                    material.SetFloat("_Cutoff", _Cutoff);
                }

            }

            if (color.a < _Cutoff)
            {
                smr.receiveShadows = false;
                smr.castShadows = false;
            }
            else
            {
                smr.receiveShadows = true;
                smr.castShadows = true;
            }
        }

        MeshRenderer[] mrs = m_obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            foreach (Material material in mr.materials)
            {
                material.color = color;

                if (color.a >= 0.8f)
                {
                    material.shader = _originShader;
                }
                else
                {
                    material.shader = _replaceShader;
                }
                //当_cutoff大于等于alpha时,该shader会导致模型看不见,从而导致闪烁现象
                if (color.a <= _Cutoff + _offset && color.a > 0)
                {
                    material.SetFloat("_Cutoff", color.a - _offset);
                }
                else
                {
                    material.SetFloat("_Cutoff", _Cutoff);
                }
            }

            if (color.a < _Cutoff)
            {
                mr.receiveShadows = false;
                mr.castShadows = false;
            }
            else
            {
                mr.receiveShadows = true;
                mr.castShadows = true;
            }
        }
    }


    public void UpdateShader(float alpha)
    {
        SkinnedMeshRenderer[] smrs = m_obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            foreach (Material material in smr.materials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

                if (alpha >= 0.8f)
                {
                    material.shader = _originShader;
                }
                else
                {
                    material.shader = _replaceShader;
                }

                //当_cutoff大于等于alpha时,该shader会导致模型看不见,从而导致闪烁现象
                if (alpha < _Cutoff + _offset && alpha > 0)
                {
                    material.SetFloat("_Cutoff", alpha - _offset);
                }
                else
                {
                    material.SetFloat("_Cutoff", _Cutoff);
                }

            }

            if (alpha < _Cutoff)
            {
                smr.receiveShadows = false;
                smr.castShadows = false;
            }
            else
            {
                smr.receiveShadows = true;
                smr.castShadows = true;
            }
        }

        MeshRenderer[] mrs = m_obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            foreach (Material material in mr.materials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

                if (alpha >= 0.8f)
                {
                    material.shader = _originShader;
                }
                else
                {
                    material.shader = _replaceShader;
                }
                //当_cutoff大于等于alpha时,该shader会导致模型看不见,从而导致闪烁现象
                if (alpha <= _Cutoff + _offset && alpha > 0)
                {
                    material.SetFloat("_Cutoff", alpha - _offset);
                }
                else
                {
                    material.SetFloat("_Cutoff", _Cutoff);
                }
            }

            if (alpha < _Cutoff)
            {
                mr.receiveShadows = false;
                mr.castShadows = false;
            }
            else
            {
                mr.receiveShadows = true;
                mr.castShadows = true;
            }
        }
    }

    public void UpdateShader(float alpha,int nLayer)
    {
        SkinnedMeshRenderer[] smrs = m_obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if(smr.gameObject.layer != nLayer)
            {
                continue;
            }
            foreach (Material material in smr.materials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

                if (alpha >= 0.8f)
                {
                    material.shader = _originShader;
                }
                else
                {
                    material.shader = _replaceShader;
                }

                //当_cutoff大于等于alpha时,该shader会导致模型看不见,从而导致闪烁现象
                if (alpha < _Cutoff + _offset && alpha > 0)
                {
                    material.SetFloat("_Cutoff", alpha - _offset);
                }
                else
                {
                    material.SetFloat("_Cutoff", _Cutoff);
                }

            }

            if (alpha < _Cutoff)
            {
                smr.receiveShadows = false;
                smr.castShadows = false;
            }
            else
            {
                smr.receiveShadows = true;
                smr.castShadows = true;
            }
        }

        MeshRenderer[] mrs = m_obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.gameObject.layer != nLayer)
            {
                continue;
            }
            foreach (Material material in mr.materials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

                if (alpha >= 0.8f)
                {
                    material.shader = _originShader;
                }
                else
                {
                    material.shader = _replaceShader;
                }
                //当_cutoff大于等于alpha时,该shader会导致模型看不见,从而导致闪烁现象
                if (alpha <= _Cutoff + _offset && alpha > 0)
                {
                    material.SetFloat("_Cutoff", alpha - _offset);
                }
                else
                {
                    material.SetFloat("_Cutoff", _Cutoff);
                }
            }

            if (alpha < _Cutoff)
            {
                mr.receiveShadows = false;
                mr.castShadows = false;
            }
            else
            {
                mr.receiveShadows = true;
                mr.castShadows = true;
            }
        }
    }
}

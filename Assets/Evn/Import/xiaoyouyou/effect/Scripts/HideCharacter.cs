using UnityEngine;
using System.Collections;

//该脚本用来实现当某些特效产生效果时隐藏人物,注意必须给人物指定层次
public class HideCharacter : MonoBehaviour 
{

    private int _nCharacterLayer = 8;//玩家层
    public float DelayTime = 0;
    public float LastTime = 2.0f;
    public float FadeSpeed = 0.02f;//淡入淡出速度

    private float _CurrTime;
    private float _alpha;
    private float _originAlpha;//原始alpha

    private Transform _rootTrans;

    private ReplaceShader _pReplaceShader;

	// Use this for initialization
	void Start () 
    {
        _CurrTime = Time.time;

        _rootTrans = transform.root;

        SkinnedMeshRenderer[] smrs = _rootTrans.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if(smr.gameObject.layer == _nCharacterLayer)
            {
                _alpha = smr.material.color.a;
                _originAlpha = _alpha;
                break;
            }
        }

        _pReplaceShader = new ReplaceShader(_rootTrans.gameObject, FadeSpeed);
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time - _CurrTime < DelayTime) return;
        if (Time.time - _CurrTime > DelayTime + LastTime)
        {
            if (_alpha < _originAlpha)
            {
                _alpha += FadeSpeed;
                if(_alpha > _originAlpha)
                {
                    _alpha = _originAlpha;
                }

                _pReplaceShader.UpdateShader(_alpha, _nCharacterLayer);
            }
            return;
        }

        _alpha -= FadeSpeed;
        if(_alpha < 0)
        {
            _alpha = 0;
        }

        _pReplaceShader.UpdateShader(_alpha, _nCharacterLayer);


	}
}

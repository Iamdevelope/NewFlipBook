using UnityEngine;
using System.Collections;

public enum eHideMySelfState
{
    OnlyChildren,//只有子物体被隐藏
    MeAndChildren,//自己和子物体都被隐藏
}

public class HideMyself : MonoBehaviour {

    public float DelayTime = -1;
    public eHideMySelfState _state = eHideMySelfState.MeAndChildren;
    private float _startTime;
	// Use this for initialization
	void Start () 
    {
        _startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (-1 == DelayTime)
        {
            return;
        }
        if (Time.time - _startTime >= DelayTime)
        {
            switch (_state)
            {
                case eHideMySelfState.MeAndChildren:
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case eHideMySelfState.OnlyChildren:
                    {
                        SetChildActive(transform, false);
                    }
                    break;
            }
            DelayTime = -1;
        }
	}

    private void SetChildActive(Transform target, bool active)
    {
        int count = target.childCount;
        Transform temp;
        for (int i = 0; i < count; i++)
        {
            temp = target.GetChild(i);
            //if (temp.childCount > 0)
            //{
            //    SetChildActive(temp, active);
            //}
            temp.gameObject.SetActive(active);
        }
    }
}

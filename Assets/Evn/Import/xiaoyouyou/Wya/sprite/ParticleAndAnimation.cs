/*************************
 * Delay play function
 * LiuYu. 153250945@qq.com
 ************************/
using UnityEngine;
using System.Collections;

public class ParticleAndAnimation : MonoBehaviour
{
#if UNITY_EDITOR
	void Start () 
	{	
		PlayOnce();
	}
#endif
	
	[ContextMenu("Play Loop")]
	public void PlayLoop()
	{
		//PlaySelf(gameObject, true);
		Transform[] trans = GetComponentsInChildren<Transform>(true);
		foreach(Transform t in trans)
		{		
			DelayPlay d = t.GetComponent<DelayPlay>();
			if (d != null)
			{
				if (d.delayTime >= 0)
                {
                    d.InvokePlay(true, false);
                }
			}
			else
			{
                PlaySelf(t.gameObject, true);
			}
		}
	}
	
	[ContextMenu("Play Once")]
	public void PlayOnce () 
	{	
		//PlaySelf(gameObject, false);
		Transform[] trans = GetComponentsInChildren<Transform>(true);
		foreach(Transform t in trans)
		{		
			DelayPlay d = t.GetComponent<DelayPlay>();
			if (d != null)
			{
				if (d.delayTime >= 0)
                {
                    d.InvokePlay(false, false);
                }
			}
			else
			{
                PlaySelf(t.gameObject, false);
			}
		}
	}

	void PlaySelfAndAllChildren(GameObject obj, bool loop)
	{
		if (obj == null)
			return;

		//EnableChildrenAll(obj, true);
		
		ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>(true);
		foreach(ParticleSystem ps in pss)
		{
			ps.loop = loop;
			ps.Clear(true);
			ps.Play();
		}
		
		Animation[] anis = obj.GetComponentsInChildren<Animation>(true);
		foreach(Animation an in anis)
		{           
			an.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
			an.Play();
		}
		
		Animator[] amts = obj.GetComponentsInChildren<Animator>();
		foreach(Animator amt in amts)
		{           
			if (null != amt)
			{         
				#if UNITY_5   
				AnimatorClipInfo[] infs = amt.GetCurrentAnimatorClipInfo(0);
				foreach (AnimatorClipInfo info in infs)
				{
					info.clip.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
					amt.Play(info.clip.name, -1, 0);
					break;
				}
				#else
				AnimatorClipInfo[] infs = amt.GetCurrentAnimatorClipInfo(0);
				foreach (AnimatorClipInfo info in infs)
				{
					info.clip.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
					amt.Play(info.clip.name, -1, 0);
					break;
				}
				#endif
			}	
		}
	}

	void PlaySelf(GameObject obj, bool loop)
	{
		if (obj == null)
			return;

		ParticleSystem ps = obj.GetComponent<ParticleSystem>();
		if (null != ps)
		{
			ps.loop = loop;
			ps.Clear(true);
			ps.time = 0f;
			ps.Play();
		}

		Animation anim = obj.GetComponent<Animation>();
		if (null != anim)
		{           
			anim.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
			anim.Play();
		}
		
		Animator amt = obj.GetComponent<Animator>();
		if (null != amt)
		{            
		    #if UNITY_5
			AnimatorClipInfo[] infs = amt.GetCurrentAnimatorClipInfo(0);
			foreach (AnimatorClipInfo info in infs)
			{
				info.clip.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
				amt.Play(info.clip.name, -1, 0);
				break;
			}
			#else			
			AnimatorClipInfo[] infs = amt.GetCurrentAnimatorClipInfo(0);
			foreach (AnimatorClipInfo info in infs)
			{
				info.clip.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
				amt.Play(info.clip.name, -1, 0);
				break;
			}
			#endif
		}

		//EnableChildrenAll(obj, true);
	}

	
	public void EnableChildrenAll(GameObject obj, bool enable)
	{		
		if (obj == null)
			return;

		for (int i = obj.transform.childCount - 1; i >= 0; i--) 
		{			
			obj.transform.GetChild(i).gameObject.SetActive(enable);			
		}
	}
}

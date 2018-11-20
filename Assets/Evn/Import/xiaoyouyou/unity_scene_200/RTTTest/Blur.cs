using UnityEngine;
using System.Collections;
using System;

public class Blur : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(brightShader == null)
			return ;
		
		brightMat = new Material(brightShader);
		BlurMat		= new Material(BlurShader);
	}
	
	// Update is called once per frame
//	void Update () {
//		
//		double value1 = GaussianBlur(-1,1,1.5f);
//		double value2 = GaussianBlur(0,1,1.5f);
//		double value3 = GaussianBlur(1,1,1.5f);
//		double value4 = GaussianBlur(-1,0,1.5f);
//		double value5 = GaussianBlur(0,0,1.5f);
//		double value6 = GaussianBlur(1,0,1.5f);
//		double value7 = GaussianBlur(-1,-1,1.5f);
//		double value8 = GaussianBlur(0,-1,1.5f);
//		double value9 = GaussianBlur(1,-1,1.5f);
//		
//		double value1 = GaussianBlur(-1,1,1f);
//		double value2 = GaussianBlur(0,1,1f);
//		double value3 = GaussianBlur(1,1,1f);
//		double value4 = GaussianBlur(-1,0,1f);
//		double value5 = GaussianBlur(0,0,1f);
//		double value6 = GaussianBlur(1,0,1f);
//		double value7 = GaussianBlur(-1,-1,1f);
//		double value8 = GaussianBlur(0,-1,1f);
//		double value9 = GaussianBlur(1,-1,1f);
//		
//		double total = value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8 + value9;
//		
//		double realValue = value1 / total;
//		double realValue1 = value2 / total;
//		
//	
//	}
	
	public float Hold	= 0.5f;
	
	public Shader brightShader;
	public Shader BlurShader;
	
	private Material brightMat;
	private Material BlurMat;
	
//	public static double GaussianBlur(float x,float y,float a)
//	{
//		float temp =  -1.0f * (x*x + y*y) / (2 * a*a);
//		double value = (1.0f / (2 * 3.1415926 * a * a) ) * Mathf.Exp(temp);
//		return value;
//	}
	
	void OnRenderImage(RenderTexture src,RenderTexture target)
	{
		if(brightMat == null)
			return ;
		
		RenderTexture tempBrightRT = RenderTexture.GetTemporary(src.width,src.height);
		RenderTexture tempBlurRT = RenderTexture.GetTemporary(src.width,src.height);
		
		brightMat.SetFloat("_Hold",Hold);
		Graphics.Blit(src,tempBrightRT,brightMat);	
		
		float offset = 1.0f / src.width;
		BlurMat.SetVector("offsets",new Vector4(offset,offset,offset,offset));
		Graphics.Blit(tempBrightRT,tempBlurRT,BlurMat);	
		Graphics.Blit(tempBlurRT,target);
		
		RenderTexture.ReleaseTemporary (tempBrightRT);
		RenderTexture.ReleaseTemporary (tempBlurRT);		
	}
	
}

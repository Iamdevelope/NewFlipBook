/*************************
 * EffectRotate function
 * LiuYu. 153250945@qq.com
 ************************/
using UnityEngine;

public class EffectRotate : MonoBehaviour
{
	public float SpeedX;
	public float SpeedY;
	public float SpeedZ;

	private Transform mTransform;

	void Awake()
	{
		mTransform = transform;
	}

	void Update()
	{
		float deltaTime = Time.unscaledDeltaTime;
		float x = SpeedX * deltaTime;
		float y = SpeedY * deltaTime;
		float z = SpeedZ * deltaTime;
		if (mTransform != null)
		{
			mTransform.Rotate(x, y, z);
		}
	}
}



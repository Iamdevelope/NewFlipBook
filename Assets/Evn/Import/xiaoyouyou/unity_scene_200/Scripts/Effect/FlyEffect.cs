using UnityEngine;
using System.Collections;

public class FlyEffect : MonoBehaviour {
	
	public float m_fInterference = 1.0f;
	
	private Vector3 m_v3TargetPos = Vector3.zero;
	
	private Vector3 m_v3SourcePos = Vector3.zero;
	
	private float m_fCurrentValue = 0.0f;
	
	public float m_fSpeed = 1.0f;
	
	private Vector3 m_v3RandomDir = Vector2.zero;
	
	public float m_fAddSpeedDistance = 700.0f;
	
	private bool m_bInitForce = false;
	
	public float m_fTimeForceSpeed = 1.0f;
	
	private float m_fTimeForce = 0.001f;
	
	private uint m_uiSoulValue = 0;
	
	// Use this for initialization
	void Start () {
		GameObject.Destroy(gameObject,4.0f );
	}
	
	// Update is called once per frame
	void FixedUpdate  () {
		
		m_fTimeForce += Time.fixedDeltaTime*m_fTimeForceSpeed*(1.0f+m_fTimeForce);
		
		Vector3 targetDir = m_v3TargetPos - transform.position;
		
		//power 0 - 1
		float powerAdd = (m_fAddSpeedDistance - Mathf.Min(targetDir.magnitude,m_fAddSpeedDistance) )/m_fAddSpeedDistance;
		
		if( m_bInitForce )
		{
			GetComponent<Rigidbody>().AddForce(targetDir.normalized*powerAdd*m_fSpeed*m_fTimeForce );
		}else
		{
			GetComponent<Rigidbody>().AddForce( m_v3RandomDir);
			m_bInitForce = true;
		}
		
		if(30.0f > Vector3.Distance(transform.position,m_v3TargetPos) )
		{
			GameObject.Destroy(gameObject );
			
			XEventManager.SP.SendEvent(EEvent.FightHead_Show_Soul_Explosion,transform.position ,m_uiSoulValue );
		}
	}
	
	public void fireEffect(Vector3 targetPos,uint soulValue ) {
        m_v3TargetPos = targetPos;
		
		m_v3SourcePos = transform.position;
		
		m_v3SourcePos.z = m_v3TargetPos.z;
		
		Quaternion rotation = Quaternion.Euler(0, 30, 0);
		
		Vector3 targetDir = m_v3TargetPos - m_v3SourcePos;
		targetDir.Normalize();
		
		if(0== Time.frameCount%2 )
		{
			m_v3RandomDir = Vector3.Cross(targetDir,Vector3.back );
		}else
		{
			m_v3RandomDir = Vector3.Cross(targetDir,Vector3.forward );
		}
		
		m_v3RandomDir*=Random.Range(m_fInterference*0.5f,m_fInterference*1.5f );
		
		m_uiSoulValue = soulValue;
	}
	
}

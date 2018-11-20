using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class MotionTrail : MonoBehaviour {

	public Transform target1;
	public Transform target2;

	//public int sampleRate =1;
	int sampleClick =0;
	public float slerpSegmentMaxDistan = 0.05f;//��С��ֵ�ֶεľ����ƽ��

	List<Vertex> mVertex = new List<Vertex>();
	//GameObject trailObj=null;
	Mesh mesh = null;
	 Material _material;

	public int MaxSegment = 10;
	public int DisappearFactor = 5;

	void OnDisable()
	{
		//GameObject.DestroyImmediate(trailObj);
	}
	MeshRenderer mMR;
	// Use this for initialization
	void Start () {
		mMR = GetComponent<MeshRenderer>();
		if (mMR==null)
		{
			Debug.LogError("mMR==null fatal error !!!");
			return;
		}
	_material = mMR.sharedMaterial;
	//trailObj = new GameObject("MotionTrail");
    //trailObj.transform.parent = this.transform;
	//trailObj.transform.parent = null;
	//trailObj.transform.position = Vector3.zero;
   // trailObj.transform.rotation = Quaternion.identity;
// 	MeshFilter meshFilter = (MeshFilter) trailObj.AddComponent(typeof(MeshFilter));
// 	mesh = meshFilter.mesh;
	mesh = new Mesh();

//     trailObj.AddComponent(typeof(MeshRenderer));
// 	trailObj.renderer.sharedMaterial = _material;

	}
	
	// Update is called once per frame
	void Update () {
		
 		//UpdateMesh();

		//Debug.Log("mVertex.Count="+mVertex.Count);
		//DebugMesh();

// 				 		SampleVertex(target1, target2);
// 				 		UpdateMesh();

	}

	void LateUpdate()
	{
		
	}

	void OnGUI()
	{
		 		SampleVertex(target1, target2);
		 		UpdateMesh();
				
	}

	//[RenderBeforeQueues(4000)]
	void OnRenderObject(/*int queueIndex*/)
	{
		RenderTrail();
	}

	void RenderTrail()
	{
		_material = mMR.sharedMaterial;
		if(_material==null)
		{
			_material = mMR.sharedMaterial;
		}
		else
		{
			if(mesh!=null)
			{
				for(int i=0;i<_material.passCount;i++)
				{
					if (_material.SetPass(i))
						Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
				}
			  
			}
		}
	}

	void FixedUpdate()
	{
	//	SampleVertex(target1, target2);
	//	UpdateMesh();
	}

	void DebugMesh()
	{
		MeshFilter tmpMesh = GetComponent<MeshFilter>();
		foreach (Vector3 tmp in tmpMesh.mesh.vertices)
		{
			Debug.DrawRay(tmpMesh.transform.TransformPoint(tmp), Vector3.up * 0.5f);
			//Debug.Log(tmp);
		}
		
	}

	void UpdateMesh()
	{
		if(mVertex.Count<4)
			return;
		//Debug.Log("update mesh");
		if (mesh != null)
		mesh.Clear();
		Vector3[] mVertices = new Vector3[mVertex.Count];
		Vector2[] mUv = new Vector2[mVertex.Count];
// 		mesh.vertices = new Vector3[mVertex.Count];
// 		mesh.uv = new Vector2[mVertex.Count];
		int[] triangles = new int[(mVertex.Count-2)*3];
		for (int i=0;i<mVertex.Count;i++)
		{
//			mesh.vertices[i] = new Vector3(0.0f,0.0f,0.0f);
// 			mesh.vertices[i].x = mVertex[i].position.x;
// 			mesh.vertices[i].y = mVertex[i].position.y;
// 			mesh.vertices[i].z = mVertex[i].position.z;
//			mesh.uv[i] = new Vector2(0.0f, 0.0f);
			mVertices[i] = new Vector3(0.0f, 0.0f, 0.0f);
			mVertices[i] = mVertex[i].position;
			//Debug.Log("mesh.vertices " + i + "=" + mVertices[i] + "mVertex[i].position=" + mVertex[i].position);
			
			if(i%2 ==0)
			{
				mUv[i].x = 0.0f;
				mUv[i].y = (((float)i) / (mVertex.Count - 2));
			}
			else
			{
				mUv[i].x = 1.0f;
				mUv[i].y = (((float)(i - 1)) / (mVertex.Count - 2));
			}

			
		}

		for(int j=0;j<mVertex.Count-2;j++)
		{
			if(j%2==0)
			{
				triangles[j*3] = j;
				triangles[j*3+1] =j+1;
				triangles[j*3+2] = j+3;
			}
			else
			{
				triangles[j * 3] = j - 1;
				triangles[j*3+1] =j+1;
				triangles[j*3+2] = j+2;
			}
		}
		mesh.vertices = mVertices;
		mesh.uv = mUv;
		mesh.triangles = triangles;

		//Debug.Log("lastVertexUV = " + mesh.uv[mesh.vertexCount - 1] + " preLastUV=" + mesh.uv[mesh.vertexCount - 2]);
		//DebugMesh();
	}

	void SampleVertex(Transform trans1,Transform trans2)
	{
		while(mVertex.Count>MaxSegment*2)
		{
			//Debug.Log("   !!!mVertex.Count=" + mVertex.Count);
			mVertex.RemoveAt(0);
			mVertex.RemoveAt(1);
		}



		if (mVertex.Count >= 4)
			//SlerpVector(trans1.position, trans2.position,mVertex[mVertex.Count - 2].position, mVertex[mVertex.Count - 1].position );
			if (!SlerpVector(mVertex[mVertex.Count - 2].position, mVertex[mVertex.Count - 1].position, trans1.position, trans2.position))
				return;

		Vertex tmpV1 = new Vertex();
		//tmpV1.position = trailObj.transform.InverseTransformPoint(trans1.position);
		tmpV1.position = trans1.position;
		tmpV1.uv = Vector2.zero;

		Vertex tmpV2 = new Vertex();
		//tmpV2.position = trailObj.transform.InverseTransformPoint(trans2.position);
		tmpV2.position = trans2.position;
        tmpV2.uv = Vector2.zero;

		

		mVertex.Add(tmpV1);
		mVertex.Add(tmpV2);
		//Debug.Log("mVertex.Count=" + mVertex.Count);
		//Debug.Log("trans1=" + trans1.position + "trans2=" + trans2.position + "tmpV1.position=" + tmpV1.position + "tmpV2.position=" + tmpV2.position);
	}


	bool SlerpVector(Vector3 fromPos1,Vector3 fromPos2,Vector3 toPos1,Vector3 toPos2)
	{
		float tmpDistan1 = Vector3.Magnitude(toPos1-fromPos1);
		float tmpDistan2 = Vector3.Magnitude(toPos2 - fromPos2);
		float tmpDistan = (tmpDistan1>tmpDistan2) ? tmpDistan1:tmpDistan2;
		if(   slerpSegmentMaxDistan < tmpDistan)
		{
			int segments = (int)(tmpDistan / slerpSegmentMaxDistan)+1;
			for (int i = 1; i < segments;i++ )
			{
				while (mVertex.Count > MaxSegment * 2)
				{
					//Debug.Log("   !!!mVertex.Count=" + mVertex.Count);
					mVertex.RemoveAt(0);
					mVertex.RemoveAt(1);
				}

				Vertex tmpLerpV1 = new Vertex();
				tmpLerpV1.position = Vector3.Slerp(fromPos1, toPos1, i / (float)segments);
				tmpLerpV1.uv = Vector2.zero;

				Vertex tmpLerpV2 = new Vertex();
				tmpLerpV2.position = Vector3.Slerp(fromPos2, toPos2, i / (float)segments);
				tmpLerpV2.uv = Vector2.zero;
				mVertex.Add(tmpLerpV1);
				mVertex.Add(tmpLerpV2);
			}
			return true;
		}
		else
		{
			//Debug.Log("SlerpVector  no point add");
// 			mVertex.RemoveAt(0);
// 			mVertex.RemoveAt(1);
// 
// 			    for (int j = 0; j < mVertex.Count; j++)
// 				{
// 					mVertex.RemoveAt(0);
// 					mVertex.RemoveAt(1);
// 				}

			if (mVertex.Count > (DisappearFactor*2))
			{
				for (int j = 0; j < DisappearFactor; j++)
				{
					mVertex.RemoveAt(0);
					mVertex.RemoveAt(1);
				}
			}


				return false;
		}

		return false;
		
	}

	class Vertex{
		public Vector3 position;
		public Vector2 uv;
		public Vertex()
		{
			//position = new Vector3(0.0f,0.0f,0.0f);
			//uv = new Vector2(0.0f,0.0f);
		}
	}



}

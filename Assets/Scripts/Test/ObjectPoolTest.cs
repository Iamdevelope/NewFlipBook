using PJW;
using PJW.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTest : MonoBehaviour {

    private ObjectPoolManager objectPoolManager;
    public GameObject cube;
    public GameObject sphere;
    private IObjectPool<Cube> cubePool;
    Cube temp;
    Cube sphe;

    void Start () {
        temp = new Cube("cube", cube, 60, false);
        sphe = new Cube("sphere", sphere, 60, false);
        objectPoolManager = new ObjectPoolManager();
        cubePool = objectPoolManager.CreateMultiSpawnObjectPool<Cube>("cube", 10, 60);
        cubePool.CreateObject(temp, true);
        cubePool.CreateObject(sphe, true);
        Debug.Log(cubePool.GetName);
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = Instantiate((GameObject)cubePool.Spawn("cube").GetTarget);
            
            StartCoroutine(UnSpawn(go));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject go = Instantiate((GameObject)cubePool.Spawn("sphere").GetTarget);

            StartCoroutine(UnSpawn(go));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            cubePool.Release();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            cubePool.Unspawn(temp);
        }
	}

    private IEnumerator UnSpawn(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        cubePool.Unspawn(go);
    }
}



public class Cube : ObjectBase
{
    public Cube(string name) : base(name)
    {

    }
    public Cube(string name,object target,int priority,bool isLock) : base(name, target, priority, isLock)
    {
        
    }
    protected internal override void OnSpawn()
    {
        Debug.Log(" the cube is spawn ");
    }

    protected internal override void UnSpawn()
    {
        GameObject.Destroy((GameObject)GetTarget);
        Debug.Log(" the cube is upspawn ");
    }

    protected internal override void Release(bool isShutdown)
    {
        GameObject.Destroy((GameObject)GetTarget);
        Debug.Log("释放了");
    }
}

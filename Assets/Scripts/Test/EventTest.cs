using System;
using System.Collections;
using System.Collections.Generic;
using PJW.Event;
using UnityEngine;

public class EventTest : MonoBehaviour {

	private EventManager eventManager;
	private void Start() {
		eventManager=new EventManager();
		eventManager.Subscribe(1,EventHandler);
		eventManager.Fire(this,new EventAvgsTest());
	}
	private void Update() {
		if(Input.GetKeyDown(KeyCode.C)){
			eventManager.Shutdown();
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			eventManager.Update(1,Time.deltaTime);
		}
	}

    private void EventHandler(object sender, GameEventAvgs e)
    {
		Debug.Log(sender.ToString()+e.Id);
    }
}
public class EventAvgsTest : GameEventAvgs
{
    public override int Id
    {
        get
        {
            return 10;
        }
    }

    public override void Clear()
    {
        Debug.Log(" 清空了 ");
    }
}
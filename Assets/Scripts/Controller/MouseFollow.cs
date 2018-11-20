using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour {
    public float depth = 10.0f;
    
    //private void Update()
    //{
    //    if (GameObject.FindWithTag("3DCam").GetComponent<Camera>().enabled == false)
    //    {
    //        Vector2 mousePos = Input.mousePosition;
    //        Vector3 wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
    //        transform.position = wantedPos;
    //    }
    //}
}

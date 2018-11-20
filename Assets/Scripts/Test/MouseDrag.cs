using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {
    private void OnMouseDrag()
    {
        Debug.Log(Input.mousePosition);
    }
    private void OnMouseDown()
    {
        Debug.Log("mouse on down ");
    }
}

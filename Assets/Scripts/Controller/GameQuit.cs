using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameQuit : MonoBehaviour {
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
